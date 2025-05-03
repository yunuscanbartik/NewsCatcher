namespace NewsCatcherApi.Models
{
    public class CategoriesModel
    {


        public class BrowseModel
        {
            public class Request
            {
                public int CategorieId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int CategorieId { get; set; }
                public string CategorieName { get; set; }
                public string CategorieDescription { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }

        public class CreateModel
        {
            public class Request
            {
                public string CategorieName { get; set; }
                public string CategorieDescription { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int CategorieId { get; set; }
                public string CategorieName { get; set; }
                public string CategorieDescription { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }

        public class UpdateModel
        {
            public class Request
            {
                public int CategorieId { get; set; }
                public string CategorieName { get; set; }
                public string CategorieDescription { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int CategorieId { get; set; }
                public string CategorieName { get; set; }
                public string CategorieDescription { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }

        public class DeleteModel 
        { 
            public class Request
            {
                public int CategorieId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int CategorieId { get; set; }
                public string CategorieName { get; set; }
                public string CategorieDescription { get; set; }
                public DateTime CreatedDate { get; set; }
                public DateTime UpdatedDate { get; set; }
            }
        }
    }
}
