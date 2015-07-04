using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Gwent.NET.Model;
using Gwent.NET.Model.Serialization;
using Gwent.NET.Properties;

namespace Gwent.NET.Repositories
{
    // TODO: Use CreateDatabaseIfNotExists as the base class when the the model is done.
    //public class GwintContextInitializer : DropCreateDatabaseAlways<GwintContext>
    public class GwintContextInitializer : CreateDatabaseIfNotExists<GwintContext>
    {
        protected override void Seed(GwintContext context)
        {
            var cards = ReadCustomGwintCardDefinitions().ToList();
            context.Cards.AddRange(cards);
            base.Seed(context);
        }

        private IEnumerable<Card> ReadCustomGwintCardDefinitions()
        {
            //var gwintCards = ReadGwintCardsFromFile("Resources\\def_gwint_cards_final.xml");
            var gwintCards = ReadGwintCardsFromResource(Resources.def_gwint_cards_final);
            foreach (var card in gwintCards.GwintCardDefinitions)
            {
                card.Types = card.GetGwintTypes();
                card.Effect = card.GetGwintEffects();
                yield return card;
            }

            //var gwintBattleKingCards = ReadGwintCardsFromFile("Resources\\def_gwint_battle_king_cards.xml");
            var gwintBattleKingCards = ReadGwintCardsFromResource(Resources.def_gwint_battle_king_cards);
            foreach (var card in gwintBattleKingCards.GwintBattleKingCardDefinitions)
            {
                card.Types = card.GetGwintTypes();
                card.Effect = card.GetGwintEffects();
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
