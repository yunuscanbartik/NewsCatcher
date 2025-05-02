namespace NewsCatcherApi.Models
{
    public class NewsStatisticsModel
    {
        public int NewStaticId { get; set; }
        public int NewsId { get; set; }
        public int ViewCount { get; set; }
        public int MyProperty { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
