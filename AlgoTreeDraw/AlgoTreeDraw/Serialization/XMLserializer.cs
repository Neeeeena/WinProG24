using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlgoTreeDraw.Serialization
{
    public class XMLserializer
    {
        public static XMLserializer Instance { get; } = new XMLserializer();

        private XMLserializer() { }

        public async void AsyncSerializeToFile(Diagram diagram, string path)
        {
            await Task.Run(() => SerializeToFile(diagram, path));
        }

        private void SerializeToFile(Diagram diagram, string path)
        {
            using (FileStream stream = File.Create(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                serializer.Serialize(stream, diagram);
            }
        }

        public Task<Diagram> AsyncDeserializeFromFile(string path)
        {
            return Task.Run(() => DeserializeFromFile(path));
        }

        private Diagram DeserializeFromFile(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                Diagram diagram = serializer.Deserialize(stream) as Diagram;

                return diagram;
            }
        }


    }
}
