using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class StartGameCommandTest
    {

        [Fact]
        public void StartGame()
        {
            var game = new Game
            {
                State = new LobbyState(),
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
                            BattleKingCard = new DeckCard
                            {
                                Card = new Card()
                            }
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
                            BattleKingCard = new DeckCard
                            {
                                Card = new Card()
                            }
                        }
                    }
                }
            };
            var command = new StartGameCommand
            {
                SenderUserId = 1
            };

            command.Execute(game);

            Assert.IsType(typeof(RedrawState), command.NextState);
            Assert.True(game.Players.All(p => p.CanUseBattleKingCard));
            Assert.NotNull(game.Players.SingleOrDefault(p => p.IsRoundStarter));
        }

        [Fact]
        public void StartGameWithOneCounterKing()
        {
            var game = new Game
            {
                State = new LobbyState(),
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
                            BattleKingCard = new DeckCard
                            {
                                Card = new Card
                                {
                                    Effect = GwintEffect.CounterKing
                                }
                            }
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
            var command = new StartGameCommand
            {
                SenderUserId = 1
            };

            command.Execute(game);

            Assert.True(game.Players.All(p => !p.CanUseBattleKingCard));
        }

        [Fact]
        public void StartGameWithTwoCounterKing()
        {
            var game = new Game
            {
                State = new LobbyState(),
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
                            BattleKingCard = new DeckCard
                            {
                                Card = new Card
                                {
                                    Effect = GwintEffect.CounterKing
                                }
                            }
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
                            BattleKingCard = new DeckCard
                            {
                                Card = new Card
                                {
                                    Effect = GwintEffect.CounterKing
                                }
                            }
                        }
                    }
                }
            };
            var command = new StartGameCommand
            {
                SenderUserId = 1
            };

            command.Execute(game);

            Assert.True(game.Players.All(p => !p.CanUseBattleKingCard));
        }


        [Fact]
        public void StartGameWithScoiataelFaction()
        {
            var game = new Game
            {
                State = new LobbyState(),
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
                            Faction = GwintFaction.Scoiatael,
                            BattleKingCard = new DeckCard
                            {
                                Card = new Card()
                            }
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
                            BattleKingCard = new DeckCard
                            {
                                Card = new Card()
                            }
                        }
                    }
                }
            };
            var command = new StartGameCommand
            {
                SenderUserId = 1
            };

            command.Execute(game);

            Assert.IsType(typeof(PickStartingPlayerState), command.NextState);
        }
    }
}
