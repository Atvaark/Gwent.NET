using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Model.States.Substates;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class PickStartingPlayerCommandTest
    {
        [Fact]
        public void PickStartingPlayer()
        {
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                IsOwner = true,
                Deck = new Deck
                {
                    BattleKingCard = new DeckCard()
                }
            };
            var game = new Game
            {
                State = new PickStartingPlayerState
                {
                    Substates =
                    {
                        new PickStartingPlayerSubstate
                        {
                            UserId = 1,
                            CanPickStartingPlayer = true
                        },
                        new PickStartingPlayerSubstate
                        {
                            UserId = 2
                        },
                    }
                },
                Players =
                {
                    player1,
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        },
                        Deck = new Deck
                        {
                            BattleKingCard = new DeckCard()
                        }
                    }
                }
            };
            var command = new PickStartingPlayerCommand
            {
                SenderUserId = 1,
                StartingPlayerUserId = 1
            };

            command.Execute(game);

            Assert.True(player1.IsRoundStarter);
            Assert.IsType(typeof(RedrawState), command.NextState);
        }

        [Fact]
        public void PickStartingPlayerBeforeOpponent()
        {
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                IsOwner = true,
                Deck = new Deck
                {
                    BattleKingCard = new DeckCard()
                }
            };
            var game = new Game
            {
                State = new PickStartingPlayerState
                {
                    Substates =
                    {
                        new PickStartingPlayerSubstate
                        {
                            UserId = 1,
                            CanPickStartingPlayer = true
                        },
                        new PickStartingPlayerSubstate
                        {
                            UserId = 2,
                            CanPickStartingPlayer = true
                        }
                    }
                },
                Players =
                {
                    player1,
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        },
                        Deck = new Deck
                        {
                            BattleKingCard = new DeckCard()
                        }
                    }
                }
            };
            var command = new PickStartingPlayerCommand
            {
                SenderUserId = 1,
                StartingPlayerUserId = 1
            };

            command.Execute(game);

            Assert.Null(command.NextState);
        }

        [Fact]
        public void PickSameStartingPlayerAsOpponent()
        {
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                IsOwner = true,
                Deck = new Deck
                {
                    BattleKingCard = new DeckCard()
                }
            };
            var player2 = new Player
            {
                User = new User
                {
                    Id = 2
                },
                Deck = new Deck
                {
                    BattleKingCard = new DeckCard()
                }
            };
            var game = new Game
            {
                State = new PickStartingPlayerState
                {
                    Substates =
                    {
                        new PickStartingPlayerSubstate
                        {
                            UserId = 1,
                            CanPickStartingPlayer = true
                        },
                        new PickStartingPlayerSubstate
                        {
                            UserId = 2,
                            CanPickStartingPlayer = true,
                            StartingPlayerUserId = 2
                        },
                    }
                },
                Players =
                {
                    player1,
                    player2
                }
            };
            var command = new PickStartingPlayerCommand
            {
                SenderUserId = 1,
                StartingPlayerUserId = 2
            };

            command.Execute(game);

            Assert.True(player2.IsRoundStarter);
            Assert.IsType(typeof(RedrawState), command.NextState);
        }

        [Fact]
        public void PickDifferentStartingPlayerThanOpponent()
        {
            var game = new Game
            {
                State = new PickStartingPlayerState
                {
                    Substates =
                    {
                        new PickStartingPlayerSubstate
                        {
                            UserId = 1,
                            CanPickStartingPlayer = true
                        },
                        new PickStartingPlayerSubstate
                        {
                            UserId = 2,
                            CanPickStartingPlayer = true,
                            StartingPlayerUserId = 2
                        },
                    }
                },
                Players =
                {
                    new Player
                    {
                        User = new User
                        {
                            Id = 1
                        },
                        IsOwner = true,
                        Deck = new Deck
                        {
                            BattleKingCard = new DeckCard()
                        }
                    },
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        },
                        Deck = new Deck
                        {
                            BattleKingCard = new DeckCard()
                        }
                    }
                }
            };
            var command = new PickStartingPlayerCommand
            {
                SenderUserId = 1,
                StartingPlayerUserId = 1
            };

            command.Execute(game);

            var coinTossEvent = command.Events.SingleOrDefault();
            Assert.NotNull(coinTossEvent);
            Assert.IsType(typeof(CoinTossEvent), coinTossEvent);
            Assert.IsType(typeof(RedrawState), command.NextState);
        }
    }
}