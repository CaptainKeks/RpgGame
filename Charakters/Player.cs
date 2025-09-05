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
        Inventory.AddItems([new HeilTrank("HeilTrank", 30), new GiftTrank("GiftTrank", 5, 3)]);
    }
    public Player()
    {
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
}