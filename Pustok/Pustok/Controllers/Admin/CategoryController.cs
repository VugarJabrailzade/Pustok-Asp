using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
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
        public readonly IValidator<CategoryAddResponseViewModel> _validator;



        public CategoryController(PustokDbContext pustokDbContext, 
                                  ILogger<CategoryController> logger,
                                  IValidator<CategoryAddResponseViewModel> validator)
        {
            _logger = logger;
            _validator = validator;
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
            ValidationResult result = await _validator.ValidateAsync(model);


            if (!result.IsValid)
            {
                // If validation fails, add errors to ModelState and return BadRequest
                foreach (var failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) return BadRequest();

            if(model == null)
            {
                ModelState.AddModelError("Name", "Can't be null");
                return BadRequest(ModelState);
            }


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


        [HttpDelete("delete", Name = "category-delete")]
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

                var response = new
                {
                    deleteid = id,
                    deleteName = category.Name,
                };

                return Json(response);
            }
            catch (Exception)
            {
                return StatusCode(202);
            }

            
        }
    }
}
