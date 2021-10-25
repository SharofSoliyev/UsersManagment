using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersManagment.Businees.Models
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            StatusCode = ResponseStatusCode.Ok;
        }

        public ResponseModel(object data)
            : this()
        {
            Data = data;
        }

        public ResponseModel(string message)
            : this()
        {
            Message = message;
        }

        public ResponseModel(ResponseStatusCode statusCode, string message)
            : this(message)
        {
            StatusCode = statusCode;
        }

        public ResponseModel(object data, ResponseStatusCode statusCode = ResponseStatusCode.Ok, string message = "")
            : this(statusCode, message)
        {
            Data = data;
        }

        public object Data { get; set; }
        public ResponseStatusCode StatusCode { get; set; }
        public string Message { get; set; }

    }
    public enum ResponseStatusCode
    {
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalServerError = 500
    }
}
