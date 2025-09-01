
using Game.Combat;
using Game.Items;

namespace Game.Charakters;

public abstract class Entity
{
    public virtual string Name { get; set; }
    public virtual double BaseAttack { get; set; }
    public virtual double BaseDefence { get; set; }
    public virtual double BaseWisdom { get; set; }
    public virtual double CurrentHealth { get; set; }
    public virtual double ActualDamage { get; set; }
    public virtual bool IsLoadedFromFile { get; set; }
    public virtual double MaxHealth { get; set; }
    public virtual bool InDefensePosition { get; set; }
    public virtual Class Class { get; set; }
    public virtual List<Item> Inventory { get; set; }
    public virtual List<StatusEffekt> StatusEffekts { get; set; }
    public virtual MetaProgression MetaProgression { get; set; }

    public virtual Fight.ActionHistoryEntry Attack(Entity defender)
    {
        ActualDamage = GetAttackValue() - defender.GetDefenseValue();
        ActualDamage = defender.InDefensePosition ? ActualDamage / 2 : ActualDamage;
        ActualDamage = ActualDamage < 0 ? 0 : ActualDamage;
        defender.CurrentHealth -= ActualDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        defender.InDefensePosition = false;
        return new Fight.ActionHistoryEntry(Fight.PlayerActionEnum.Attack, this, [defender]);

    }
    public virtual Fight.ActionHistoryEntry SpecialAttack(Entity defender)
    {
        ActualDamage = (GetSpecialAttackValue() - defender.GetDefenseValue());
        ActualDamage = defender.InDefensePosition ? ActualDamage / 2 : ActualDamage;
        ActualDamage = ActualDamage < 0 ? 0 : ActualDamage;
        defender.CurrentHealth -= ActualDamage;
        defender.CurrentHealth = defender.CurrentHealth < 0 ? 0 : defender.CurrentHealth;
        defender.InDefensePosition = false;
        return new Fight.ActionHistoryEntry(Fight.PlayerActionEnum.SpecialAttack, this, [defender]);
    }
    public virtual Fight.ActionHistoryEntry GetInDefensePosition()
    {
        InDefensePosition = true;
        return new Fight.ActionHistoryEntry(Fight.PlayerActionEnum.Defend, this, []);
    }
    public virtual double GetAttackValue()
    {
        return (BaseAttack + MetaProgression.Attack + Class.AttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }
    public virtual double GetSpecialAttackValue()
    {
        return (BaseAttack + MetaProgression.Attack + Class.SpecialAttackModifier + CurrentHealth * 0.1) * GetWisdomValue();
    }
    public virtual double GetDefenseValue()
    {
        return (BaseDefence + MetaProgression.Defense + Class.DefenceModifier) * GetWisdomValue();
    }
    public virtual double GetMaxHealthValue()
    {
        return (MaxHealth + MetaProgression.Health + Class.HealthModifier) * GetWisdomValue();
    }
    public virtual double GetWisdomValue()
    {
        return (BaseWisdom + MetaProgression.Wisdom + Class.WisdomModifier);
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
                UpgradeBaseValues(MetaProgression, m => m.Attack, (m, v) => m.Attack = v, "Attack", 1);
                Console.ReadKey();
                break;
            case BaseValue.Defense:
                UpgradeBaseValues(MetaProgression, m => m.Defense, (m, v) => m.Defense = v, "Defense", 1);
                Console.ReadKey();
                break;
            case BaseValue.Wisdom:
                UpgradeBaseValues(MetaProgression, m => m.Wisdom, (m, v) => m.Wisdom = v, "Wisdom", 1);
                Console.ReadKey();
                break;
            case BaseValue.Health:
                UpgradeBaseValues(MetaProgression, m => m.Health, (m, v) => m.Health = v, "Health", 5);
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
    private void UpgradeBaseValues(MetaProgression meta, Func<MetaProgression, double> getter, Action<MetaProgression, double> setter, string label, int increment)
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