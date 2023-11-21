using Pustok.Database.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Database.DomainModels;

[Table("categories")]
public class Category : IEntity
{
    

    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }
}
