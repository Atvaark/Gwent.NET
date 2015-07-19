using Newtonsoft.Json;

namespace Gwent.NET.DTOs
{
    public class PlayerCardDto
    {
        public long Id { get; set; }

        public long CardId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? EffectivePower { get; set; }

        public bool IsActive { get; set; }
    }
}