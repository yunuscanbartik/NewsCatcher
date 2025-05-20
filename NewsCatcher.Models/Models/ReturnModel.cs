namespace NewsCatcher.Models.Models
{
    public class ReturnModel
    {
        public bool Status { get; set; } = false;
        public string? Message { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? RequestId { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? RequestTime { get; set; }
        public DateTime? ResponseTime { get; set; }
    }
}
