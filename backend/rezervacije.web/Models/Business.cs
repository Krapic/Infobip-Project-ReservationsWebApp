using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Rezervacije.Dtos;
using Rezervacije.Web.Models;

namespace Rezervacije.Models;

public class Business
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool IsWaitingValidation { get; set; } = true;
    public required string BusinessName { get; set; }
    public required int BusinessIdentificationNumber { get; set; }
    public required BusinessTypes BusinessType { get; set; }
    public List<BusinessActivity> BusinessActivities { get; set; } = new List<BusinessActivity>();
    public List<WorkingDayStructure> WorkingDayStructures { get; set; } = new List<WorkingDayStructure>(7);
    public List<Appointment> BusinessAppointments { get; set; } = new List<Appointment>();
    

    public Address Address { get; set; }
    public int AddressId { get; set; }

    public User User {  get; set; }
    public int UserId { get; set; }
}
