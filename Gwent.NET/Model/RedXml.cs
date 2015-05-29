using System.Xml.Serialization;

namespace Gwent.NET.Model
{
    [XmlRoot("redxml")]
    public class RedXml
    {
        [XmlElement("custom")]
        public Custom Custom { get; set; }
    }
}
