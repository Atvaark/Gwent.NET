using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    public class GwintEffectFlag
    {
        [XmlAttribute("name")]
        public GwintEffect Name { get; set; }
    }
}