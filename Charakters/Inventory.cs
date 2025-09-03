
using Game.Combat;
using Game.Items;

namespace Game.Charakters;

public class Inventory
{
    public Inventory() { }

    public List<ItemStack> Items { get; set; } = [];
    public record ViewItem(string Name, string Description, int Count, double Value, int? Duration = 0);

    public record ItemStack(Item Item, int Count)
    {
        public ViewItem GetViewItem()
        {
            return new ViewItem(Item.Name, Item.Description, Count, Item.Value);
        }
    };

    public void AddItem(Item item, int count = 1)
    {
        var stack = Items.FirstOrDefault(s => string.Equals(s.Item.Name, item.Name, StringComparison.OrdinalIgnoreCase));

        if (stack is not null)
        {
            var idx = Items.IndexOf(stack);
            Items[idx] = stack with { Count = stack.Count + count };
        }
        else
        {
            Items.Add(new ItemStack(item, count));
        }
    }

    public void AddItems(Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items is null || items.Length == 0) return;

            // Zusammenfassen, damit du nicht zig mal suchst
            foreach (var grp in items.GroupBy(i => i.Name, StringComparer.OrdinalIgnoreCase))
            {
                var first = grp.First();
                AddItem(first, grp.Count());
            }
        }
    }

    // returns: welche Items kannst du haben / verwenden als spieler ohne das Item "seblst" anwenden zu können
    // Items sollte nur über Inventar verwendet werden können

    public ViewItem[] GetInventoryContents()
    {
        return Items.Select(item => item.GetViewItem()).ToArray();
    }

    public Fight.ActionHistoryEntry UseItem(ViewItem viewItem, Entity player, Entity enemy)
    {
        bool noItemUsed = false;
        Fight.ActionHistoryEntry? historyEntry = null;
        foreach (var item in Items)
        {
            if (item.Item.Name == viewItem.Name && item.Count > 0)
            {
                noItemUsed = false;
                historyEntry = item.Item.UseItem(player, enemy, out noItemUsed);
            }
            else
            {
                noItemUsed = true;
                return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.UseItem, null, player, [enemy], item.Item, noItemUsed: noItemUsed);
            }
        }
        return historyEntry;

        // finde dein Item im inventar 
        // aktuell noch nach Name später vielleicht ID
        // Item zu VIewItem finden in Liste

        // prüfen ob es vorhanden ist
        // Item.Count > 0 sonst Fehler / meldung ans Spieler

        // rufe useItem auf Item auf (weiterleiten) History erhalten und nach oben reichen

        // if consumable -> Count - 1 und löschenb falls <= 0
    }

    // zu jedem Item Typ anders 
    // z.B Duration gibt es nicht überall
}