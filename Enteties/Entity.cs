using Game.Classes;
using Game.Combat;
using Game.Utilities;
using Newtonsoft.Json;

namespace Game.Enteties;

public abstract class Entity
{
    [JsonProperty]
    public virtual string Name { get; set; }
    [JsonProperty]
    public virtual double BaseAttack { get; protected set; }
    [JsonProperty]
    public virtual double BaseDefence { get; protected set; }
    [JsonProperty]
    public virtual double BaseWisdom { get; protected set; }
    [JsonProperty]
    public virtual double BaseHealth { get; protected set; }
    [JsonProperty]
    public virtual double BaseHealthPotionValue { get; protected set; } = 30;
    [JsonProperty]
    public virtual double BasePoisonPotionValue { get; protected set; } = 5;
    [JsonProperty]
    public virtual double CurrentHealth { get; set; }
    [JsonProperty]
    public virtual double ActualDamage { get; set; }
    [JsonProperty]
    public virtual double MaxHealth { get; set; }
    [JsonProperty]
    public virtual bool InDefensePosition { get; set; } = false;
    [JsonProperty]
    public virtual Class Class { get; set; }
    [JsonProperty]
    public virtual Inventory Inventory { get; protected set; } = new();
    [JsonProperty]
    public virtual List<StatusEffekt> StatusEffekts { get; set; } = [];
    [JsonProperty]
    public virtual UpgradedStatsFromShop ShopBonusStats { get; protected set; } = new(0, 0, 0, 0, 0, 0);
    [JsonProperty]
    public virtual Stats Stats { get; protected set; } = new(0, 0);

    public Entity(Class @class)
    {
        Class = @class;
        Inventory = @class.BaseInventory;
        MaxHealth = GetMaxHealthValue();
        CurrentHealth = MaxHealth;
    }

    public Entity() { }


    public virtual ActionHistoryEntry Attack(Entity defender)
    {
        ActualDamage = GetAttackValue() - defender.GetDefenseValue();
        ActualDamage = defender.InDefensePosition ? ActualDamage / 2 : ActualDamage;
        ActualDamage = ActualDamage < 0 ? 0 : ActualDamage;
        defender.CurrentHealth -= ActualDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        defender.InDefensePosition = false;
        return new ActionHistoryEntry(ActivePlayerActionEnum.Attack, this, [defender]);
    }

    public virtual ActionHistoryEntry SpecialAttack(Entity defender)
    {
        ActualDamage = GetSpecialAttackValue() - defender.GetDefenseValue();
        ActualDamage = defender.InDefensePosition ? ActualDamage / 2 : ActualDamage;
        ActualDamage = ActualDamage < 0 ? 0 : ActualDamage;
        defender.CurrentHealth -= ActualDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        defender.InDefensePosition = false;
        return new ActionHistoryEntry(ActivePlayerActionEnum.SpecialAttack, this, [defender]);
    }

    public virtual ActionHistoryEntry GetInDefensePosition()
    {
        InDefensePosition = true;
        return new ActionHistoryEntry(ActivePlayerActionEnum.Defend, this, []);
    }

    public virtual double GetAttackValue()
    {
        return (BaseAttack + ShopBonusStats.BonusShopAttackStat + Class.AttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }

    public virtual double GetSpecialAttackValue()
    {
        return (BaseAttack + ShopBonusStats.BonusShopAttackStat + Class.SpecialAttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }

    public virtual double GetDefenseValue()
    {
        return (BaseDefence + ShopBonusStats.BonusShopDefenseStat + Class.DefenceModifier) * GetWisdomValue();
    }

    public virtual double GetMaxHealthValue()
    {
        return (BaseHealth + ShopBonusStats.BonusShopHealthStat + Class.HealthModifier) * GetWisdomValue();
    }

    public virtual double GetWisdomValue()
    {
        return BaseWisdom + ShopBonusStats.BonusShopWisdomStat + Class.WisdomModifier;
    }

    public virtual double GetHealthPotionValue()
    {
        return BaseHealthPotionValue + ShopBonusStats.BonusShopHealthPotionStat;
    }
    public virtual double GetPoisonPotionValue()
    {
        return BasePoisonPotionValue + ShopBonusStats.BonusShopPoisonPotionStat;
    }


    public virtual void UpgradeBaseValue(BaseValue baseValue)
    {
        if (Inventory.Gold < Shop.Instance.Prices[baseValue])
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Du hast nicht genügend Gold");
            Console.WriteLine("Drücke [Enter]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            return;
        }

        switch (baseValue)
        {
            case BaseValue.Attack:
                ShopBonusStats.UpgradeShopBonusStats(new Shop.ShopValueUpgrade(baseValue, 1));
                ChangePrice(baseValue);
                break;
            case BaseValue.Defense:
                ShopBonusStats.UpgradeShopBonusStats(new Shop.ShopValueUpgrade(baseValue, 1));
                ChangePrice(baseValue);
                break;

            case BaseValue.Wisdom:
                ShopBonusStats.UpgradeShopBonusStats(new Shop.ShopValueUpgrade(baseValue, 1));
                ChangePrice(baseValue);
                break;

            case BaseValue.Health:
                ShopBonusStats.UpgradeShopBonusStats(new Shop.ShopValueUpgrade(baseValue, 5));
                ChangePrice(baseValue);
                break;

            case BaseValue.HealthPotion:
                ShopBonusStats.UpgradeShopBonusStats(new Shop.ShopValueUpgrade(baseValue, 5));
                ChangePrice(baseValue);
                break;

            case BaseValue.PoisenPotion:
                ShopBonusStats.UpgradeShopBonusStats(new Shop.ShopValueUpgrade(baseValue, 1));
                ChangePrice(baseValue);
                break;

            default:
                break;
        }
    }

    private void ChangePrice(BaseValue baseValue)
    {
        Inventory.RemoveGold(Shop.Instance.Prices[baseValue], out _);
        Inventory.HigherPrice(baseValue, 15);
    }

    public virtual string GetShortInfo()
    {
        return $"{Name} {Class.ClassName} HP: {CurrentHealth:F2}/{MaxHealth:F2}";
    }
}