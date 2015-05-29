using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    public class GwintSummonFlag
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}