namespace GOS.Models;

public class FiniteStateMachineState
{
    public MOVE Move { get; set; }
    /*
        Defines the next state based on the move of the adversary.
        If the adversary plays C then stateTransition[0] will be the next state.
    */
    public List<int> StateTransition { get; set; } = new List<int>{0, 0};
}