using System;
using System.ComponentModel.DataAnnotations;
using PizzaShop.Repository.Models;

namespace PizzaShop.Repository.ModelView;

public class MenuWithItemsViewModel
{


    //helper properties
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(40, ErrorMessage = "Name limit exceed ")]
    public string? CategoryName { get; set; }
    [Required(ErrorMessage = "Category Description is required")]
    [MaxLength(250, ErrorMessage = "description limit exceed ")]
    public string? CategoryDescription { get; set; }
    [Required(ErrorMessage = "Modifier group name is required")]
    [MaxLength(40, ErrorMessage = "Name limit exceed ")]
    public string? Modifiergroupname { get; set; }
    [Required(ErrorMessage = "Modifier Description is required")]
    [MaxLength(250, ErrorMessage = "description limit exceed ")]
    public string? Modifiergroupdescription { get; set; }
    [Required]
    public int Userid { get; set; }
    [Required]
    public int Categoryid { get; set; }
    [Required]
    public int Modifiergroupid {get; set;}
    public int itemid { get; set; }
    public int modiferid {get; set;}


    //helper view models
    public ItemsViewModel? item { get; set; }
    public ModifiersViewModel? modifiersViewModel {get; set;}


    // lists 
    public List<Category>? Categories { get; set; }
    public List<Item>? Items { get; set; }
    public List<Modifier>? Modifiers { get; set; }
    public List<Modifiergroup>? modifiergroups {get; set;}
    public List<SelectedModifiersViewModel>? selectedModifiersViewModels {get;set;}


    // pagging info
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage1 { get; set; }
    public int PageSize1 { get; set; }
    public int TotalItems1 { get; set; }
}

