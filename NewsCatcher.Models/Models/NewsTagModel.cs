namespace NewsCatcher.Models.Models
{
    public class NewsTagModel
    {
        public class CreateModel
        {
            public class Request
            {
                public int? NewsId { get; set; }
                public int? TagId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? NewsTagId { get; set; }
                public int? NewsId { get; set; }
                public int? TagId { get; set; }
            }
        }
    }
}
