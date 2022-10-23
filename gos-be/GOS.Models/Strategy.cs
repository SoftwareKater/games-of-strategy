namespace GOS.Models;

public class Strategy
{
    public Guid id { get; set; } = new Guid();
    public String author { get; set; }
    public String name { get; set; }
    public List<FiniteStateMachineState> states { get; set; }
}