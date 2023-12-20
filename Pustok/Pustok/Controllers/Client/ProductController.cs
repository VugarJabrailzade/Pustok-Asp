using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Extensions;
using Pustok.Services.Abstract;
using Pustok.ViewModels.Product;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Client;

[Route("product")]
public class ProductController : Controller
{
    private readonly PustokDbContext _pustokDbContext;
    private readonly IFileService _fileService;

    public ProductController(PustokDbContext pustokDbContext, IFileService fileService)
    {
        _pustokDbContext = pustokDbContext;
        _fileService = fileService;
    }


    [HttpGet("index")]
    public async Task<IActionResult> Index(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] int? colorId,
        [FromQuery] decimal? priceMinRangeFilter,
        [FromQuery] decimal? priceMaxRangeFilter)
    {

        var productPageViewModel = new ProductPageViewModel();
        productPageViewModel.Search = search;
        productPageViewModel.CategoryId = categoryId;
        productPageViewModel.ColorId = colorId;

         productPageViewModel.Products = await _pustokDbContext.Products.
                    Where(p=> (search != null ? EF.Functions.ILike(p.Name, $"%{search}%") : true) &&
                    (categoryId != null ? p.CategoryId == categoryId : true) && 
                    (colorId != null ? p.ProductColors.Any(x=> x.ColorId == colorId) : true)).
                    Select(p => new ProductViewModel
                    {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Rating = p.Rating,
                    Imageurl = UploadDirectory.Products.GetUrl(p.ImageNameInFileSystem),

                    }).ToListAsync();

         productPageViewModel.Categories = await _pustokDbContext.Categories.Select(c => new CategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ProductsCount = c.Products.Count,
                    }).ToListAsync();

         productPageViewModel.Colors = await _pustokDbContext.Colors.Select(c => new ColorViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ProductsCount = c.ProductColors.Count
                    }).ToListAsync();

        productPageViewModel.PriceMinRange = _pustokDbContext.Products.OrderBy(p => p.Price).FirstOrDefault()?.Price;
        productPageViewModel.PriceMaxRange = _pustokDbContext.Products.OrderByDescending(p => p.Price).FirstOrDefault()?.Price;

  

        return View(productPageViewModel);
    }
}
