using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Model.States.Substates;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class RedrawCardCommandTest
    {
        [Fact]
        public void RedrawLastCard()
        {
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                Deck = new Deck
                {
                    BattleKingCard = new Card()
                },
                HandCards =
                {
                    new Card
                    {
                        Id = 1
                    }
                },
                DeckCards =
                {
                    new Card
                    {
                        Id = 2
                    }
                }
            };
            var game = new Game
            {
                State = new RedrawState
                {
                    Substates =
                    {
                        new RedrawPlayerSubstate
                        {
                            UserId = 1,
                            RedrawCardCount = 1
                        },
                        new RedrawPlayerSubstate
                        {
                            UserId = 2,
                            RedrawCardCount = 0
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
                            BattleKingCard = new Card()
                        }
                    }
                }
            };

            var command = new RedrawCardCommand()
            {
                SenderUserId = 1,
                CardId = 1
            };

            command.Execute(game);

            Assert.IsType(typeof(RoundState), command.NextState);

            var handCard = player1.HandCards.SingleOrDefault();
            Assert.NotNull(handCard);
            Assert.Equal(2, handCard.Id);

            var deckCard = player1.DeckCards.SingleOrDefault();
            Assert.NotNull(deckCard);
            Assert.Equal(1, deckCard.Id);
        }

        [Fact]
        public void RedrawLastCardWhileOpponentCanStillRedraw()
        {
            var redrawPlayer1Substate = new RedrawPlayerSubstate
            {
                UserId = 1,
                RedrawCardCount = 1
            };
            var game = new Game
            {
                State = new RedrawState
                {
                    Substates =
                    {
                        redrawPlayer1Substate,
                        new RedrawPlayerSubstate
                        {
                            UserId = 2,
                            RedrawCardCount = 1
                        }
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
                        Deck = new Deck
                        {
                            BattleKingCard = new Card()
                        },
                        HandCards =
                        {
                            new Card
                            {
                                Id = 1
                            }
                        },
                        DeckCards =
                        {
                            new Card
                            {
                                Id = 2
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
                            BattleKingCard = new Card()
                        }
                    }
                }
            };

            var command = new RedrawCardCommand()
            {
                SenderUserId = 1,
                CardId = 1
            };

            command.Execute(game);

            Assert.Null(command.NextState);
            Assert.Equal(0, redrawPlayer1Substate.RedrawCardCount);
        }

        [Fact]
        public void RedrawFirstOfTwoCard()
        {
            var redrawPlayer1Substate = new RedrawPlayerSubstate
            {
                UserId = 1,
                RedrawCardCount = 2
            };
            var game = new Game
            {
                State = new RedrawState
                {
                    Substates =
                    {
                        redrawPlayer1Substate,
                        new RedrawPlayerSubstate
                        {
                            UserId = 2,
                            RedrawCardCount = 0
                        }
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
                        Deck = new Deck
                        {
                            BattleKingCard = new Card()
                        },
                        HandCards =
                        {
                            new Card
                            {
                                Id = 1
                            }
                        },
                        DeckCards =
                        {
                            new Card
                            {
                                Id = 2
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
                            BattleKingCard = new Card()
                        }
                    }
                }
            };

            var command = new RedrawCardCommand()
            {
                SenderUserId = 1,
                CardId = 1
            };

            command.Execute(game);

            Assert.Null(command.NextState);

            Assert.Equal(1, redrawPlayer1Substate.RedrawCardCount);
        }
    }
}