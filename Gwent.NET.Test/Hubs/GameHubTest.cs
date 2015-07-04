using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Autofac;
using Effort;
using Gwent.NET.DTOs;
using Gwent.NET.Extensions;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Repositories;
using Gwent.NET.Webservice.Hubs;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using Xunit;

namespace Gwent.NET.Test.Hubs
{
    public class GameHubFixture
    {
        public void Setup()
        {
            // TODO: Test if this prevents the first test from running really long
            DbConnection connection = DbConnectionFactory.CreateTransient();
            using (var context = new GwintContext(connection))
            {
                context.Cards.Add(new Card());
                context.SaveChanges();
            }
        }
    }

    public class GameHubTest : IClassFixture<GameHubFixture>
    {
        public GameHubTest(GameHubFixture fixture)
        {
            fixture.Setup();
        }

        [Fact]
        public void OnConnect()
        {
            var scopeMock = new Mock<ILifetimeScope>();
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);

            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            hub.OnConnected();
        }


        [Fact]
        public void OnDisconnectGracefully()
        {
            var scopeMock = new Mock<ILifetimeScope>();
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);

            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            hub.OnDisconnected(true);
        }

        [Fact]
        public void OnDisconnectUngracefully()
        {
            var scopeMock = new Mock<ILifetimeScope>();
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);

            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            hub.OnDisconnected(false);
        }
        
        [Fact]
        public void Authenticate()
        {
            var scopeMock = new Mock<ILifetimeScope>();
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();


            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);

            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            hub.Authenticate();
        }

        [Fact]
        public void BrowseGames()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            var game = new Game
            {
                Id = 1,
                IsActive = true,
                State = new LobbyState(),
                Players =
                {
                    new Player()
                }
            };
            using (var gwintContext = new GwintContext(connection))
            {
                gwintContext.Games.Add(game);
                gwintContext.SaveChanges();
            }

            var scopeMock = new Mock<ILifetimeScope>();
            scopeMock.SetupResolve<ILifetimeScope, IGwintContext>(new GwintContext(connection));
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();
            
            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);

            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            var result = hub.BrowseGames();

            Assert.Null(result.Error);
            var gameBrowseDtos = result.Data;
            Assert.NotNull(gameBrowseDtos);
            Assert.Equal(1, gameBrowseDtos.Count);
            Assert.Contains(gameBrowseDtos, g => g.Id == game.Id && g.State == game.State.Name && g.PlayerCount == game.Players.Count);
        }

        [Fact]
        public void GetActiveGame()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            var game = new Game
            {
                Id = 1,
                IsActive = true,
                State = new LobbyState(),
                Players =
                {
                    new Player
                    {
                        User = new User
                        {
                            Id = 1
                        }
                    },
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        }
                    }
                }
            };
            using (var gwintContext = new GwintContext(connection))
            {
                gwintContext.Games.Add(game);
                gwintContext.SaveChanges();
            }

            var scopeMock = new Mock<ILifetimeScope>();
            scopeMock.SetupResolve<ILifetimeScope, IGwintContext>(new GwintContext(connection));
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);
            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            var result = hub.GetActiveGame();
            Assert.Null(result.Error);
            var gameDto = result.Data;
            Assert.Equal(game.Id, gameDto.Id);
            Assert.Equal(game.State.Name, gameDto.State);
            var playerDtos = gameDto.Players;
            Assert.Equal(2, playerDtos.Count);
            Assert.Contains(playerDtos, p => p.Key == Constants.PlayerKeySelf && p.Value.Id == 1);
            Assert.Contains(playerDtos, p => p.Key == Constants.PlayerKeyOpponent && p.Value.Id == 2);
        }

        [Fact]
        public void GetActiveGameWithInactiveGame()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            var game = new Game
            {
                Id = 1,
                IsActive = false,
                Players =
                {
                    new Player
                    {
                        User = new User
                        {
                            Id = 1
                        }
                    },
                    new Player
                    {
                        User = new User
                        {
                            Id = 2
                        }
                    }
                }
            };
            using (var gwintContext = new GwintContext(connection))
            {
                gwintContext.Games.Add(game);
                gwintContext.SaveChanges();
            }

            var scopeMock = new Mock<ILifetimeScope>();
            scopeMock.SetupResolve<ILifetimeScope, IGwintContext>(new GwintContext(connection));
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);
            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            var result = hub.GetActiveGame();
            Assert.NotNull(result.Error);
            Assert.Null(result.Data);
        }

        [Fact]
        public void GetActiveGameWithWithoutGames()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();

            var scopeMock = new Mock<ILifetimeScope>();
            scopeMock.SetupResolve<ILifetimeScope, IGwintContext>(new GwintContext(connection));
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);
            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            var result = hub.GetActiveGame();
            Assert.NotNull(result.Error);
            Assert.Null(result.Data);
        }


        [Fact]
        public void CreateGame()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            var user = new User
            {
                Id = 1
            };
            using (var gwintContext = new GwintContext(connection))
            {
                gwintContext.Cards.AddRange(TestCardProvider.GetDefaultCards());
                gwintContext.Users.Add(user);
                gwintContext.SaveChanges();
            }

            var scopeMock = new Mock<ILifetimeScope>();
            scopeMock.SetupResolve<ILifetimeScope, IGwintContext>(new GwintContext(connection));
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);
            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            var result = hub.CreateGame();
            Assert.Null(result.Error);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void JoinGame()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            using (var gwintContext = new GwintContext(connection))
            {
                gwintContext.Cards.AddRange(TestCardProvider.GetDefaultCards());
                gwintContext.Users.Add(new User
                {
                    Id = 1
                });
                var user2 = new User
                {
                    Id = 2
                };
                gwintContext.Users.Add(user2);
                gwintContext.Games.Add(new Game
                {
                    Id = 1,
                    State = new LobbyState(),
                    Players = new List<Player>
                    {
                        new Player
                        {
                            User = user2
                        }
                    }
                });
                gwintContext.SaveChanges();
            }
            var scopeMock = new Mock<ILifetimeScope>();
            scopeMock.SetupResolve<ILifetimeScope, IGwintContext>(new GwintContext(connection));
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            int gameId = 1;
            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);
            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };

            var result = hub.JoinGame(gameId);

            Assert.Null(result.Error);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public void RecieveClientCommand()
        {
            DbConnection connection = DbConnectionFactory.CreateTransient();
            long nextCardId = 1;

            var createNewCards = new Func<int, int, ICollection<DeckCard>>((deckId, count) =>
            {
                var cards = new List<DeckCard>();
                for (int i = 0; i < count; i++)
                {
                    cards.Add(new DeckCard
                    {
                        //DeckId = deckId,
                        Card = new Card
                        {
                            Id = nextCardId++,                            
                        }
                    });
                }
                return cards;
            });

            using (var gwintContext = new GwintContext(connection))
            {
                var user1 = new User
                {
                    Id = 1
                };
                var user2 = new User
                {
                    Id = 2
                };
                //gwintContext.Cards.AddRange(TestCardProvider.GetDefaultCards());
                gwintContext.Users.Add(user1);
                gwintContext.Users.Add(user2);
                var player1BattleKingCard = createNewCards(1, 1).Single().Card;
                var player2BattleKingCard = createNewCards(2, 1).Single().Card;
                var player1DeckCards = createNewCards(1, Constants.MinDeckCardCount);
                var player2DeckCards = createNewCards(2, Constants.MinDeckCardCount);

                gwintContext.Cards.Add(player1BattleKingCard);
                gwintContext.Cards.Add(player2BattleKingCard);
                gwintContext.Cards.AddRange(player1DeckCards.Select(c => c.Card));
                gwintContext.Cards.AddRange(player2DeckCards.Select(c => c.Card));

                gwintContext.Games.Add(new Game
                {
                    Id = 1,
                    IsActive = true,
                    State = new LobbyState(),
                    Players = new List<Player>
                    {
                        new Player
                        {
                            Id = 1,
                            User = user1,
                            IsOwner = true,
                            Deck = new Deck
                            {
                                Id = 1,
                                BattleKingCard = player1BattleKingCard.ToDeckCard(),
                                Cards = player1DeckCards
                            }
                        },
                        new Player
                        {
                            Id = 2,
                            User = user2,
                            Deck =  new Deck
                            {
                                Id = 2,
                                BattleKingCard = player2BattleKingCard.ToDeckCard(),
                                Cards = player2DeckCards
                            }
                        }
                    }
                });
                
                gwintContext.SaveChanges();
            }
            var scopeMock = new Mock<ILifetimeScope>();
            scopeMock.SetupResolve<ILifetimeScope, IGwintContext>(new GwintContext(connection));
            var userConnectionMapMock = new Mock<IUserConnectionMap>();
            userConnectionMapMock.SetupMapping();
            scopeMock.SetupResolve<ILifetimeScope, IUserConnectionMap>(userConnectionMapMock.Object);
            var rootScopeMock = new Mock<ILifetimeScope>();
            rootScopeMock.Setup(s => s.BeginLifetimeScope()).Returns(scopeMock.Object);
            var clientsMock = new Mock<IHubCallerConnectionContext<dynamic>>();
            clientsMock.SetupClients();

            var userName = "User1";
            var userId = "1";
            var connectionID = "13245";
            var hubCallerContextMock = CreateHubCallerContextMock(userName, userId, connectionID);
            GameHub hub = new GameHub(rootScopeMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object
            };
            var commandDto = new CommandDto();
            commandDto.Type = CommandType.StartGame;

            var result = hub.RecieveClientCommand(commandDto);

            Assert.Null(result.Error);
        }

        private static Mock<HubCallerContext> CreateHubCallerContextMock(string userName, string userId, string connectionID)
        {
            var claimsIdentityMock = new Mock<ClaimsIdentity>();
            claimsIdentityMock.SetupGet(i => i.Name).Returns(userName);
            claimsIdentityMock
                .Setup(i => i.FindFirst(ClaimTypes.Name))
                .Returns(new Claim(ClaimTypes.Name, userName));
            claimsIdentityMock
                .Setup(i => i.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));
            claimsIdentityMock.Setup(i => i.FindFirst(It.Is<Predicate<Claim>>(c => false)));
            var principalMock = new Mock<IPrincipal>();
            principalMock.SetupGet(p => p.Identity).Returns(claimsIdentityMock.Object);
            var hubCallerContextMock = new Mock<HubCallerContext>();
            hubCallerContextMock.SetupGet(h => h.User).Returns(principalMock.Object);
            hubCallerContextMock.SetupGet(h => h.ConnectionId).Returns(connectionID);
            return hubCallerContextMock;
        }
    }
}