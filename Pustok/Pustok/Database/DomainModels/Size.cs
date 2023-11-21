using Pustok.Database.Abstracts;
using System.Collections.Generic;

namespace Pustok.Database.DomainModels
{
    public class Size : IEntity
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
    }
}
