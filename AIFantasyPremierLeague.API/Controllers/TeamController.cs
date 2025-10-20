using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace AIFantasyPremierLeague.API.Controllers;

[ApiController]
[Route("team")]
public class TeamController : ControllerBase
{

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public ActionResult<IEnumerable<Team>> GetElements()
    {

        List<Team> teams = new() { new Team("Mike"), new Team("Sam") };

        return Ok(teams);

    }
}

