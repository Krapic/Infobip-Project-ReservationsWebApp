using Rezervacije.Web.Models;

namespace Rezervacije.Web.ResponseDtos;

public class BusinessListResponse
{
    public required Address Address { get; set; }
    public required string BusinessName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string DialingCode { get; set; }
    public List<BusinessActivitiesData> BusinessActivities { get; set; } = new List<BusinessActivitiesData>();
    public List<WorkHour> WorkHours { get; set; } = new List<WorkHour>();
    public List<ActiveAppointment> Appointments { get; set; } = new List<ActiveAppointment>();
    public required string MinimumHour {  get; set; }
    public required string MaximumHour { get; set; }
}

public class BusinessActivitiesData
{
    public required string NameOfActivity { get; set; }
    public required string DescriptionOfActivity { get; set; }
    public required decimal Price { get; set; }
}

public class WorkHour
{
    public int Day { get; set; }
    public string Start { get; set; } = "08:00";
    public string End { get; set; } = "22:00";
    public bool Highlight { get; set; } = true;
}

public class ActiveAppointment
{
    public required int Id { get; set; }
    public required string Subject { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required bool IsAllDay { get; set; } = false;
    public required string EventType { get; set; }
}
