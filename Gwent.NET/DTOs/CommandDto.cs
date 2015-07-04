using Gwent.NET.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gwent.NET.DTOs
{
    public class CommandDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandType Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? CardId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? ResurrectCardId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public GwintSlot? Slot { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? StartingPlayerUserId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? GameId { get; set; }
        
    }
}
