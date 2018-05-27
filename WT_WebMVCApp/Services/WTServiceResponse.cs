using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WT_WebMVCApp.Services
{
    public class WTServiceResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseMessage { get; set; }
        public T ViewModel { get; set; }
    }
}
