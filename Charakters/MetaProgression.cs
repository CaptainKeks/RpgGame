

namespace Game.Charakters;

public class MetaProgression
{
    public double Attack { get; set; }
    public double Defense { get; set; }
    public double Wisdom { get; set; }
    public double Health { get; set; }
    public double HealthPotion { get; set; }
    public double PoisonPotion { get; set; }
    public double Gold { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public double Price { get; set; }

    public MetaProgression(double attack, double defense, double wisdom, double health, double gold, int wins, int losses, double price, double healthPotion, double poisonPotion)
    {
        Attack = attack;
        Defense = defense;
        Wisdom = wisdom;
        Health = health;
        Gold = gold;
        Wins = wins;
        Losses = losses;
        Price = price;
        HealthPotion = healthPotion;
        PoisonPotion = poisonPotion;
    }
}
