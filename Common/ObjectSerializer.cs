using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace TempleGardens
{
    public class ObjectSerializer : IDisposable
    {
        public ObjectSerializer() { }

        public void SerializeObject<T>(string fileName, T objGraph)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, objGraph);
            }
        }

        public T DeserializeObject<T>(string fileName)
        {
            T objGraph;

            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                objGraph = (T)formatter.Deserialize(stream);
            }

            return objGraph;
        }
        public void Dispose() { }
    }
}
