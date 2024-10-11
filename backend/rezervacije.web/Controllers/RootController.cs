using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Rezervacije.Controllers;

[Route("/")]
[ApiController]
public class RootController : Controller
{
    [HttpGet]
    public IActionResult GetRootResponse()
    {
        return Ok(new { response = "Backend is running!"});
    }
}
