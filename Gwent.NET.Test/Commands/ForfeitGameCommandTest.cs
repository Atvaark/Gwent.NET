using Gwent.NET.Commands;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class ForfeitGameCommandTest
    {
        [Fact]
        public void ForfeitGame()
        {
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                }
            };
            var player2 = new Player
            {
                User = new User
                {
                    Id = 2
                }
            };
            var game = new Game
            {
                Players =
                {
                    player1,
                    player2
                }
            };

            var command = new ForfeitGameCommand
            {
                SenderUserId = 1
            };

            command.Execute(game);

            Assert.IsType(typeof(GameEndState), command.NextState);
            Assert.False(player1.IsVictor);
            Assert.True(player2.IsVictor);
        }
    }
}