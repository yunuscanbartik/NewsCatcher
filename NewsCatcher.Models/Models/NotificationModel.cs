namespace NewsCatcher.Models.Models
{
    public class NotificationModel
    {
        public class DeleteModel
        {
            public class Request
            {
                public int NotificationId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int NotificationId { get; set; }
                public int UserId { get; set; }
                public string Message { get; set; }
                public bool IsRead { get; set; }
                public DateTime SendDate { get; set; }
            }
        }

        public class BrowseModel
        {
            public class Request
            {
                public int? UserId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? NotificationId { get; set; }
                public int? UserId { get; set; }
                public string? Message { get; set; }
                public bool? IsRead { get; set; }
                public DateTime? SendDate { get; set; }
            }
        }

        public class CreateModel
        {
            public class Request
            {
                public int? UserId { get; set; }
                public string? Message { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int? NotificationId { get; set; }
                public int? UserId { get; set; }
                public string? Message { get; set; }
                public bool? IsRead { get; set; }
                public DateTime? SendDate { get; set; }
            }
        }

        public class NotificationReadModel
        {
            public class Request
            {
                public int? NotificationId { get; set; }
                public bool? IsRead { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? NotificationId { get; set; }
                public int? UserId { get; set; }
                public string? Message { get; set; }
                public bool? IsRead { get; set; }
                public DateTime? SendDate { get; set; }
            }
        }
    }
}
