namespace Gwent.NET.DTOs
{
    public enum CommandType
    {
        None = 0,
        StartGame = 1,
        PickStartingPlayer = 2,
        RedrawCard = 3,
        EndRedrawCard = 4,
        ForfeitGame = 5,
        Pass = 6,
        PlayCard = 7,
        UseBattleKingCard = 8
    }
}