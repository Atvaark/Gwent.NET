using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    [XmlRoot("redxml")]
    public class RedXml
    {
        [XmlElement("custom")]
        public CustomGwintCardDefinitions GwintCardDefinitions { get; set; }
    }
}
