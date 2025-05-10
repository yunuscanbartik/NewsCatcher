namespace NewsCatcherApi.Models
{
    public class UsersModel
    {
        public class LoginModel
        {
            public class Request 
            {
                public string Email { get; set; }
                public string VerificationCode { get; set; }
            }

            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }

            public class ReturnData
            {
                public string UserName { get; set; }
                public string Email { get; set; }
            }
        }
    }
}
