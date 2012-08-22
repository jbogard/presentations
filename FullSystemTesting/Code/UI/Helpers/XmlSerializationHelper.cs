using System.IO;
using System.Xml.Serialization;

namespace CodeCampServerLite.UI.Helpers.XmlSerialization
{
    public static class XmlSerializationHelper
    {
        public static string Serialize<T>(this T value)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var writer = new StringWriter();
            xmlSerializer.Serialize(writer, value);

            return writer.ToString();
        }

        public static T Deserialize<T>(this string rawValue)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var reader = new StringReader(rawValue);

            T value = (T)xmlSerializer.Deserialize(reader);
            return value;
        }
    }
}