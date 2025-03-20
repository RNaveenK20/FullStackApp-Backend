using System;
using System.Collections.Generic;

namespace Backend.DBModel;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int? UserId { get; set; }

    public string? RecipeName { get; set; }

    public string? Ingredients { get; set; }

    public string? Instructions { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? Category { get; set; }

    public virtual TblUser? User { get; set; }
}
