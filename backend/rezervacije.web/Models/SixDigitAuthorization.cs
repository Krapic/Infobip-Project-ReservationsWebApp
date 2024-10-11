using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rezervacije.Models;

public class SixDigitAuthorization
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string SixDigitNumber { get; set; } = string.Empty;
    public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
    public DateTime ValidUntil { get; set; } = DateTime.UtcNow.AddMinutes(30);
    public bool IsUsed { get; set; } = false;
    public required int UserId { get; set; }
}
