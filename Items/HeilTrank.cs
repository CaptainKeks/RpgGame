using Game.Charakters;
using Game.Combat;

namespace Game.Items
{
    class HeilTrank : Item
    {
        public HeilTrank(string name, double value) : base(name, value) { }

        public override string Name { get; set; } = "Heiltrank";
        public override string Description => "Ein Rotes Blubberndes Getränk das 30 Leben wiederherstellt.";
        public override double Value { get; set; } = 30;
        public override int Duration { get; set; }

        public override Fight.ActionHistoryEntry UseItem(Entity target, Entity enemy, out bool noItemUsed)
        {
            // MetaProgression werte addieren
            var value = Value + target.MetaProgression.HealthPotion;
            double healed = target.MaxHealth - target.CurrentHealth > value ? value : target.MaxHealth - target.CurrentHealth;
            if (target.CurrentHealth != target.MaxHealth)
            {
                target.CurrentHealth += healed;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Heiltrank hat mich um {healed:F2} geheilt.");
                Console.ForegroundColor = ConsoleColor.White;
                noItemUsed = false;
                return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.UseItem, null, target, [target], this);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Du kannst dich nicht Heilen du hast schon volles Leben!");
                Console.ForegroundColor = ConsoleColor.White;
                noItemUsed = true;
                return new Fight.ActionHistoryEntry(Fight.ActivePlayerActionEnum.UseItem, null, target, [target], this);
            }
        }
    }
}