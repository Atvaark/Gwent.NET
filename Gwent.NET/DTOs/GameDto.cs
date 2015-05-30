using System.Collections.Generic;

namespace Gwent.NET.DTOs
{
    public class GameDto
    {
        public GameDto()
        {
            Participants = new List<ParticipantDto>();
        }

        public int Id { get; set; }
        public string StateType { get; set; }
        public List<ParticipantDto> Participants { get; set; }
    }
}