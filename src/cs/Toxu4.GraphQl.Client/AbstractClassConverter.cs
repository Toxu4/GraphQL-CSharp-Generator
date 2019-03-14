using System;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Toxu4.GraphQl.Client
{
    internal class AbstractClassConverter : JsonConverter
    {
        private static readonly ConcurrentDictionary<(Type type, string name), Type> Implementations = new ConcurrentDictionary<(Type type , string name), Type>(); 
        
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

            var implementationType = Implementations
                .GetOrAdd(
                    (objectType, typeNameToken.Value.ToString()), 
                    tuple =>
                        tuple.type
                            .Assembly.GetTypes()
                            .FirstOrDefault(t => 
                                t.IsSubclassOf(tuple.type) 
                                && 
                                t.Name == $"{tuple.name}Result"));

            return implementationType != null 
                ? token.ToObject(implementationType, serializer) 
                : null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAbstract;
        }
    }
}
