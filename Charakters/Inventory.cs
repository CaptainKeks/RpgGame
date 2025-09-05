
using Game.Combat;
using Game.Items;
using Game.Utilities;
using Newtonsoft.Json;

namespace Game.Charakters;

public class Inventory
{
    public List<ItemStack> Items { get; private set; } = [];
    [JsonProperty]
    public int Gold { get; private set; } = 0; // TODO vielleicht später mal ulong falls 2 millarden nicht reichen xD

    public Inventory() { }


    public void AddLoot(Enemy enemy)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Addiert Gold zum Inventar Hinzu.
    /// </summary>
    /// <param name="amount"></param>
    public void AddGold(int amount)
    {
        Gold += amount;
    }

    /// <summary>
    /// Entfernt Gold falls Genug Gold vorhanden ist.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="failed"></param>
    public void RemoveGold(int amount, out bool failed)
    {
        failed = false;
        if (Gold < amount)
        {
            failed = true;
            return;
        }
        Gold -= amount;
    }

    /// <summary>
    /// Erhöht den Preis der jeweiligen BaseValue.
    /// </summary>
    /// <param name="baseValue"></param>
    /// <param name="amount"></param>
    public void HigherPrice(BaseValue baseValue, int amount)
    {
        Shop.Instance.Prices[baseValue] += amount;
    }


    public void AddItem(Item item, int count = 1)
    {
        var stack = Items.FirstOrDefault(s => string.Equals(s.Item.Name, item.Name, StringComparison.OrdinalIgnoreCase));

        if (stack is not null)
        {
            var idx = Items.IndexOf(stack);
            stack.SetCount(stack.Count + count);
            Items[idx] = stack;
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

    public ActionHistoryEntry UseItem(ViewItem viewItem, Entity initiator, Entity target)
    {
        bool noItemUsed = false;
        ActionHistoryEntry? historyEntry = null;
        foreach (var itemStack in Items)
        {
            if (itemStack.Item.Name == viewItem.Name && itemStack.Count > 0)
            {
                itemStack.Item.UseItem(initiator, target, out noItemUsed);
                itemStack.SubtractCount(1);
                if (itemStack.Count < 1)
                    initiator.Inventory.Items.Remove(itemStack);
                noItemUsed = false;
                return new ActionHistoryEntry(ActivePlayerActionEnum.UseItem, initiator, [target], viewItem, noItemUsed: noItemUsed);
            }
            else
            {
                noItemUsed = true;
                historyEntry = new ActionHistoryEntry(ActivePlayerActionEnum.UseItem, initiator, [target], viewItem, noItemUsed: noItemUsed);
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


    // Inventory Models
    public record ViewItem(string Name, string Description, int Count, double Value, int? Duration = 0);

    public record ItemStack(Item item, int Count)
    {
        public Item Item { get; private set; } = item;
        public int Count { get; private set; } = Count;
        public ViewItem GetViewItem()
        {
            return new ViewItem(Item.Name, Item.Description, Count, Item.Value, Item.Duration);
        }

        public void SubtractCount(int amount)
        {
            Count -= amount;
        }
        public void SetCount(int amount)
        {
            Count = amount;
        }

    };
}