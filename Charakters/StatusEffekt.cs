
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

    public void ApplyStatusAffect(Entity target)
    {
        double damage = 0;
        if (Duration < 1)
            return;
        damage = target.CurrentHealth > Value ? Value : target.CurrentHealth;
        target.CurrentHealth -= damage;
        Duration--;
        // message+= ($"Ich {attacker.Name} habe {defender.Name} ");
        // Console.ForegroundColor = ConsoleColor.Green;
        // Console.Write($"{damage}");
        // Console.ForegroundColor = ConsoleColor.White;
        // Console.WriteLine($" Giftschaden hinzugefügt. ({Duration})");
        // Console.WriteLine();
    }

    public void RemoveElapsedStatusEffect(Player charakter)
    {
        if (Duration < 1)
            charakter.StatusEffekts.Remove(this);
    }

    public override string ToString()
    {
        return Name + " (" + Duration + ")" + " mit " + Value + " Schaden.";
    }
}