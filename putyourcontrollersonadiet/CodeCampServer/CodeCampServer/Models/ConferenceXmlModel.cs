using System;
using System.Xml.Serialization;

namespace CodeCampServerLite.Models
{
    public class ConferenceXmlModel
    {
        [XmlAttribute]
        public string EventName;
        public string SessionCount;
        public string AttendeeCount;
    }
}