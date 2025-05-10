namespace NewsCatcher.Models.Models
{
    public class NewsStatisticsModel
    {

        public class BrowseModel
        {
            public class Request
            {
                public int NewsStatisticsId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int NewsStatisticsId { get; set; }
                public int NewStaticId { get; set; }
                public int NewsId { get; set; }
                public int ViewCount { get; set; }
                public int MyProperty { get; set; }
                public DateTime CreatedDate { get; set; }
            }
        }
    }
}
