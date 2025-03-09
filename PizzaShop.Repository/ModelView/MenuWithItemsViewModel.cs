using System;
using System.ComponentModel.DataAnnotations;
using PizzaShop.Repository.Models;

namespace PizzaShop.Repository.ModelView;

public class MenuWithItemsViewModel
{
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(40, ErrorMessage = "Name limit exceed ")]
    public string? CategoryName { get; set; }
    [Required(ErrorMessage = "Category Description is required")]
    [MaxLength(250, ErrorMessage = "description limit exceed ")]
    public string? CategoryDescription { get; set; }
    [Required]
    public int Userid { get; set; }
    [Required]
    public int Categoryid { get; set; }
    public int itemid { get; set; }
    public ItemsViewModel? item { get; set; }
    public List<Category>? Categories { get; set; }
    public List<Item>? Items { get; set; }


    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}

