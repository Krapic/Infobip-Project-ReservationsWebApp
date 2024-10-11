using Rezervacije.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rezervacije.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool IsAdmin { get; set; } = false;
    public required string DialingCode { get; set; }
    public required string PhoneNumber { get; set; }
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    public Address Address { get; set; }
    public int AddressId { get; set; }

    public int? BusinessId { get; set; }
    public Business? Business { get; set; }
}
