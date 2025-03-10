using System;
using Microsoft.AspNetCore.Http;
using PizzaShop.Repository.Models;
using PizzaShop.Repository.ModelView;

namespace PizzaShop.Service.Interfaces;

public interface IMenuService
{
    Task<MenuWithItemsViewModel> GetAllCategory(int? categoryId = null, string? searchTerm = null, int pageNumber = 1, int pageSize = 5);
    Task<List<Modifiergroup>> GetAllModifier(int? modifierId = null, string? searchModifier = null);
    Task AddCategoryService(MenuWithItemsViewModel model);
    Task AddModifierGroupService(MenuWithItemsViewModel model);
    Task EditCategoryService(MenuWithItemsViewModel model);
    Task DeleteCategoryService(MenuWithItemsViewModel model);
    Task DeleteModifierGroupService(MenuWithItemsViewModel model);
    Task DeleteItemService(MenuWithItemsViewModel model);
    Task AddItemAsync(MenuWithItemsViewModel viewModel, IFormFile? uploadFile, int userId);
    Task UpdateItemAsync(MenuWithItemsViewModel viewModel, IFormFile? uploadFile, int userId);
    Task<Item> GetItemById(int id);
}
