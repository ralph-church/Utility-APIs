using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace integration.services.kafka.shared.converters
{
    public class HeaderConverter : JsonConverter
    {
        private class HeaderContainer
        {
            public string? Key { get; set; }
            public byte[]? Value { get; set; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Header) || objectType == typeof(IHeader);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var headerContainer = serializer.Deserialize<HeaderContainer>(reader)!;
            return new Header(headerContainer.Key, headerContainer.Value);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var header = (IHeader)value!;
            var container = new HeaderContainer { Key = header.Key, Value = header.GetValueBytes() };
            serializer.Serialize(writer, container);
        }
    }

    public class HeadersConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Headers);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else
            {
                var surrogate = serializer.Deserialize<List<Header>>(reader)!;
                var headers = new Headers();

                foreach (var header in surrogate)
                {
                    headers.Add(header.Key, header.GetValueBytes());
                }
                return headers;
            }
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
