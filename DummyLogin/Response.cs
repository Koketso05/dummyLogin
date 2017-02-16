using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DummyLogin
{
    public class Response
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}