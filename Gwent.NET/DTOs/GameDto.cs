using System.Collections.Generic;

namespace Gwent.NET.DTOs
{
    public class GameDto
    {
        public GameDto()
        {
            Players = new Dictionary<string, PlayerDto>();
        }

        public long Id { get; set; }
        public string State { get; set; }
        public Dictionary<string, PlayerDto> Players { get; set; }
    }
}