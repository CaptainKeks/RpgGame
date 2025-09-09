using Game.Classes;
using Game.Items;
using Newtonsoft.Json;

namespace Game.Enteties;

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
    public Player(Class @class, string name) : base(@class)
    {
        if (@class is Mage)
            Inventory.AddItems([new HeilTrank("HeilTrank", GetHealthPotionValue()), new GiftTrank("GiftTrank", GetPoisonPotionValue(), 3)]);
        if (@class is Warrior)
            Inventory.AddItems([new HeilTrank("HeilTrank", GetHealthPotionValue())]);
        Name = name;
    }
    public Player() { }

    [JsonProperty]
    public override string Name { get; set; } = "Aria";
    [JsonProperty]
    public override double BaseAttack { get; protected set; } = 4;
    [JsonProperty]
    public override double BaseDefence { get; protected set; } = 3;
    [JsonProperty]
    public override double BaseWisdom { get; protected set; } = 0;
    [JsonProperty]
    public override double MaxHealth { get; set; } = 30;
    [JsonProperty]
    public override double BaseHealth { get; protected set; } = 30;
}