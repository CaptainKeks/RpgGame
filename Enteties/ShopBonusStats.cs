

using System.ComponentModel.DataAnnotations;
using Game.Utilities;

namespace Game.Enteties;

public class ShopBonusStats
{
    // Klasse stats in Player + Preise und effekte in shop definieren ABER wird an spieler übergeben
    // TODO rename
    public double BonusShopAttackStat { get; set; }
    public double BonusShopDefenseStat { get; set; }
    public double BBonusShopWisdomStat { get; set; }
    public double BonusShopHealthStat { get; set; }
    public double BonusShopHealthPotionStat { get; set; }
    public double BonusShopPoisonPotionStat { get; set; }

    // in shop pro upgrade

    public ShopBonusStats(double attack, double defense, double wisdom, double health, double healthPotion, double poisonPotion)
    {
        BonusShopAttackStat = attack;
        BonusShopDefenseStat = defense;
        BBonusShopWisdomStat = wisdom;
        BonusShopHealthStat = health;
        BonusShopHealthPotionStat = healthPotion;
        BonusShopPoisonPotionStat = poisonPotion;
    }

    public void UpgradeShopBonusStats(Shop.ShopValueUpgrade shopValueUpgrade)
    {
        
    }
}
