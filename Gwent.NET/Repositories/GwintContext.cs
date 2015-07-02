using System.Data.Common;
using System.Data.Entity;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Model.States.Substates;

namespace Gwent.NET.Repositories
{
    public class GwintContext : DbContext, IGwintContext
    {
        public GwintContext()
        {
            Database.SetInitializer<GwintContext>(new GwintContextInitializer());
        }

        public GwintContext(DbConnection existingConnection) : base(existingConnection, true)
        {
            Database.SetInitializer<GwintContext>(new GwintContextInitializer());
        }

        public IDbSet<Card> Cards { get; set; }
        public IDbSet<Deck> Decks { get; set; }
        public IDbSet<Game> Games { get; set; }
        public IDbSet<Player> Players { get; set; }
        public IDbSet<State> States { get; set; }
        public IDbSet<Substate> Substates { get; set; }
        public IDbSet<User> Users { get; set; }

        public void Reload<T>(T entity) where T : class
        {
            Entry(entity).Reload();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deck>()
                .HasMany(d => d.Cards)
                .WithMany(c => c.Decks)
                .Map(dc =>
                {
                    dc.ToTable("DeckCards");
                    dc.MapLeftKey("DeckId");
                    dc.MapRightKey("CardId");
                });

            modelBuilder.Entity<Player>()
                .HasMany(p => p.HandCards)
                .WithMany(c => c.HandPlayer)
                .Map(pc =>
                {
                    pc.ToTable("PlayerHand");
                    pc.MapLeftKey("PlayerId");
                    pc.MapRightKey("CardId");
                });

            modelBuilder.Entity<Player>()
                .HasMany(p => p.DeckCards)
                .WithMany(c => c.DeckPlayer)
                .Map(pc =>
                {
                    pc.ToTable("PlayerDeck");
                    pc.MapLeftKey("PlayerId");
                    pc.MapRightKey("CardId");
                });

            modelBuilder.Entity<Player>()
                .HasMany(p => p.GraveyardCards)
                .WithMany(c => c.GraveyardPlayer)
                .Map(pc =>
                {
                    pc.ToTable("PlayerGraveyard");
                    pc.MapLeftKey("PlayerId");
                    pc.MapRightKey("CardId");
                });

            modelBuilder.Entity<GwintSummonFlag>()
                .ToTable("CardsSummonCards")
                .HasRequired(s => s.SummonCard)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
