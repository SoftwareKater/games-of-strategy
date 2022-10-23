namespace GOS.Models;

public class RoundProtocoll
{
    public int StateA { get; set; }
    public int StateB { get; set; }
    public MOVE moveA { get; set; }
    public MOVE moveB { get; set; }
    public int payoffA { get; set; }
    public int payoffB { get; set; }
}