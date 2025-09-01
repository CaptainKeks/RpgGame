

using Game.Items;

namespace Game.Charakters;

class Warrior : Class
{
    public override string ClassName { get; set; } = "Warrior";
    public override double HealthModifier { get; set; } = 16;
    public override double WisdomModifier { get; set; } = 1;
    public override double AttackModifier { get; set; } = 3;
    public override double DefenceModifier { get; set; } = 6;
    public override double SpecialAttackModifier { get; set; } = 10;
    public override List<Item> Inventory { get; set; } = [];
}
