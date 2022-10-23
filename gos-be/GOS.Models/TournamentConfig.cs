namespace GOS.Models;

public class TournamentConfig
{
    /*
        Payoff matrix contains game payoffs for the row player
                    C  D                    C  D
                C | R  S |    e.g.      C | 3  1 |
                D | T  P |              D | 4  2 |
    */
    public List<List<int>> payoffMatrix { get; set; } = new List<List<int>>{new List<int>{3, 1}, new List<int>{4, 2}};
    public int rounds { get; set; } = 1000;
    public List<Strategy> strategies { get; set; } = new List<Strategy>();
}