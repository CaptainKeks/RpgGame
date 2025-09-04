namespace Game.Charakters;

public class Stats
{
    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;

    public Stats(int wins, int losses)
    {
        Wins = wins;
        Losses = losses;
    }
}