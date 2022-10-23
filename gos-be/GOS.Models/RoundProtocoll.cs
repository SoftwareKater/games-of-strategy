namespace GOS.Models;

public class RoundProtocoll
{
    public int StateA { get; set; }
    public int StateB { get; set; }
    public MOVE MoveA { get; set; }
    public MOVE MoveB { get; set; }
    public int PayoffA { get; set; }
    public int PayoffB { get; set; }
}