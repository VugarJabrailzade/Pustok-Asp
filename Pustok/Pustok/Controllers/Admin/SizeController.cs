using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.ViewModels.Product;
using System;
using System.Linq;

namespace Pustok.Controllers.Admin
{
    [Route("admin/size")]
    public class SizeController : Controller
    {
        public readonly PustokDbContext _pustokDbContext;
        public readonly  ILogger<SizeController> _logger;

        public SizeController(PustokDbContext pustokDbContext, ILogger<SizeController> logger)
        {
            _pustokDbContext = pustokDbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Size()
        {
            var size = _pustokDbContext.Sizes.ToList();

            return View("Views/Admin/Size/Size.cshtml",size);
        }

        [HttpGet("add", Name = "add-size")]
        public IActionResult Add()
        {
            var sizes = _pustokDbContext.Sizes.ToList();

            return View("Views/Admin/Size/Add.cshtml", sizes);
        }

        [HttpPost("add", Name ="add-size")]
        public IActionResult Add(ProductSizeAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }


            try
            {
                foreach(var size in model.SelectedSizesIds)
                {
                    var sizes = _pustokDbContext.Sizes.FirstOrDefault(x => x.Id == size);

                    if(sizes == null)
                    {
                        return NotFound();
                    }

                    var productSizes = new Size
                    {
                        Name = model.Name
                    };

                    _pustokDbContext.Sizes.Add(productSizes);

                }
                
            }
            catch(Exception )
            {
                _logger.LogError("e");   
            }


            _pustokDbContext.SaveChanges();

            return View("Views/Admin/Size/Size.cshtml");

        }

        
    }
}
