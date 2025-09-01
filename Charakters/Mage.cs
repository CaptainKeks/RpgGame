

using Game.Items;

namespace Game.Charakters;

class Mage : Class
{
    public override string ClassName { get; set; } = "Mage";
    public override double HealthModifier { get; set; } = 13;
    public override double WisdomModifier { get; set; } = 1.2;
    public override double AttackModifier { get; set; } = 7;
    public override double DefenceModifier { get; set; } = 2;
    public override double SpecialAttackModifier { get; set; } = 12;
    public override List<Item> Inventory { get; set; } = [new HeilTrank("Heiltrank", 3, 30),
                                                          new GiftTrank("GiftTrank", 2, 3, 5)];
}
