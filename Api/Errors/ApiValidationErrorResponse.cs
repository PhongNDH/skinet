using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        public IEnumerable<string>? Errors {get; set;}
        //It's designed to be used when there are validation errors in API requests, and it includes an HTTP status code of 400 (Bad Request) along 
        //with a collection of error messages in the Errors property
    }
}