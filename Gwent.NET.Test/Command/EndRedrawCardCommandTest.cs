using Gwent.NET.Commands;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Model.States.Substates;
using Xunit;

namespace Gwent.NET.Test.Command
{
    public class EndRedrawCardCommandTest
    {
        [Fact]
        public void EndRedraw()
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
                    },
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        },
                    }
                }
            };

            var command = new EndRedrawCardCommand
            {
                SenderUserId = 1
            };

            command.Execute(game);

            Assert.IsType(typeof(RoundState), command.NextState);
            Assert.Equal(0, redrawPlayer1Substate.RedrawCardCount);
        }

        [Fact]
        public void EndRedrawWhileOpponentCanStillRedraw()
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
                    },
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        },
                    }
                }
            };

            var command = new EndRedrawCardCommand
            {
                SenderUserId = 1
            };

            command.Execute(game);

            Assert.Null(command.NextState);
            Assert.Equal(0, redrawPlayer1Substate.RedrawCardCount);
        }

    }
}