using GOS.Models;
using System.ComponentModel;

namespace GOS.Services;

public class TournamentService
{
    public TournamentResult RunTournament(/*TournamentConfig tournamentConfig*/)
    {
        TournamentConfig tournamentConfig = new TournamentConfig
        {
            Strategies = GetMockStrategies()
        };

        if (!VerifyConfig(tournamentConfig))
        {
            throw new Exception("Invalid tournament config");
        }

        int numberOfStrategies = tournamentConfig.Strategies.Count;

        // Initialize game protocolls list
        List<GameProtocoll> gameProtocolls = new List<GameProtocoll> { };

        // Initialize tournament payoff dictionary
        Dictionary<string, int> tournamentPayoffs = new Dictionary<string, int>() { };
        for (var i = 0; i < numberOfStrategies; i++)
        {
            // this is a bad idea: use Id instead, because Name must not be unique
            tournamentPayoffs.Add(tournamentConfig.Strategies[i].Name, 0);
        }

        // Run the tournament
        for (var i = 0; i < numberOfStrategies; i++)
        {
            for (var j = i; j < numberOfStrategies; j++)
            {
                Strategy agentA = tournamentConfig.Strategies[i];
                Strategy agentB = tournamentConfig.Strategies[j];

                GameProtocoll gameProtocoll = RunGame(tournamentConfig, agentA, agentB);
                gameProtocolls.Add(gameProtocoll);

                tournamentPayoffs[tournamentConfig.Strategies[i].Name] += gameProtocoll.GamePayoffOfAgentA;
                tournamentPayoffs[tournamentConfig.Strategies[j].Name] += gameProtocoll.GamePayoffOfAgentB;
            }
        }

        // Find the winners of the tournament
        int maxPayoff = tournamentPayoffs.Values.Max();
        List<int> winnerIndices = tournamentPayoffs.Values
            .Select((strategy, index) => strategy == maxPayoff ? index : -1)
            .Where(i => i != -1).ToList();
        List<string> winners = tournamentConfig.Strategies
            .Where((strategy, index) => winnerIndices.Contains(index))
            .Select(strategy => strategy.Name).ToList();

        return new TournamentResult
        {
            Winners = winners,
            TournamentPayoffs = tournamentPayoffs,
            GameProtocolls = gameProtocolls,
        };
    }

    private bool VerifyConfig(TournamentConfig tournamentConfig)
    {
        if (tournamentConfig.Strategies.Count() < 2)
        {
            return false;
        }
        if (tournamentConfig.Rounds < 1)
        {
            return false;
        }
        return true;
    }

    private GameProtocoll RunGame(TournamentConfig tournamentConfig, Strategy agentA, Strategy agentB)
    {

        List<RoundProtocoll> RoundProtocolls = new List<RoundProtocoll> { };

        int StateOfAgentA = 0;
        int StateOfAgentB = 0;

        for (int i = 0; i < tournamentConfig.Rounds; i++)
        {
            RoundProtocoll RoundProtocoll = new RoundProtocoll { };

            RoundProtocoll.StateA = StateOfAgentA;
            RoundProtocoll.StateB = StateOfAgentB;

            MOVE agentAMove = agentA.States[StateOfAgentA].Move;
            MOVE agentBMove = agentB.States[StateOfAgentB].Move;

            RoundProtocoll.MoveA = agentAMove;
            RoundProtocoll.MoveB = agentBMove;

            int PayoffA = tournamentConfig.PayoffMatrix[((int)agentAMove)][((int)agentBMove)];
            int PayoffB = tournamentConfig.PayoffMatrix[((int)agentBMove)][((int)agentAMove)];

            RoundProtocoll.PayoffA = PayoffA;
            RoundProtocoll.PayoffB = PayoffB;

            StateOfAgentA = agentA.States[StateOfAgentA].StateTransition[((int)agentBMove)];
            StateOfAgentB = agentB.States[StateOfAgentB].StateTransition[((int)agentAMove)];

            RoundProtocolls.Add(RoundProtocoll);
        }

        int GamePayoffA = RoundProtocolls.Select(RoundProtocoll => RoundProtocoll.PayoffA).Aggregate(0, (acc, x) => acc + x);
        int GamePayoffB = RoundProtocolls.Select(RoundProtocoll => RoundProtocoll.PayoffB).Aggregate(0, (acc, x) => acc + x);

        GameProtocoll gameProtocoll = new GameProtocoll
        {
            NameOfAgentA = agentA.Name,
            NameOfAgentB = agentB.Name,
            GamePayoffOfAgentA = GamePayoffA,
            GamePayoffOfAgentB = GamePayoffB,
            RoundProtocolls = RoundProtocolls,
        };

        ConsoleLogProtocoll(gameProtocoll);

        return gameProtocoll;
    }

    private void ConsoleLogProtocoll(GameProtocoll gameProtocoll)
    {
        Console.WriteLine("Game Protocoll");
        Console.WriteLine("==============");

        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(gameProtocoll))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(gameProtocoll);
            Console.WriteLine("{0}={1}", name, value);
        }

        Console.WriteLine("Round    1");
        Console.WriteLine("----------");

        RoundProtocoll roundProtocoll1 = gameProtocoll.RoundProtocolls[0];
        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(roundProtocoll1))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(roundProtocoll1);
            Console.WriteLine("{0}={1}", name, value);
        }

        Console.WriteLine("...");
        Console.WriteLine("Round 1000");
        Console.WriteLine("----------");

        RoundProtocoll roundProtocoll1000 = gameProtocoll.RoundProtocolls[999];
        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(roundProtocoll1000))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(roundProtocoll1000);
            Console.WriteLine("{0}={1}", name, value);
        }

        Console.WriteLine("==============");
    }

    private List<Strategy> GetMockStrategies()
    {
        return new List<Strategy>
        {
            new Strategy
            {
                Author = "SoftwareKater",
                Name = "Always Cooperate",
                States = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        Move = MOVE.Cooperate,
                        StateTransition = new List<int>{0, 0}
                    }
                }
            },
            new Strategy
            {
                Author = "SoftwareKater",
                Name = "Always Defect",
                States = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        Move = MOVE.Defect,
                        StateTransition = new List<int>{0, 0}
                    }
                }
            },
            new Strategy
            {
                Author = "SoftwareKater",
                Name = "Nice Tit for Tat",
                States = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        Move = MOVE.Cooperate,
                        StateTransition = new List<int>{0, 1}
                    },
                    new FiniteStateMachineState
                    {
                        Move = MOVE.Defect,
                        StateTransition = new List<int>{0, 1}
                    }
                }
            },
            new Strategy
            {
                Author = "SoftwareKater",
                Name = "Nasty Tit for Tat",
                States = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        Move = MOVE.Defect,
                        StateTransition = new List<int>{1, 0}
                    },
                    new FiniteStateMachineState
                    {
                        Move = MOVE.Cooperate,
                        StateTransition = new List<int>{1, 0}
                    }
                }
            }
        };
    }
}
