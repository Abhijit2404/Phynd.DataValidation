using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phynd.DataValidation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Controllers
{
    [Authorize]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IS3FileUpload _service;
        public UploadController(IS3FileUpload service)
        {
            _service = service;
        }
        [HttpPut]
        [AllowAnonymous]
        [RequestSizeLimit(10000000000)]
        [Route("Upload")]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            try
            {
                var response = await _service.AddFileAsync(file);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
