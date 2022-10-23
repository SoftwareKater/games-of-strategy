namespace GOS.Models;

public class Strategy
{
    public Guid Id { get; set; } = new Guid();
    public String? Author { get; set; }
    public String Name { get; set; } = new Guid().ToString();
    public List<FiniteStateMachineState> States { get; set; }
}