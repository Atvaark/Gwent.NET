using System.Linq;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.States
{
    public class LobbyStateTest
    {
        [Fact]
        public void InitializeLobbyState()
        {
            var state = new LobbyState();
            var game = new Game
            {
                State = state
            };

            var events = state.Initialize(game).ToList();

            Assert.Empty(events);
        }
    }
}
