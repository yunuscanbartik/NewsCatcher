namespace NewsCatcher.Models.Models
{
    public class TagsModel
    {
        public class BrowseModel
        {
            public class Request
            {
                public int TagId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData> Data { get; set; }
            }
            public class ReturnData
            {
                public int TagId { get; set; }
                public string TagName { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }
        public class CreateModel
        {
            public class Request
            {
                public string TagName { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int TagId { get; set; }
                public string TagName { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }

        public class UpdateModel
        {
            public class Request
            {
                public int TagId { get; set; }
                public string TagName { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int TagId { get; set; }
                public string TagName { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }

        public class DeleteModel
        {
            public class Request
            {
                public int TagId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int TagId { get; set; }
                public string TagName { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }
    }
}
