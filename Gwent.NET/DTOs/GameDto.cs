using System;
using System.Collections.Generic;
using System.Linq;

namespace Gwent.NET.DTOs
{
    public class GameDto
    {
        public GameDto()
        {
            Players = new List<PlayerDto>();
        }

        public int Id { get; set; }
        public string State { get; set; }
        public List<PlayerDto> Players { get; set; }

        public GameDto StripPrivateInfo()
        {
            foreach (var playerDto in Players)
            {
                playerDto.HandCards.Clear();
            }
            return this;
        }

        public GameDto StripOpponentPrivateInfo(int userId)
        {
            foreach (var playerDto in Players)
            {
                if (playerDto.User != userId)
                {
                    playerDto.HandCards.Clear();
                }
            }
            return this;
        }
    }
}