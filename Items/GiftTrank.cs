
using Game.Charakters;
using Game.Combat;
using Game.Enteties;

namespace Game.Items;

class GiftTrank : Item
{
    public GiftTrank(string name, double value, int? duration = 0) : base(name, value)
    {
        Duration = duration ?? Duration;
    }

    public override int Duration { get; set; } = 3;
    public override string Name { get; set; }
    public override string Description => $"Ein Grün Blubberndes Getränk das 5 Leben pro Runde schaden macht für {Duration} Runden.";
    public override double Value { get; set; } = 5;

    public override void UseItem(Entity initiator, Entity target, out bool noItemUsed, out double healed)
    {
        healed = 0;
        var value = Value + initiator.ShopBonusStats.BonusShopPoisonPotionStat;
        if (target.StatusEffekts.Count < 1)
        {
            noItemUsed = false;
            target.StatusEffekts.Add(new StatusEffekt("Gift", "Fügt jede Runde dem Gegner 5 Schaden zu kann gestäckt werden.", Duration, Value));
        }
        else
        {
            var query = target.StatusEffekts.Where(e => e.Name == "Gift");
            foreach (var effekt in query)
                effekt.Value += Value;
            noItemUsed = false;
        }
    }
}