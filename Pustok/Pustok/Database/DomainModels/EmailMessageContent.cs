using Pustok.Database.Abstracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pustok.Database.DomainModels;

public class EmailMessageContent : IEntity
{
    public int Id { get; set; }
    [Required]

    public List<string> To { get; set; }
    public string Email { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public string Title { get; set; }
}
