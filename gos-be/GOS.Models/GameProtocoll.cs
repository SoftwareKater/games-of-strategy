namespace GOS.Models;

public class GameProtocoll
{
    public String? NameOfAgentA { get; set; }
    public String? NameOfAgentB { get; set; }
    public List<RoundProtocoll> RoundProtocolls { get; set; } = new List<RoundProtocoll>{};
    public int GamePayoffOfAgentA { get; set; }
    public int GamePayoffOfAgentB { get; set; }
}