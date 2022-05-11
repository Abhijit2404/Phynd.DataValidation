using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Phynd.DataValidation.Data;
using Phynd.DataValidation.Interface;
using Phynd.DataValidation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Services
{
    public class S3FileUpload : IS3FileUpload
    {
        private const string bucketName = "dvap-poc/input";
        private static IAmazonS3 _s3Client;
        public S3FileUpload(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        public async Task<S3ApiResponse> AddFileAsync(IFormFile file)
        {
            List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

            if (file == null || file.Length == 0)
            {
                return new S3ApiResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "File is null or empty"
                };
            }

            InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = bucketName,
                Key = file.FileName
            };

            InitiateMultipartUploadResponse initResponse =
                await _s3Client.InitiateMultipartUploadAsync(initiateRequest);

            //File Operation
            byte[] fileBytes = new Byte[file.Length];
            var path = Path.Combine(Directory.GetCurrentDirectory(), file.FileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(stream);
            }
            long contentLength = new FileInfo(file.FileName).Length;
            long partSize = 30 * (long)Math.Pow(2, 20);

            //AWS Official Low-level API Implementation for Multipart Upload
            try
            {
                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {
                    UploadPartRequest uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = file.FileName,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = partSize,
                        FilePosition = filePosition,
                        FilePath = file.FileName
                    };

                    uploadRequest.StreamTransferProgress +=
                        new EventHandler<StreamTransferProgressArgs>(UploadPartProgress);

                    uploadResponses.Add(await _s3Client.UploadPartAsync(uploadRequest));

                    filePosition += partSize;
                }

                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    UploadId = initResponse.UploadId
                };

                completeRequest.AddPartETags(uploadResponses);

                CompleteMultipartUploadResponse completeUploadResponse =
                    await _s3Client.CompleteMultipartUploadAsync(completeRequest);

                return new S3ApiResponse
                {
                    Status = HttpStatusCode.OK,
                    Message = "File uploaded to S3",
                    Location = completeUploadResponse.Location,
                    ETag = completeUploadResponse.ETag,
                };

            }
            catch (AmazonS3Exception e)
            {
                return new S3ApiResponse
                {
                    Status = e.StatusCode,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("An AmazonS3Exception was thrown: {0}", e.Message);

                AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    UploadId = initResponse.UploadId
                };
                await _s3Client.AbortMultipartUploadAsync(abortMPURequest);
            }

            return new S3ApiResponse
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Error. Failed to upload File"
            };
        }

        public static void UploadPartProgress(object sender, StreamTransferProgressArgs e)
        {
            Console.WriteLine("{0}/{1}, {2}% done", e.TransferredBytes, e.TotalBytes, e.PercentDone);
        }
    }
}
