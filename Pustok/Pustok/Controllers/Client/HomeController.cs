using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.ViewModels.Product;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Client;

[Route("home")]
public class HomeController : Controller
{
    private readonly PustokDbContext _dbContext;

    public HomeController(PustokDbContext pustokDbContext)
    {
        _dbContext = pustokDbContext;
    }

    // localhost:2323/home/index
    //action
    //url mapping, route mapping

    [HttpGet("index")]
    public ViewResult Index()
    {
        return View(_dbContext.Products.ToList());
    }

    [HttpGet("modal",Name ="home-modal")]
    public async Task<IActionResult> Modal(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product == null) { return NotFound(); }

        var  modal =  new ModalDetailsViewModel
        {
            Name = product.Name,
            Price = product.Price,
            Colors = _dbContext.ProductColors.Where(x => product.Id == id).Select(c => c.Color).ToList(),
            Sizes = _dbContext.ProductSizes.Where(x => product.Id == id).Select(c => c.Size).ToList(),
        };

        return  PartialView("Views/Shared/Partials/Client/_ProductDetalsPartialView.cshtml", modal);
    }






    // localhost:2323/home/contact
    //action
    public ViewResult Contact()
    {
        return View();
    }

    // localhost:2323/home/about
    //action
    public ViewResult About()
    {
        return View();
    }
}
