
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using ReInvented.DataAccess.Services;

namespace ReInvented.BackwardCompatibleSerializer.Services
{
    public class JsonDataSerializer
    {
        public static JObject Deserialize(string filePath)
        {
            string json = PersistenceService.TryReadFromFile(filePath);
            return JsonConvert.DeserializeObject<JObject>(json);
        }

        public static T MapPropertiesFromJson<T>(JObject jObject) where T : new()
        {
            //JObject jObject = JsonConvert.DeserializeObject<JObject>(json);
            return (T)ObjectMapper.MapObject(typeof(T), jObject);
        }

    }
}
