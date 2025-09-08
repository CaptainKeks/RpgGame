using Game.Classes;
using Game.Combat;
using Game.Utilities;
using Newtonsoft.Json;

namespace Game.Enteties;

public abstract class Entity
{
    [JsonProperty]
    public virtual string Name { get; protected set; }
    [JsonProperty]
    public virtual double BaseAttack { get; protected set; }
    [JsonProperty]
    public virtual double BaseDefence { get; protected set; }
    [JsonProperty]
    public virtual double BaseWisdom { get; protected set; }
    [JsonProperty]
    public virtual double BaseHealth { get; protected set; }
    [JsonProperty]
    public virtual double CurrentHealth { get; set; }
    [JsonProperty]
    public virtual double ActualDamage { get; set; }
    [JsonProperty]
    public virtual double MaxHealth { get; protected set; }
    [JsonProperty]
    public virtual bool InDefensePosition { get; set; } = false;
    [JsonProperty]
    public virtual Class Class { get; protected set; }
    [JsonProperty]
    public virtual Inventory Inventory { get; protected set; } = new();
    [JsonProperty]
    public virtual List<StatusEffekt> StatusEffekts { get; set; } = [];
    [JsonProperty]
    public virtual ShopBonusStats ShopBonusStats { get; protected set; } = new(0, 0, 0, 0, 0, 0);
    [JsonProperty]
    public virtual Stats Stats { get; protected set; } = new(0, 0);


    // Eine Klasse erstellen für GameSaves wo alle gespeicherten werte drin sind 
    // die klasse hat methoden die die werte in Player enemie usw in den konstruktor übergeben und diese dann setzen
    // geänderte werte die sich nicht berechenn lassen muss ich synchronisieren

    public Entity(Class @class)
    {
        Class = @class;
        Inventory = @class.BaseInventory;
        MaxHealth = GetMaxHealthValue();
        CurrentHealth = MaxHealth;
    }

    public Entity() { }

    public void ApplyShopBonusStats(ShopBonusStats shopBonusStats)
    {
        ShopBonusStats = shopBonusStats;
    }


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
        return BaseWisdom + ShopBonusStats.BBonusShopWisdomStat + Class.WisdomModifier;
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
                UpgradeBaseValues(ShopBonusStats, m => m.BonusShopAttackStat, (m, v) => m.BonusShopAttackStat = v, "Attack", 1, BaseValue.Attack);
                Console.ReadKey();
                break;
            case BaseValue.Defense:
                UpgradeBaseValues(ShopBonusStats, m => m.BonusShopDefenseStat, (m, v) => m.BonusShopDefenseStat = v, "Defense", 1, BaseValue.Defense);
                Console.ReadKey();
                break;
            case BaseValue.Wisdom:
                UpgradeBaseValues(ShopBonusStats, m => m.BBonusShopWisdomStat, (m, v) => m.BBonusShopWisdomStat = v, "Wisdom", 1, BaseValue.Wisdom);
                Console.ReadKey();
                break;
            case BaseValue.Health:
                UpgradeBaseValues(ShopBonusStats, m => m.BonusShopHealthStat, (m, v) => m.BonusShopHealthStat = v, "Health", 5, BaseValue.Health);
                break;
            case BaseValue.HealthPotion:
                UpgradeBaseValues(ShopBonusStats, m => m.BonusShopHealthPotionStat, (m, v) => m.BonusShopHealthPotionStat = v, "HealthPotion", 5, BaseValue.HealthPotion);
                break;
            case BaseValue.PoisenPotion:
                UpgradeBaseValues(ShopBonusStats, m => m.BonusShopPoisonPotionStat, (m, v) => m.BonusShopPoisonPotionStat = v, "PoisonPotion", 1, BaseValue.PoisenPotion);
                break;
            default:
                break;
        }
    }

    private void UpgradeBaseValues(ShopBonusStats bonusStats, Func<ShopBonusStats, double> getter, Action<ShopBonusStats, double> setter, string label, int increment, BaseValue baseValue)
    {
        var current = getter(bonusStats);
        setter(bonusStats, current + increment);
        Inventory.RemoveGold(Shop.Instance.Prices[baseValue], out bool failed);
        Inventory.HigherPrice(baseValue, 15);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Deine {label} wurde auf {getter(bonusStats)} erhöht.");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public virtual string GetShortInfo()
    {
        return $"{Name} {Class.ClassName} HP: {CurrentHealth:F2}/{MaxHealth:F2}";
    }
}