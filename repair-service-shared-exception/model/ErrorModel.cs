using System.Text.Json;
using System.Text.Json.Serialization;

namespace repair.service.shared.exception.model
{
    /// <summary>
    /// Class that holds the expection information.
    /// </summary>
    public class ErrorModel
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}