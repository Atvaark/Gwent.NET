using System.Xml.Serialization;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.Model.Serialization
{
    public class GwintEffectFlag
    {
        [XmlAttribute("name")]
        public GwintEffect Name { get; set; }
    }
}