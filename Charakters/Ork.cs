
using Game.Items;

namespace Game.Charakters;

class Ork : Class
{
    public override string ClassName { get; set; } = "Ork";
    public override double HealthModifier { get; set; } = 6;
    public override double WisdomModifier { get; set; } = 1;
    public override double AttackModifier { get; set; } = 6;
    public override double DefenceModifier { get; set; } = 4;
    public override double SpecialAttackModifier { get; set; } = 10;
    public override List<Item> Inventory { get; set; } = [];
}
