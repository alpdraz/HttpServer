using Newtonsoft.Json;
using System.IO;

namespace HttpServer.Models
{
    public class Json
    {
        public static void Save(License license, string filepath)
        {
            string json = JsonConvert.SerializeObject(license, Formatting.Indented);
            File.WriteAllText(filepath, json);
        }
    }
}
