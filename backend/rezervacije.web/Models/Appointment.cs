using Rezervacije.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rezervacije.Web.Models;

public class Appointment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Subject { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? EventType { get; set; }
    public int? ReservedUserId { get; set; }
    public User? ReservedUser { get; set; }
    public int? BusinessId { get; set; }
    public Business? Business { get; set; }
}
