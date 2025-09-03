

namespace Game.Charakters;

public class ShopBonusStats
{
    // Klasse stats in Player + Preise und effekte in shop definieren ABER wird an spieler übergeben
    // TODO rename
    public double ShopAttackUpgrade { get; set; }
    public double ShpoDefenseUpgrade { get; set; }
    public double BBonusShopWisdomStat { get; set; }
    public double BonusShopHealthStat { get; set; }

    // Upgrade in shop classe definieren
    // Übergeben an 
    public double HealthPotion { get; set; }
    public double PoisonPotion { get; set; }

    // Im Inventar, entweder als physisches Item GoldCoid: Item ... oder 
    // in Inventar als extra property
    public double Gold { get; set; }

    // in Player statistics
    public int Wins { get; set; }
    public int Losses { get; set; }

    // in shop pro upgrade
    public double Price { get; set; }

    public ShopBonusStats(double attack, double defense, double wisdom, double health, double gold, int wins, int losses, double price, double healthPotion, double poisonPotion)
    {
        ShopAttackUpgrade = attack;
        ShpoDefenseUpgrade = defense;
        BBonusShopWisdomStat = wisdom;
        BonusShopHealthStat = health;
        Gold = gold;
        Wins = wins;
        Losses = losses;
        Price = price;
        HealthPotion = healthPotion;
        PoisonPotion = poisonPotion;
    }
}
