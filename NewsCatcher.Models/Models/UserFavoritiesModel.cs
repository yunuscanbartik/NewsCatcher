namespace NewsCatcherApi.Models
{
    public class UserFavoritiesModel
    {
        public class BrowseModel
        {
            public class Request
            {
                public int UserFavoritiesId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int UserFavoritiesId { get; set; }
                public int UserId { get; set; }
                public int NewsId { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }

        public class CreateModel
        {
            public class Request
            {
                public int UserId { get; set; }
                public int NewsId { get; set; }
            }
            public class Return:ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int UserFavoritiesId { get; set; }
                public int UserId { get; set; }
                public int NewsId { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }


        public class UpdateModel
        {
            public class Request
            {
                public int UserFavoritiesId { get; set; }
                public int NewsId { get; set; }
            }
            public class Return :ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int UserFavoritiesId { get; set; }
                public int UserId { get; set; }
                public int NewsId { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }

        public class DeleteModel
        {
            public class Request
            {
                public int UserFavoritiesId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int UserFavoritiesId { get; set; }
                public int UserId { get; set; }
                public int NewsId { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }
    }
}
