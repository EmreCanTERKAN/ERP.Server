﻿using ERP.Server.Domain.Products;
using ERP.Server.Domain.RecipeDetails;
using ERPServer.Domain.Abstractions;

namespace ERP.Server.Domain.Recipes;

public sealed class Recipe : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public List<RecipeDetail>? Details { get; set; }
}
