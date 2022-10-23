using GOS.Models;
using System.ComponentModel;

namespace GOS.Services;

public class TournamentService
{
    public TournamentResult RunTournament(/*TournamentConfig tournamentConfig*/)
    {
        TournamentConfig tournamentConfig = new TournamentConfig
        {
            strategies = GetMockStrategies()
        };

        if (!VerifyConfig(tournamentConfig))
        {
            throw new Exception("Must provide at least 2 strategies to run a tournament.");
        }

        Strategy agentA = tournamentConfig.strategies[0];
        Strategy agentB = tournamentConfig.strategies[1];

        List<RoundProtocoll> RoundProtocolls = new List<RoundProtocoll> { };

        int StateOfAgentA = 0;
        int StateOfAgentB = 0;

        for (int i = 0; i < tournamentConfig.rounds; i++)
        {
            RoundProtocoll RoundProtocoll = new RoundProtocoll { };

            RoundProtocoll.StateA = StateOfAgentA;
            RoundProtocoll.StateB = StateOfAgentB;

            MOVE agentAMove = agentA.states[StateOfAgentA].move;
            MOVE agentBMove = agentB.states[StateOfAgentB].move;

            RoundProtocoll.moveA = agentAMove;
            RoundProtocoll.moveB = agentBMove;

            int PayoffA = tournamentConfig.payoffMatrix[((int)agentAMove)][((int)agentBMove)];
            int PayoffB = tournamentConfig.payoffMatrix[((int)agentBMove)][((int)agentAMove)];

            RoundProtocoll.payoffA = PayoffA;
            RoundProtocoll.payoffB = PayoffB;

            StateOfAgentA = agentA.states[StateOfAgentA].stateTransition[((int)agentBMove)];
            StateOfAgentB = agentB.states[StateOfAgentB].stateTransition[((int)agentAMove)];

            RoundProtocolls.Add(RoundProtocoll);
        }

        int GamePayoffA = RoundProtocolls.Select(RoundProtocoll => RoundProtocoll.payoffA).Aggregate(0, (acc, x) => acc + x);
        int GamePayoffB = RoundProtocolls.Select(RoundProtocoll => RoundProtocoll.payoffB).Aggregate(0, (acc, x) => acc + x);

        GameProtocoll gameProtocoll = new GameProtocoll
        {
            NameOfAgentA = agentA.name,
            NameOfAgentB = agentB.name,
            GamePayoffOfAgentA = GamePayoffA,
            GamePayoffOfAgentB = GamePayoffB,
            RoundProtocolls = RoundProtocolls,
        };

        ConsoleLogProtocoll(gameProtocoll);

        return new TournamentResult
        {
            winner = GamePayoffA == GamePayoffB ? null : (GamePayoffA > GamePayoffB ? agentA : agentB),
            GameProtocolls = new List<GameProtocoll> { gameProtocoll },
        };
    }

    private bool VerifyConfig(TournamentConfig tournamentConfig)
    {
        if (tournamentConfig.strategies.Count() < 2)
        {
            return false;
        }
        return true;
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
                author = "SoftwareKater",
                name = "Always Cooperate",
                states = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        move = MOVE.Cooperate,
                        stateTransition = new List<int>{0, 0}
                    }
                }
            },
            new Strategy
            {
                author = "SoftwareKater",
                name = "Always Defect",
                states = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        move = MOVE.Defect,
                        stateTransition = new List<int>{0, 0}
                    }
                }
            },
            new Strategy
            {
                author = "SoftwareKater",
                name = "Nice Tit for Tat",
                states = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        move = MOVE.Cooperate,
                        stateTransition = new List<int>{0, 1}
                    },
                    new FiniteStateMachineState
                    {
                        move = MOVE.Defect,
                        stateTransition = new List<int>{0, 1}
                    }
                }
            },
            new Strategy
            {
                author = "SoftwareKater",
                name = "Nasty Tit for Tat",
                states = new List<FiniteStateMachineState>
                {
                    new FiniteStateMachineState
                    {
                        move = MOVE.Defect,
                        stateTransition = new List<int>{1, 0}
                    },
                    new FiniteStateMachineState
                    {
                        move = MOVE.Cooperate,
                        stateTransition = new List<int>{1, 0}
                    }
                }
            }
        };
    }
}
