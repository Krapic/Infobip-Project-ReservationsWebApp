using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rezervacije.Models;

public class WorkingDayStructure
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public Business Business { get; set; }
    public bool IsWorkingDay { get; set; }
    public int StartingHours { get; set; }
    public int EndingHours { get; set; }
    public int Day { get; set; }
}
