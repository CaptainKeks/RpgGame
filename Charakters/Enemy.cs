using Game.Items;

namespace Game.Charakters;

public class Enemy : Entity
{
    public Enemy() { }
    public override string Name { get; set; }
    public override double BaseAttack { get; set; } = 4;
    public override double BaseDefence { get; set; } = 3;
    public override double BaseWisdom { get; set; } = 0;
    public override double MaxHealth { get; set; } = 25;
    public override double CurrentHealth { get; set; }
    public override bool InDefensePosition { get; set; } = false;
    public override Class Class { get; set; }
    public override bool IsLoadedFromFile { get; set; } = false;
    public override List<Item> Inventory { get; set; } = [];
    public override List<StatusEffekt> StatusEffekts { get; set; } = [];
    public override MetaProgression MetaProgression { get; set; } = new MetaProgression(attack: 0, defense: 0, wisdom: 0, health: 0, healthPotion: 0, poisonPotion: 0, gold: 0, wins: 0, losses: 0, price: 20);

    /// <summary>
    /// Attackiert den mitgegeben Charakter und zieht die Defence von der Attacke ab.
    /// </summary>
    /// <param name="defender"></param>
   
}
