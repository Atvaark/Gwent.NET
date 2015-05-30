using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Repositories
{
    public class CardRepository : ICardRepository
    {
        private List<Card> _cards;
        
        public IEnumerable<Card> Get()
        {
            if (_cards == null)
            {
                _cards = ReadCustomGwintCardDefinitions().ToList();
            }
            return _cards;
        }

        public Card Find(int id)
        {
            return Get().FirstOrDefault(c => c.Index == id);
        }

        private IEnumerable<Card> ReadCustomGwintCardDefinitions()
        {
            //var gwintCards = ReadGwintCardsFromFile("Resources\\def_gwint_cards_final.xml");
            var gwintCards = ReadGwintCardsFromResource(Properties.Resources.def_gwint_cards_final);
            foreach (var card in gwintCards.GwintCardDefinitions)
            {
                yield return card;
            }
            
            //var gwintBattleKingCards = ReadGwintCardsFromFile("Resources\\def_gwint_battle_king_cards.xml");
            var gwintBattleKingCards = ReadGwintCardsFromResource(Properties.Resources.def_gwint_battle_king_cards);
            foreach (var card in gwintBattleKingCards.GwintBattleKingCardDefinitions)
            {
                card.IsBattleKing = true;
                yield return card;                
            }
        }
        private CustomGwintCardDefinitions ReadGwintCardsFromFile(string filePath)
        {
            using (FileStream gwintCardsFile = File.OpenRead(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RedXml));
                var redXml = serializer.Deserialize(gwintCardsFile) as RedXml;
                if (redXml == null)
                {
                    throw new InvalidOperationException("Unable to deserialize the RedXml file.");
                }
                return redXml.GwintCardDefinitions;
            }
        }
        private CustomGwintCardDefinitions ReadGwintCardsFromResource(string resource)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(resource)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RedXml));
                var redXml = serializer.Deserialize(reader) as RedXml;
                if (redXml == null)
                {
                    throw new InvalidOperationException("Unable to deserialize the RedXml file.");
                }
                return redXml.GwintCardDefinitions;
            }
        }
    }
}