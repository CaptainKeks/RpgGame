
using Game.Combat;

namespace Game.Charakters;

public class StatusEffekt
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public double Value { get; set; }

    public StatusEffekt(string name, string description, int duration, double value)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Value = value;
    }

    public Fight.ActionHistoryEntry ApplyStatusAffect(Entity target)
    {
        double damage = 0;
        if (Duration < 1)
            RemoveElapsedStatusEffect(target);

        damage = target.CurrentHealth > Value ? Value : target.CurrentHealth;
        target.CurrentHealth -= damage;
        Duration--;
        return new Fight.ActionHistoryEntry(null, Fight.PassiveActionEnum.ApplyStatusEffect, null, [target], Duration: Duration, Value: Value);
    }

    public void RemoveElapsedStatusEffect(Entity target)
    {
        target.StatusEffekts.Remove(this);
    }

    public override string ToString()
    {
        return Name + " (" + Duration + ")" + " mit " + Value + " Schaden.";
    }
}