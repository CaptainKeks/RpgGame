using Game.Utilities;

namespace Game.Enteties;

public class UpgradedStatsFromShop
{
    public double BonusShopAttackStat { get; set; }
    public double BonusShopDefenseStat { get; set; }
    public double BonusShopWisdomStat { get; set; }
    public double BonusShopHealthStat { get; set; }
    public double BonusShopHealthPotionStat { get; set; }
    public double BonusShopPoisonPotionStat { get; set; }


    public UpgradedStatsFromShop(double attack, double defense, double wisdom, double health, double healthPotion, double poisonPotion)
    {
        BonusShopAttackStat = attack;
        BonusShopDefenseStat = defense;
        BonusShopWisdomStat = wisdom;
        BonusShopHealthStat = health;
        BonusShopHealthPotionStat = healthPotion;
        BonusShopPoisonPotionStat = poisonPotion;
    }

    public void UpgradeShopBonusStats(Shop.ShopValueUpgrade shopValueUpgrade)
    {
        switch (shopValueUpgrade.ValueToUpgrade)
        {
            case BaseValue.Attack:
                BonusShopAttackStat += shopValueUpgrade.value;
                break;
            case BaseValue.Defense:
                BonusShopDefenseStat += shopValueUpgrade.value;
                break;
            case BaseValue.Wisdom:
                BonusShopWisdomStat += shopValueUpgrade.value;
                break;
            case BaseValue.Health:
                BonusShopHealthStat += shopValueUpgrade.value;
                break;
            case BaseValue.HealthPotion:
                BonusShopHealthPotionStat += shopValueUpgrade.value;
                break;
            case BaseValue.PoisenPotion:
                BonusShopPoisonPotionStat += shopValueUpgrade.value;
                break;
        }
    }
}
