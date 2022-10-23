using GOS.Models;
using GOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace GOS.Controllers;

[ApiController]
[Route("[controller]")]
public class TournamentController : ControllerBase
{
    private readonly ILogger<TournamentController> _logger;
    private readonly TournamentService _service;

    public TournamentController(ILogger<TournamentController> logger, TournamentService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public TournamentResult Post(TournamentConfig tournamentConfig)
    {
        return _service.RunTournament();
    }
}
