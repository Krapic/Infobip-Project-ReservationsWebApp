namespace Rezervacije.Web.Dtos;

public class ReserveTermDto
{
    public required string BusinessName { get; set; }
    public required string UserEmail { get; set; }
    public required string Subject { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string EventType { get; set; }
}

public class DeleteReservationDto
{
    public required string BusinessName { get; set; }
    public required string UserEmail { get; set; }
    public required int Id { get; set; }
}

public class BusinessIdDto
{
    public required int BusinessId { get; set; }
}

public class UserEmailDto
{
    public required string UserEmail { get; set; }
}
