using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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


        #region Add

        [HttpGet("add", Name = "category-add")]

        public async Task<IActionResult> Add()
        {
            return View("Views/Admin/Category/Add.cshtml");
        }



        [HttpPost("add", Name = "category-add-post")]

        public async Task<IActionResult> Add(CategoryAddResponseViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Can't be null" });

            if (model == null)
            {
                ModelState.AddModelError("Name", "Can't be null");
                return BadRequest(new { message = "Can't be null" });
            }


            var existCate = _pustokDbContext.Categories.FirstOrDefault(c => c.Name == model.Name);
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

        #endregion

        #region Update

        [HttpGet("update", Name ="category-update")]
        public  async Task<IActionResult> Update(int id)
        {
            
                var category = await _pustokDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null) return BadRequest(ModelState);

                var cateModel = new CategoryAddRequestViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                };

                return View("Views/Admin/Category/Update.cshtml", cateModel);
            

        }

        #endregion

        [HttpPut("update-post", Name ="category-update-post")]
        public async Task<IActionResult> Update(CategoryAddRequestViewModel model)
        {
            var category = await _pustokDbContext.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);

            if(category == null) return BadRequest("Not Working!");

            var newUptCate = new Category { Name = model.Name };

            _pustokDbContext.Categories.Add(newUptCate);
            await _pustokDbContext.SaveChangesAsync();


            return RedirectToRoute("category-list");
        }




        #region Delete
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
        #endregion

    }
}