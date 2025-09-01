using Game.Charakters;
using Game.Combat;

namespace Game.Items
{
    class HeilTrank : Item
    {
        public HeilTrank(string name, int count, double value) : base(name, count, value) { }

        public override string Name { get; set; } = "Heiltrank";
        public override string Description { get; set; } = "Ein Rotes Blubberndes Getränk das 30 Leben wiederherstellt.";
        public override int Count { get; set; } = 3;
        public override double Value { get; set; } = 30;

        public override Fight.ActionHistoryEntry UseItem(Entity target, Entity enemy, out bool noItemUsed)
        {
            // MetaProgression werte addieren
            var value = Value + target.MetaProgression.HealthPotion;
            double healed = target.MaxHealth - target.CurrentHealth > value ? value : target.MaxHealth - target.CurrentHealth;
            if (Count > 0)
            {
                if (target.CurrentHealth != target.MaxHealth)
                {
                    target.CurrentHealth += healed;
                    Count--;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Heiltrank hat mich um {healed:F2} geheilt.");
                    Console.ForegroundColor = ConsoleColor.White;
                    noItemUsed = false;
                    Console.ReadKey();
                    return new Fight.ActionHistoryEntry(Fight.PlayerActionEnum.UseItem, target, [target], this);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Du kannst dich nicht Heilen du hast schon volles Leben!");
                    Console.ForegroundColor = ConsoleColor.White;
                    noItemUsed = true;
                    Console.ReadKey();
                    return new Fight.ActionHistoryEntry(Fight.PlayerActionEnum.UseItem, target, [target], this);
                }
            }
            else
            {
                noItemUsed = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Du kannst dich nicht Heilen du hast keine Heiltränke mehr!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }

            return new Fight.ActionHistoryEntry(Fight.PlayerActionEnum.UseItem, target, [target], this);
        }
    }
}