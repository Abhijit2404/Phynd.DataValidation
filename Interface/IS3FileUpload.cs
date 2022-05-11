using Microsoft.AspNetCore.Http;
using Phynd.DataValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Interface
{
    public interface IS3FileUpload
    {
        Task<S3ApiResponse> AddFileAsync(IFormFile file);
    }
}
