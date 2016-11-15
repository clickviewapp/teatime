namespace TeaTime.Models
{
    using System;
    using System.Net;

    public class ApiResponse
    {
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
        public Exception Exception { get; set; }
    }
}
