using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rezervacije.Data;
using Rezervacije.Web.Dtos;
using Rezervacije.Web.Models;
using Rezervacije.Web.Services;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Rezervacije.Web.Controllers;

[Route("/api/reservation")]
[ApiController]
public class ReservationController : Controller
{
    private RezervacijeDbContext _context;
    private IUserService _user;
    private IBusinessService _business;

    public ReservationController
    (
        RezervacijeDbContext context,
        IUserService user,
        IBusinessService business
    )
    {
        _context = context;
        _user = user;
        _business = business;
    }

    [HttpPost("reserve"), Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> ReserveThisTerm([FromBody] ReserveTermDto request)
    {
        
        var business = await _business.GetBusinessByBusinessNameIncludingUser(request.BusinessName);

        if (business is null)
        {
            return NotFound(new { response = "Nije moguće pronaći obrt." });
        }

        var user = await _user.FindUserByEmail(request.UserEmail);

        if (user is null)
        {
            return NotFound(new { response = "Nije moguće pronaći korisnika za rezerviranje ovog termina." });
        }

        
        var newAppointment = new Appointment
        {
            BusinessId = business.Id,
            ReservedUserId = user.Id,
            EndTime = request.EndTime,
            EventType = request.EventType,
            StartTime = request.StartTime,
            Subject = request.Subject
        };

        await _context.Appointments.AddAsync(newAppointment);
        await _context.SaveChangesAsync();
        
        
        return Ok(new { response = "Termin je rezerviran!" });
    }

    [HttpDelete("cancel-reservation"), Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> CancelThisReservation([FromBody] DeleteReservationDto request)
    {
        var business = await _business.GetBusinessByBusinessNameIncludingAppointments(request.BusinessName);

        if (business is null)
        {
            return NotFound(new { reponse = "Nije moguće pronaći obrt." });
        }

        var user = await _user.FindUserByEmail(request.UserEmail);

        if (user is null)
        {
            return NotFound(new { reponse = "Nije moguće pronaći korisnika za otkazivanje ovog termina." });
        }

        var appointment = _business.FindBusinessAppointmentById(business, request.Id);

        if (appointment is null)
        {
            return NotFound(new { reponse = "Nije moguće pronaći ovaj termin." });
        }

        if (appointment.ReservedUserId != user.Id)
        {
            return BadRequest(new { reponse = "Ne možete otkazati termin koji nije vaš ili koji nije rezerviran." });
        }

        _context.Remove(appointment);

        await _context.SaveChangesAsync();

        return Ok(new { reponse = "Ovaj termin je uspješno otkazan." });
    }

    [HttpPatch("update-reservation"), Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> UpdateThisReservation([FromBody] ReserveTermDto request)
    {
        var business = await _business.GetBusinessByBusinessNameIncludingAppointments(request.BusinessName);

        if (business is null)
        {
            return NotFound(new { reponse = "Nije moguće pronaći obrt." });
        }

        var user = await _user.FindUserByEmail(request.UserEmail);

        if (user is null)
        {
            return NotFound(new { reponse = "Nije moguće pronaći korisnika za otkazivanje ovog termina." });
        }

        var appointment = _business.FindBusinessAppointmentByStartTime(business, request.StartTime);

        if (appointment is null)
        {
            return NotFound(new { reponse = "Nije moguće pronaći ovaj termin." });
        }

        if (appointment.ReservedUserId != user.Id)
        {
            return BadRequest(new { reponse = "Ne možete otkazati termin koji nije vaš ili koji nije rezerviran." });
        }

        appointment.EventType = request.EventType;

        _context.Appointments.Update(appointment);

        await _context.SaveChangesAsync();

        return Ok(new { reponse = "Ovaj termin je uspješno uređen." });
    }

    [HttpGet("appointments-for-business"), Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetAllReservationsForThisBusiness([FromQuery] UserEmailDto request)
    {
        var user = await _user.FindUserByEmailIncludeEverything(request.UserEmail);

        if (user is null || user.Business is null)
        {
            return BadRequest(new { response = "Nema obrta za ovog korisnika!" });
        }

        var appointments = await _business.GetAllAppointmentsForBusinessById(user.BusinessId ?? -1);

        if (appointments is null)
        {
            return BadRequest(new { response = "Nema korisnika ili obrta za ovog korisnika!" });
        }

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        var jsonResult = JsonSerializer.Serialize(new { appointments }, jsonOptions);

        return Ok(jsonResult);
    }

    [HttpGet("appointments-for-user"), Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetAllReservationsForThisUser([FromQuery] UserEmailDto request)
    {
        var appointments = await _user.GetAllAppointmentsForThisUser(request.UserEmail);

        if (appointments is null || !appointments.Any())
        {
            return BadRequest(new { response = "Trenutno nema nikakvih rezerviranih termina za vas." });
        }

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        var jsonResult = JsonSerializer.Serialize(new { appointments }, jsonOptions);

        return Ok(jsonResult);
    }
}
