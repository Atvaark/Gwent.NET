using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    public class Custom
    {
        [XmlArray("gwint_battle_king_card_definitions", IsNullable = true)]
        [XmlArrayItem("card")]
        public List<Card> GwintBattleKingCardDefinitions { get; set; }

        [XmlArray("gwint_card_definitions_final", IsNullable = true)]
        [XmlArrayItem("card")]
        public List<Card> GwintCardDefinitionsFinal { get; set; }

    }
}