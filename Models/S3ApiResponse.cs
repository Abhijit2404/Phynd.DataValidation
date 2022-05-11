using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Phynd.DataValidation.Models
{
    public class S3ApiResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
        public string ETag { get; set; }
    }
}
