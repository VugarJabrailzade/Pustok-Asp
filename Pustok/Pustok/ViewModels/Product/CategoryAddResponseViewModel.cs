using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels.Product
{
    public class CategoryAddResponseViewModel 
    {
        [Required(ErrorMessage ="Please enter the name!")]
        public string Name { get; set; }
    }
}
