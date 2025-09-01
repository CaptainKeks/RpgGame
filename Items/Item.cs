using Game.Charakters;
using Game.Combat;

namespace Game.Items;

public abstract class Item
{
    public abstract string Name { get; set; }
    public abstract string Description { get; }
    public abstract int Count { get; set; }
    public abstract double Value { get; set; }
    public abstract int Duration { get; set; }

    public abstract Fight.ActionHistoryEntry UseItem(Entity player, Entity enemy, out bool noItemUsed);

    public Item(string name, int count, double value)
    {
        Name = name;
        Count = count;
        Value = value;
    }
}