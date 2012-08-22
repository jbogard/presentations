using System.Xml.Serialization;

namespace CodeCampServerLite.UI.Models
{
    public class ConferenceXmlModel
    {
        [XmlAttribute]
        public string EventName;
        public string SessionCount;
        public string AttendeeCount;
    }
}