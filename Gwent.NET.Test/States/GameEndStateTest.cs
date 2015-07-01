using System.Linq;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.States
{
    public class GameEndStateTest
    {
        [Fact]
        public void InitializeGameEndState()
        {
            var state = new GameEndState();
            var game = new Game
            {
                IsActive = true,
                State = state
            };

            var events = state.Initialize(game).ToList();

            Assert.Empty(events);
            Assert.False(game.IsActive);
        }
    
    }
}