namespace GOS.Models;

public class TournamentResult
{
    public Strategy? winner { get; set; }
    public List<GameProtocoll> GameProtocolls { get; set; }

}