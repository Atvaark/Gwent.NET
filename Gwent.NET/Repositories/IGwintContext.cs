using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Model.States.Substates;

namespace Gwent.NET.Repositories
{
    public interface IGwintContext
    {
        IDbSet<Card> Cards { get; set; }
        IDbSet<Deck> Decks { get; set; }
        IDbSet<Game> Games { get; set; }
        IDbSet<Player> Players { get; set; }
        IDbSet<State> States { get; set; }
        IDbSet<Substate> Substates { get; set; }
        IDbSet<User> Users { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}