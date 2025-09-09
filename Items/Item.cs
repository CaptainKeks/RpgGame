using Game.Enteties;

namespace Game.Items;

public abstract class Item
{
    public abstract string Name { get; set; }
    public abstract string Description { get; }
    public abstract double Value { get; set; }
    public abstract int Duration { get; set; }

    public abstract void UseItem(Entity player, Entity enemy, out bool noItemUsed, out double healed);

    public Item(string name, double value)
    {
        Name = name;
        Value = value;
    }
}