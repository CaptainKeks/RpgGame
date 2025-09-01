using Game.Items;

namespace Game.Charakters;

public abstract class Class
{
    public abstract string ClassName { get; set; }
    public abstract double HealthModifier { get; set; }
    public abstract double WisdomModifier { get; set; }
    public abstract double AttackModifier { get; set; }
    public abstract double DefenceModifier { get; set; }
    public abstract double SpecialAttackModifier { get; set; }
    public abstract List<Item> Inventory { get; set; }
    public Class()
    {

    }
}
