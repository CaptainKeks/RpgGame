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

        public override void UseItem(Entity initiator, Entity target, out bool noItemUsed, out double healed)
        {
            // MetaProgression werte addieren
            var value = Value + target.ShopBonusStats.BonusShopHealthPotionStat;
            healed = target.MaxHealth - target.CurrentHealth > value ? value : target.MaxHealth - target.CurrentHealth;
            if (target.CurrentHealth != target.MaxHealth)
            {
                target.CurrentHealth += healed;
                noItemUsed = false;
            }
            else
            {
                noItemUsed = true;
            }
        }
    }
}