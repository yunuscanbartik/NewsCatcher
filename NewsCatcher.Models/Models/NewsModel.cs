namespace NewsCatcher.Models.Models
{
    public class NewsModel
    {
        public class BrowseModel
        {
            public class Request
            {
                public int NewsId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int NewsId { get; set; }
                public string Title { get; set; }
                public string Content { get; set; }
                public string Summary { get; set; }
                public int CategoryId { get; set; }
                public DateTime ShareDate { get; set; }
                public string SourceName { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }
        public class CreateModel
        {
            public class Request
            {
                public string Title { get; set; }
                public string Content { get; set; }
                public string Summary { get; set; }
                public int CategoryId { get; set; }
                public string SourceName { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int NewsId { get; set; }
                public string Title { get; set; }
                public string Content { get; set; }
                public string Summary { get; set; }
                public int CategoryId { get; set; }
                public DateTime ShareDate { get; set; }
                public string SourceName { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }

        public class UpdateModel
        {
            public class Request
            {
                public int NewsId { get; set; }
                public string Title { get; set; }
                public string Content { get; set; }
                public string Summary { get; set; }
                public int CategoryId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int NewsId { get; set; }
                public string Title { get; set; }
                public string Content { get; set; }
                public string Summary { get; set; }
                public int CategoryId { get; set; }
                public DateTime ShareDate { get; set; }
                public string SourceName { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }
        public class DeleteModel
        {
            public class Request
            {
                public int NewsId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int NewsId { get; set; }
                public string Title { get; set; }
                public string Content { get; set; }
                public string Summary { get; set; }
                public int CategoryId { get; set; }
                public DateTime ShareDate { get; set; }
                public string SourceName { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }
    }
}
