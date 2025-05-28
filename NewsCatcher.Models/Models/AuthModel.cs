using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Models.Models
{
    public  class AuthModel
    {
        public class VerifyOtp
        {
            public class Request
            {
                public string? Email { get; set; }
                public string? VerificationCode { get; set; }
            }

            public class Return : ReturnModel
            {
                public List<ReturnData> Data { get; set; }
            }

            public class ReturnData
            {
                public string? JwtCode { get; set; }
            }
        }       
    }
}
