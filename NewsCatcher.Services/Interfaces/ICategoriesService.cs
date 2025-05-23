﻿using NewsCatcher.Models.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<CategoriesModel.BrowseModel.Return> GetCategoriesAsync(CategoriesModel.BrowseModel.Request request);
        Task<CategoriesModel.BrowseModel.Return> GetCategoryByIdAsync(CategoriesModel.BrowseModel.Request request);
        Task<CategoriesModel.CreateModel.Return> AddCategoryAsync(CategoriesModel.CreateModel.Request request);
        Task<CategoriesModel.UpdateModel.Return> UpdateCategoryAsync(CategoriesModel.UpdateModel.Request request);
        Task<CategoriesModel.DeleteModel.Return> DeleteCategoryAsync(CategoriesModel.DeleteModel.Request request);
    }
}
