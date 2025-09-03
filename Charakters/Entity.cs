
using Game.Combat;
using Game.Items;
using System.Numerics;

namespace Game.Charakters;

public abstract class Entity
{
    public virtual string Name { get; protected set; }
    public virtual double BaseAttack { get; protected set; }
    public virtual double BaseDefence { get; protected set; }
    public virtual double BaseWisdom { get; protected set; }
    public virtual double BaseHealth { get; protected set; }
    public virtual double CurrentHealth { get; set; }
    public virtual double ActualDamage { get; set; }
    public virtual double MaxHealth { get; protected set; }
    public virtual bool InDefensePosition { get; set; } = false;
    public virtual Class Class { get; set; }
    public virtual Inventory Inventory { get; set; } = new();
    public virtual List<StatusEffekt> StatusEffekts { get; set; } = [];
    public virtual ShopBonusStats MetaProgression { get; set; } = new(0, 0, 0, 0, 0, 0, 0, 20, 0, 0);


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

    public virtual Fight.ActionHistoryEntry Attack(Entity defender)
    {
        ActualDamage = GetAttackValue() - defender.GetDefenseValue();
        ActualDamage = defender.InDefensePosition ? ActualDamage / 2 : ActualDamage;
        ActualDamage = ActualDamage < 0 ? 0 : ActualDamage;
        defender.CurrentHealth -= ActualDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        defender.InDefensePosition = false;
        return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.Attack, null, this, [defender]);
    }

    public virtual Fight.ActionHistoryEntry SpecialAttack(Entity defender)
    {
        ActualDamage = (GetSpecialAttackValue() - defender.GetDefenseValue());
        ActualDamage = defender.InDefensePosition ? ActualDamage / 2 : ActualDamage;
        ActualDamage = ActualDamage < 0 ? 0 : ActualDamage;
        defender.CurrentHealth -= ActualDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        defender.InDefensePosition = false;
        return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.SpecialAttack, null, this, [defender]);
    }

    public virtual Fight.ActionHistoryEntry GetInDefensePosition()
    {
        InDefensePosition = true;
        return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.Defend, null, this, []);
    }

    public virtual double GetAttackValue()
    {
        return (BaseAttack + MetaProgression.ShopAttackUpgrade + Class.AttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }

    public virtual double GetSpecialAttackValue()
    {
        return (BaseAttack + MetaProgression.ShopAttackUpgrade + Class.SpecialAttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }

    public virtual double GetDefenseValue()
    {
        return (BaseDefence + MetaProgression.ShpoDefenseUpgrade + Class.DefenceModifier) * GetWisdomValue();
    }

    public virtual double GetMaxHealthValue()
    {
        return (BaseHealth + MetaProgression.BonusShopHealthStat + Class.HealthModifier) * GetWisdomValue();
    }

    public virtual double GetWisdomValue()
    {
        return (BaseWisdom + MetaProgression.BBonusShopWisdomStat + Class.WisdomModifier);
    }

    public virtual void UpgradeBaseValue(BaseValue baseValue)
    {
        if (MetaProgression.Gold < MetaProgression.Price)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Du hast nicht genügend Gold");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            return;
        }

        switch (baseValue)
        {
            case BaseValue.Attack:
                UpgradeBaseValues(MetaProgression, m => m.ShopAttackUpgrade, (m, v) => m.ShopAttackUpgrade = v, "Attack", 1);
                Console.ReadKey();
                break;
            case BaseValue.Defense:
                UpgradeBaseValues(MetaProgression, m => m.ShpoDefenseUpgrade, (m, v) => m.ShpoDefenseUpgrade = v, "Defense", 1);
                Console.ReadKey();
                break;
            case BaseValue.Wisdom:
                UpgradeBaseValues(MetaProgression, m => m.BBonusShopWisdomStat, (m, v) => m.BBonusShopWisdomStat = v, "Wisdom", 1);
                Console.ReadKey();
                break;
            case BaseValue.Health:
                UpgradeBaseValues(MetaProgression, m => m.BonusShopHealthStat, (m, v) => m.BonusShopHealthStat = v, "Health", 5);
                break;
            case BaseValue.HealthPotion:
                UpgradeBaseValues(MetaProgression, m => m.HealthPotion, (m, v) => m.HealthPotion = v, "HealthPotion", 5);
                break;
            case BaseValue.PoisenPotion:
                UpgradeBaseValues(MetaProgression, m => m.PoisonPotion, (m, v) => m.PoisonPotion = v, "PoisonPotion", 1);
                break;
            default:
                break;
        }
    }

    private void UpgradeBaseValues(ShopBonusStats meta, Func<ShopBonusStats, double> getter, Action<ShopBonusStats, double> setter, string label, int increment)
    {
        var current = getter(meta);
        setter(meta, current + increment);
        meta.Gold -= meta.Price;
        meta.Price += 15;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Deine {label} wurde auf {getter(meta)} erhöht.");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public virtual string GetShortInfo()
    {
        return $"{Name} {Class.ClassName} HP: {CurrentHealth:F2}/{MaxHealth:F2}";
    }
}