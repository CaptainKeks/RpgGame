using Game.Utilities;
using static Programm;
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
    public StartMenu(GameSave gameSaves, List<LoadedSaveGameActionsEnum> actions)
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

        int i = 0;
        foreach (var action in actions)
        {
            Console.WriteLine($"[{i++}] {action.ToString().Replace('_', ' ')}");
        }
    }
}