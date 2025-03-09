using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Repository.Models;

public partial class Item
{
    public int Itemid { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public int Categoryid { get; set; }
   
    [Required(ErrorMessage = "Item name is required")]
    public string? Itemname { get; set; }

    [Required(ErrorMessage = "Item type is required")]
    public int? Itemtype { get; set; }

    [Required(ErrorMessage = "Rate is required")]
    public decimal? Rate { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public decimal? Quantity { get; set; }

    [Required(ErrorMessage = "Unit is required")]
    public int? Status { get; set; }

    [Required(ErrorMessage = "shortcode is required")]
    public int? Shortcode { get; set; }

    public string? Description { get; set; }

    public string? Imageid { get; set; }

    [Required(ErrorMessage = "tax % is required")]
    public decimal? Taxpercentage { get; set; }

    public bool? Favourite { get; set; }

    [Required(ErrorMessage = "units is required")]
    public int? Units { get; set; }

    public DateTime? Editedat { get; set; }

    public DateTime? Createdat { get; set; }

    public int? Createdbyid { get; set; }

    public int? Editedbyid { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Deletedat { get; set; }

    public int? Deletedbyid { get; set; }

    public bool? Isavailabe { get; set; }

    public bool Defaulttax { get; set; }

    public virtual Category Category { get; set; } = null!;
}
