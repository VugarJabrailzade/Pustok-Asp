﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Extensions;
using Pustok.Helpers.Paging;
using Pustok.Services.Abstract;
using Pustok.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Client;

[Route("product")]
public class ProductController : Controller
{
    private readonly PustokDbContext _pustokDbContext;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductController(PustokDbContext pustokDbContext, IFileService fileService, IHttpContextAccessor httpContextAccessor)
    {
        _pustokDbContext = pustokDbContext;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
    }


    [HttpGet("index")]
    public async Task<IActionResult> Index(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] int? colorId,
        [FromQuery(Name ="price-range-filter")] string priceRangeFilter,
        [FromQuery] string sort,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
        )
    
    {

        (decimal? priceMinRangeFilter, decimal? priceMaxRangeFilter) = GetRanges(priceRangeFilter);

        var productPageViewModel = new ProductPageViewModel();
        productPageViewModel.CategoryId = categoryId;
        productPageViewModel.ColorId = colorId;
        productPageViewModel.PriceMinRange = GetPriceMinRange();
        productPageViewModel.PriceMaxRange = GetPriceMaxRange();
        productPageViewModel.Categories = await GetCategoryAsync();
        productPageViewModel.Colors = await GetColorAsync();
        (var products, var paginator) = await GetProductsAsync();

        productPageViewModel.PriceMaxRangeFilter = priceMaxRangeFilter;
        productPageViewModel.PriceMinRangeFilter = priceMinRangeFilter;
        productPageViewModel.Products = products;
        productPageViewModel.Paginator = paginator;
        

        productPageViewModel.Page = page ?? 1;
        productPageViewModel.PageSize = pageSize;
        productPageViewModel.Search = search;
        productPageViewModel.Sort = sort;

        productPageViewModel.PriceMinRange = _pustokDbContext.Products.OrderBy(p => p.Price).FirstOrDefault()?.Price;
        productPageViewModel.PriceMaxRange = _pustokDbContext.Products.OrderByDescending(p => p.Price).FirstOrDefault()?.Price;

  
        async Task<(List<ProductViewModel> products, Paginator<Product> paginator)> GetProductsAsync()
        {
            var productQuery = _pustokDbContext.Products.
                    WhereNotNull(search, p => EF.Functions.ILike(p.Name, $"%{search}%")).
                    WhereNotNull(categoryId, p => p.CategoryId == categoryId).
                    WhereNotNull(colorId, p => p.ProductColors.Any(x => x.ColorId == colorId)).
                    WhereNotNull(priceMinRangeFilter, p => p.Price >= priceMinRangeFilter.Value).
                    WhereNotNull(priceMaxRangeFilter, p => p.Price <= priceMaxRangeFilter.Value);

            productQuery = ImplementProductSorting(productQuery, sort);

            paginator = new Paginator<Product>(productQuery, page, pageSize ?? 9);

           var products = await paginator.QuerySet.
                    Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Rating = p.Rating,
                        Imageurl = UploadDirectory.Products.GetUrl(p.ImageNameInFileSystem),

                    }).ToListAsync();

            return (products, paginator);


        }
        async Task<List<CategoryViewModel>> GetCategoryAsync()
        {
            return await _pustokDbContext.Categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProductsCount = c.Products.Count,
            }).ToListAsync();
        }
        async Task<List<ColorViewModel>> GetColorAsync()
        {
            return await _pustokDbContext.Colors.Select(c => new ColorViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProductsCount = c.ProductColors.Count
            }).ToListAsync();
        }
        decimal? GetPriceMinRange()
        {
            return _pustokDbContext.Products.OrderBy(p => p.Price).FirstOrDefault()?.Price;
        }
        decimal? GetPriceMaxRange()
        {
            return _pustokDbContext.Products.OrderByDescending(p => p.Price).FirstOrDefault()?.Price;
        };
     

        (decimal? priceMinRangeFilter, decimal? priceMaxRangeFilter) GetRanges(string priceRangeFilter)
        {
            if (priceRangeFilter == null)
            {
                return (null, null);
            }

            var ranges = priceRangeFilter.Split(";");

            decimal? priceMinRangeFilter = priceRangeFilter != null ? decimal.Parse(ranges[0]) : null;
            decimal? priceMaxRangeFilter = priceRangeFilter != null ? decimal.Parse(ranges[1]) : null; 

            return(priceMinRangeFilter, priceMaxRangeFilter);

        }

        IOrderedQueryable<Product> ImplementProductSorting(IQueryable<Product> productQuery, string sortQuery)
        {
            if (sortQuery == null)
            {
                return productQuery.OrderByDescending(p => p.Id);
            }

            switch (sortQuery)
            {
                case "price_desc":
                    return productQuery.OrderByDescending(p => p.Price);
                case "price_asc":
                    return productQuery.OrderBy(p => p.Price);
                case "rate_desc":
                    return productQuery.OrderByDescending(p => p.Rating);
                case "rate_asc":
                    return productQuery.OrderBy(p => p.Rating);
                default:
                    throw new Exception("Sort query doesn't found");
            }
        }

        IQueryable<Product> ImplementPaging(IQueryable<Product> productQuery, int? page = 1)
        {
            const int pageSize = 6;

            int skipCount = ((page ?? 1)-1) * pageSize;

           return productQuery.Skip(skipCount).Take(pageSize);

        }


        


        return View(productPageViewModel);
    }
}
