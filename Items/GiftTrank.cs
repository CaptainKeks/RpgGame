
using Game.Charakters;
using Game.Combat;

namespace Game.Items;

class GiftTrank : Item
{
    public GiftTrank(string name, int count, double value, int? overrideDuration = null) : base(name, count, value)
    {
        Duration = overrideDuration ?? Duration;
    }

    public override int Duration { get; set; } = 3;
    public override string Name { get; set; }
    public override string Description => $"Ein Grün Blubberndes Getränk das 5 Leben pro Runde schaden macht für {Duration} Runden.";
    public override int Count { get; set; }
    public override double Value { get; set; } = 5;

    public override Fight.ActionHistoryEntry UseItem(Entity initiator, Entity target, out bool noItemUsed)
    {
        // MetaProgression Werte addieren
        var value = Value + initiator.MetaProgression.PoisonPotion;
        if (Count > 0)
        {
            if (target.StatusEffekts.Count < 1)
            {
                noItemUsed = false;
                target.StatusEffekts.Add(new StatusEffekt("Gift", "Fügt jede Runde dem Gegner 5 Schaden zu kann gestäckt werden.", Duration, Value));
                Count--;
            }
            else
            {
                var query = target.StatusEffekts.Where(e => e.Name == "Gift");
                foreach (var effekt in query)
                    effekt.Value += Value;
                noItemUsed = false;
                Count--;
            }
            return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.UseItem, null, initiator, [target], this);
        }
        else
        {
            initiator.Inventory.Remove(this);
            noItemUsed = true;
            return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.UseItem, null, initiator, [target], this, noItemUsed::noItemUsed);
        }
    }
}