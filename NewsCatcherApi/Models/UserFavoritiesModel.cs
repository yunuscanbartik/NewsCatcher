namespace NewsCatcherApi.Models
{
    public class UserFavoritiesModel
    {
        public int UserFavoritiesId { get; set; }
        public int UserId { get; set; }
        public int NewsId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
