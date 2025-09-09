using Game.Enteties;
namespace Game.Classes;

public abstract class Class
{
    public abstract string ClassName { get; set; }
    public abstract double HealthModifier { get; set; }
    public abstract double WisdomModifier { get; set; }
    public abstract double AttackModifier { get; set; }
    public abstract double DefenceModifier { get; set; }
    public abstract double SpecialAttackModifier { get; set; }
    public abstract Inventory BaseInventory { get; set; } // BaseInventory
    public Class() { }
}