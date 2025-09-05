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
    public StartMenu(GameSave gameSaves)
    {
        Console.WriteLine("---------------------");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {gameSaves.Player.Inventory.Gold}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"Wins: {gameSaves.Player.Stats.Wins}");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($" Losses: {gameSaves.Player.Stats.Losses}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("---------------------");
        Console.WriteLine();
        Console.WriteLine("[1] Neuer Run");
        Console.WriteLine("[2] Spiel Laden");
        Console.WriteLine("[3] Upgrades");
        Console.WriteLine("[4] Zurück");
    }
}