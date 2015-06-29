using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    public class GwintTypeFlag
    {
        [XmlAttribute("name")]
        public GwintType Name { get; set; }
    }
}