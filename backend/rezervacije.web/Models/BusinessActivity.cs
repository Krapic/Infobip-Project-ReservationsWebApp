using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rezervacije.Models;

public class BusinessActivity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public Business Business { get; set; }
    public string NameOfActivity { get; set; } = string.Empty;
    public string DescriptionOfActivity { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
