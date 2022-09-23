using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class Response<T>
    {
        public Response(bool isSucceeded, string message, T content)
        {
            Success = isSucceeded;
            Message = message;
            Content = content;
        }
        public T Content { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
