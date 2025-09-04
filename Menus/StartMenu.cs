using Game.Charakters;
namespace Game.Menus;

class StartMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("RPG Kampf-Simulator:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------------");
        Console.WriteLine();
    }
    public StartMenu(Entity player)
    {
        Console.WriteLine("---------------------");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {player.Inventory.Gold}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"Wins: {player.Stats.Wins}");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($" Losses: {player.Stats.Losses}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("---------------------");
        Console.WriteLine();
        Console.WriteLine("[1] Neues Spiel");
        Console.WriteLine("[2] Spiel Laden");
        Console.WriteLine("[3] Upgrades");
        Console.WriteLine("[4] Beenden");
    }
}