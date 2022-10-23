namespace GOS.Models;

public class TournamentResult
{
    public List<string>? Winners { get; set; }
    public Dictionary<string, int>? TournamentPayoffs { get; set; }
    public List<GameProtocoll>? GameProtocolls { get; set; }

}