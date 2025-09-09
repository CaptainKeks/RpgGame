using Game.Classes;
using Game.Items;
using Newtonsoft.Json;

namespace Game.Enteties;

public class Enemy : Entity
{
    public Enemy(Class @class, int level = 1) : base(@class)
    {
        Random rnd = new Random();
        MaxHealth = GetMaxHealthValue() - 8 + 4 * level;
        CurrentHealth = MaxHealth;
        Class.AttackModifier = rnd.Next((int)(Class.AttackModifier + level * 1.5 - 2), (int)(Class.AttackModifier + level * 1.5 + 2));
        Inventory.AddItem(new GiftTrank("GiftTrank", 5, 2));
    }

    [JsonProperty]
    public override string Name { get; set; } = "Orga";
    [JsonProperty]
    public override double BaseAttack { get; protected set; } = 4;
    [JsonProperty]
    public override double BaseDefence { get; protected set; } = 3;
    [JsonProperty]
    public override double BaseWisdom { get; protected set; } = 0;
    [JsonProperty]
    public override double MaxHealth { get; set; } = 25;
    [JsonProperty]
    public override double BaseHealth { get; protected set; } = 25;

    /// <summary>
    /// Attackiert den mitgegeben Charakter und zieht die Defence von der Attacke ab.
    /// </summary>
    /// <param name="defender"></param>   
}