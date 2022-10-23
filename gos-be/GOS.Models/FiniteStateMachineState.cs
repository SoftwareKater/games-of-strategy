namespace GOS.Models;

public class FiniteStateMachineState
{
    public MOVE move { get; set; }
    /*
        Defines the next state based on the move of the adversary.
        If the adversary plays C then stateTransition[0] will be the next state.
    */
    public List<int> stateTransition { get; set; }
}