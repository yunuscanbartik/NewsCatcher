namespace NewsCatcherApi.Models
{
    public class UsersModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string VerificationCode { get; set; }
        public DateTime VerificationCodeExpiration { get; set; }
        public bool IsUsed { get; set; }
    }
}
