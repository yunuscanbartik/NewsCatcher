namespace NewsCatcherApi.Models
{
    public class CategoriesModel
    {
        public int CategorieId { get; set; }
        public string CategorieName { get; set; }
        public string CategorieDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
