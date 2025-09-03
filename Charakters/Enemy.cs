using Game.Combat;
using Game.Items;
using System;

namespace Game.Charakters;

public class Enemy : Entity
{
    public Enemy(Class @class, Fight fight) : base(@class)
    {
        Random rnd = new Random();
        MaxHealth = (GetMaxHealthValue() - 8) + (4 * fight.Level);
        CurrentHealth = MaxHealth;
        Class.AttackModifier = rnd.Next((int)(Class.AttackModifier + (fight.Level * 1.5) - 2), (int)(Class.AttackModifier + (fight.Level * 1.5) + 2));
        Inventory.AddItem(new GiftTrank("GiftTrank", 5));
    }

    public override string Name { get; protected set; } = "Orga";
    public override double BaseAttack { get; protected set; } = 4;
    public override double BaseDefence { get; protected set; } = 3;
    public override double BaseWisdom { get; protected set; } = 0;
    public override double MaxHealth { get; protected set; } = 25;
    public override double BaseHealth { get; protected set; } = 25;

    /// <summary>
    /// Attackiert den mitgegeben Charakter und zieht die Defence von der Attacke ab.
    /// </summary>
    /// <param name="defender"></param>   
}