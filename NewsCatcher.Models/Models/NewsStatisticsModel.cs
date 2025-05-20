namespace NewsCatcher.Models.Models
{
    public class NewsStatisticsModel
    {

        public class BrowseModel
        {
            public class Request
            {
                public int? NewsId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData?> Data { get; set; }
            }
            public class ReturnData
            {
                public int? NewsStatisticId { get; set; }
                public int? NewsId { get; set; }
                public int? ViewCount { get; set; }
                public int? ReadCount { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
            }
        }
    }
}
