using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzaShop.Core.Filters;
using PizzaShop.Repository.Models;
using PizzaShop.Repository.ModelView;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Core.Controllers;

[Authorize]
[ServiceFilter(typeof(AuthorizePermissionMenu))]
public class MenuController : Controller
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly IUserTableService _userTableService;
    private readonly IRoleService _roleService;
    private readonly IMenuService _menuService;

    public MenuController(
        IUserTableService userTableService,
        IConfiguration configuration,
        IUserService userService,
        IRoleService roleService,
        IMenuService menuService)
    {
        _configuration = configuration;
        _userService = userService;
        _userTableService = userTableService;
        _roleService = roleService;
        _menuService = menuService;
    }

    public async Task FetchData()
    {
        string email = HttpContext.Items["UserEmail"] as string ?? string.Empty;
        string role = HttpContext.Items["UserRole"] as string ?? string.Empty;
        UserBagViewModel? userBag = await _userService.UserDetailBag(email);
        List<RolePermissionModelView>? rolefilter = await _userService.RoleFilter(role);
        if (userBag != null)
        {
            ViewBag.Username = userBag.UserName;
            ViewBag.Userid = userBag.UserId;
            ViewBag.ImageUrl = userBag.ImageUrl;
            ViewBag.permission = rolefilter;
        }
    }

    public async Task<IActionResult> Index(int? categoryId = null, string? searchTerm = null, int? modifierId = null, string? searchModifier = null, int pageNumber = 1, int pageSize = 5)
    {
        await FetchData();
        MenuWithItemsViewModel menu = await _menuService.GetAllCategory(categoryId, searchTerm, pageNumber, pageSize);
        List<Modifiergroup>? result = await _menuService.GetAllModifier(modifierId, searchModifier);

        ViewBag.SelectedCategoryId = categoryId;
        menu.modifiergroups = result;
        ViewBag.SelectedModifierId = modifierId;

        return View(menu);
    }

    public async Task<IActionResult> FilterItems(int? categoryId = null, string? searchTerm = null, int pageNumber = 1, int pageSize = 5)
    {
        await FetchData();
        MenuWithItemsViewModel menu = await _menuService.GetAllCategory(categoryId, searchTerm, pageNumber, pageSize);

        // Get total items count before pagination
        int totalItems = menu.Items.Count;

        // Apply pagination
        menu.Items = menu.Items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        menu.TotalItems = totalItems; // Ensure TotalItems is set

        return PartialView("_ItemsPartial", menu);
    }



    [HttpPost]
    public async Task<IActionResult> AddCategory(MenuWithItemsViewModel model)
    {
        await _menuService.AddCategoryService(model);
        TempData["CategoryAdd"] = "Category added successfully";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddModifierGroup(MenuWithItemsViewModel model)
    {
        await _menuService.AddModifierGroupService(model);
        TempData["ModifierGroupAdd"] = "ModifierGroup added successfully";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(MenuWithItemsViewModel model)
    {
        await _menuService.EditCategoryService(model);
        TempData["CategoryAdd"] = "Category Edited successfully";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(MenuWithItemsViewModel model)
    {
        await _menuService.DeleteCategoryService(model);
        TempData["CategoryAdd"] = "Modifier Group Deleted successfully";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteModifierGroup(MenuWithItemsViewModel model)
    {
        await _menuService.DeleteModifierGroupService(model);
        TempData["ModifierGroupAdd"] = "Category Deleted successfully";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(MenuWithItemsViewModel viewModel)
    {
        if (viewModel.item == null)
        {
            // Handle null item case early
            MenuWithItemsViewModel menu2 = await _menuService.GetAllCategory(0, "");
            menu2.item = viewModel.item;
            ModelState.AddModelError("", "Item details are required.");
            return View("Index", menu2);
        }
        if (viewModel.item != null)
        {
            try
            {
                await FetchData();
                await _menuService.AddItemAsync(viewModel, viewModel.item?.UploadFiles, ViewBag.Userid);

                TempData["SuccessMessage"] = "Item added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("error :" + ex.Message);
                MenuWithItemsViewModel menu1 = await _menuService.GetAllCategory(0, "");
                menu1.item = viewModel.item;
                return RedirectToAction("Index", menu1);
            }
        }
        MenuWithItemsViewModel menu = await _menuService.GetAllCategory(0, "");
        menu.item = viewModel.item;
        return RedirectToAction("Index", menu);
    }


    [HttpPost]
    public async Task<IActionResult> DeleteItem(MenuWithItemsViewModel model)
    {
        await _menuService.DeleteItemService(model);
        TempData["CategoryAdd"] = "Item Deleted successfully";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditItem(int id)
    {
        var item = await _menuService.GetItemById(id); // New service method
        if (item == null)
        {
            return NotFound();
        }

        var viewModel = new MenuWithItemsViewModel
        {
            item = new ItemsViewModel
            {
                Itemid = item.Itemid,
                Categoryid = item.Categoryid,
                Itemname = item.Itemname,
                Itemtype = item.Itemtype,
                Rate = item.Rate,
                Quantity = item.Quantity,
                Units = item.Units,
                Isavailabe = (bool)item.Isavailabe,
                Defaulttax = (bool)item.DefaultTax,
                Taxpercentage = item.Taxpercentage,
                Shortcode = item.Shortcode,
                Description = item.Description
            },
            // Fetch categories for dropdown
        };

        return View("Index", viewModel); // Reuse Index view with pre-filled modal
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditItem(MenuWithItemsViewModel viewModel)
    {
        // Early check for null item
        if (viewModel.item == null)
        {
            MenuWithItemsViewModel menu = await _menuService.GetAllCategory(0, "");
            ModelState.AddModelError("", "Item details are required.");
            return View("Index", menu);
        }

        try
        {
            await FetchData();
            int userId = ViewBag.Userid;
            await _menuService.UpdateItemAsync(viewModel, viewModel.item.UploadFiles, userId);
            TempData["SuccessMessage"] = "Item updated successfully!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("error :" + ex.Message);
            MenuWithItemsViewModel menu1 = await _menuService.GetAllCategory(0, "");
            menu1.item = viewModel.item;
            return RedirectToAction("Index", menu1);
        }
    }

}