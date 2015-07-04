using System.Xml.Serialization;

namespace Gwent.NET.Model.Serialization
{
    [XmlRoot("redxml")]
    public class RedXml
    {
        [XmlElement("custom")]
        public CustomGwintCardDefinitions GwintCardDefinitions { get; set; }
    }
}
