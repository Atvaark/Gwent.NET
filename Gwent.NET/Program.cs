using System.IO;
using System.Xml.Serialization;
using Gwent.NET.Model;

namespace Gwent.NET
{
    public class Program
    {
        public static void Main()
        {
            using (FileStream battleKingCardsFile = File.OpenRead("Resources\\def_gwint_battle_king_cards.xml"))
            using (FileStream gwintCardsFile = File.OpenRead("Resources\\def_gwint_cards_final.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RedXml));
                RedXml battleKingCardsRedXml = serializer.Deserialize(battleKingCardsFile) as RedXml;
                RedXml gwintCardsRedXml = serializer.Deserialize(gwintCardsFile) as RedXml;
            }
        }
    }
}