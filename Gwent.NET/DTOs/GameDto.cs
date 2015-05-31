using System.Collections.Generic;

namespace Gwent.NET.DTOs
{
    public class GameDto
    {
        public GameDto()
        {
            Players = new List<PlayerDto>();
        }

        public int Id { get; set; }
        public string StateType { get; set; }
        public List<PlayerDto> Players { get; set; }
    }
}