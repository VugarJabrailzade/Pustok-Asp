using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels.Product
{
    public class CategoryUpdateResponseViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the name!")]
        public string Name { get; set; }
    }
}
