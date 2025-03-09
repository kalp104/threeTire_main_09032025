using System;
using Microsoft.AspNetCore.Http;
using PizzaShop.Repository.Models;
using PizzaShop.Repository.ModelView;

namespace PizzaShop.Service.Interfaces;

public interface IMenuService
{
    Task<MenuWithItemsViewModel> GetAllCategory(int? categoryId = null, string? searchTerm = null);
    Task AddCategoryService(MenuWithItemsViewModel model);
    Task EditCategoryService(MenuWithItemsViewModel model);
    Task DeleteCategoryService(MenuWithItemsViewModel model);
    Task DeleteItemService(MenuWithItemsViewModel model);
    Task AddItemAsync(MenuWithItemsViewModel viewModel, IFormFile? uploadFile, int userId);
    Task UpdateItemAsync(MenuWithItemsViewModel viewModel, IFormFile? uploadFile, int userId);
    Task<Item> GetItemById(int id);
}
