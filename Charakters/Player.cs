using Game.Items;
using Newtonsoft.Json;

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

    public Player()
    {
        Inventory = new Inventory();
    }

    [JsonProperty]
    public override string Name { get; protected set; } = "Aria";
    [JsonProperty]
    public override double BaseAttack { get; protected set; } = 4;
    [JsonProperty]
    public override double BaseDefence { get; protected set; } = 3;
    [JsonProperty]
    public override double BaseWisdom { get; protected set; } = 0;
    [JsonProperty]
    public override double MaxHealth { get; protected set; } = 30;
    [JsonProperty]
    public override double BaseHealth { get; protected set; } = 30;

    /// <summary>
    /// Attackiert den mitgegeben Charakter und zieht die Defence von der Attacke ab.
    /// </summary>
    /// <param name="defender"></param>   
}