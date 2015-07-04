using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.Model
{
    public class Player
    {
        public Player()
        {
            HandCards = new HashSet<PlayerCard>();
            DeckCards = new HashSet<PlayerCard>();
            GraveyardCards = new HashSet<PlayerCard>();
            CardSlots = new HashSet<PlayerCardSlot>();
        }

        [Key]
        public long Id { get; set; }

        public virtual User User { get; set; }

        public virtual Deck Deck { get; set; }

        public bool IsOwner { get; set; }

        public bool IsVictor { get; set; }

        public bool IsRoundStarter { get; set; }

        public bool IsTurn { get; set; }

        public bool IsPassing { get; set; }

        public bool CanUseBattleKingCard { get; set; }

        public int Lives { get; set; }

        public GwintFaction Faction { get; set; }

        public virtual PlayerCard BattleKingCard { get; set; }

        public virtual ICollection<PlayerCard> DeckCards { get; set; }

        public virtual ICollection<PlayerCard> HandCards { get; set; }

        public virtual ICollection<PlayerCard> GraveyardCards { get; set; }
        
        [InverseProperty("Player")]
        public virtual ICollection<PlayerCardSlot> CardSlots  { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
        
    }
}