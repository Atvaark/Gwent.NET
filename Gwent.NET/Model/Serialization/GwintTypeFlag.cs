using System.Xml.Serialization;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.Model.Serialization
{
    public class GwintTypeFlag
    {
        [XmlAttribute("name")]
        public GwintType Name { get; set; }
    }
}