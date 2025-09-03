using Game.Items;

namespace Game.Charakters;

public enum BaseValue
{
    Attack,
    Defense,
    Wisdom,
    Health,
    HealthPotion,
    PoisenPotion
}

public class Player : Entity
{
    public Player(Class @class) : base(@class)
    {
        Inventory.AddItems([new GiftTrank("GiftTrank", 5), new HeilTrank("HeilTrank", 30)]);
    }

    public override string Name { get; protected set; } = "Aria";
    public override double BaseAttack { get; protected set; } = 4;
    public override double BaseDefence { get; protected set; } = 3;
    public override double BaseWisdom { get; protected set; } = 0;
    public override double MaxHealth { get; set; } = 30;

    /// <summary>
    /// Attackiert den mitgegeben Charakter und zieht die Defence von der Attacke ab.
    /// </summary>
    /// <param name="defender"></param>   
}