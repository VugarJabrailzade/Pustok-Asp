using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.ViewModels.Product;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Admin
{
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        public readonly PustokDbContext _pustokDbContext;
        public readonly ILogger<CategoryController> _logger;
        public IValidator<Category> _validator;


        public CategoryController(PustokDbContext pustokDbContext, ILogger<CategoryController> logger)
        {
            _logger = logger;
            _pustokDbContext = pustokDbContext;

        }

        [HttpGet("index", Name = "category-list")]
        public IActionResult Index()
        {
            var category = _pustokDbContext.Categories.ToList();

            return View("Views/Admin/Category/Index.cshtml", category);
        }




        [HttpGet("add", Name = "category-add")]

        public async Task<IActionResult> Add()
        {
            return View("Views/Admin/Category/Add.cshtml");
        }



        [HttpPost("add", Name ="category-add-post")]

        public async Task<IActionResult> Add(CategoryAddResponseViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();


            var existCate = _pustokDbContext.Categories.FirstOrDefault(c=> c.Name == model.Name);
            if (existCate != null) 
            {
                return BadRequest();
            }
              
            var categoryAdd = new Category
            {
                Name = model.Name,
            };

            _pustokDbContext.Categories.Add(categoryAdd);
            _pustokDbContext.SaveChanges();

            return RedirectToRoute("category-list");

        }


        [HttpDelete("delete/{id}", Name = "category-delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                Category category = await _pustokDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return NotFound();
                }

                _pustokDbContext.Remove(category);

                await _pustokDbContext.SaveChangesAsync();

                return RedirectToAction("category-list");
            }
            catch (Exception)
            {
                return StatusCode(202);
            }
        }
    }
}
