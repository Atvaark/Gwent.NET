using Gwent.NET.Commands;
using Gwent.NET.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gwent.NET.DTOs
{
    public class CommandDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandType Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CardId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public GwintSlot? Slot { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? StartPlayerId { get; set; }
    }
}
