using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Toxu4.GraphQl.Client
{
    internal class AbstractClassConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            var token = JToken.ReadFrom(reader);

            var typeNameToken = token.Children<JProperty>().FirstOrDefault(p => p.Name == "__typename");
            if (typeNameToken == null)
            {
                return null;
            }

            var implementationType = objectType
                .Assembly.GetTypes()
                .FirstOrDefault(t => 
                    t.IsSubclassOf(objectType) 
                    && 
                    t.Name == $"{typeNameToken.Value}Result");

            return implementationType != null 
                ? token.ToObject(implementationType) 
                : null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAbstract;
        }
    }
}
