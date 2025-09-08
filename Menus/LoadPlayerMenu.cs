using Game.Combat;
using Game.Utilities;
using static Programm;
namespace Game.Menus;

public class LoadPlayerMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("RPG Kampf-Simulator:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.WriteLine("---------------------");
        Console.WriteLine("Wähle einen Spieler: ");
        Console.WriteLine("---------------------");
        Console.WriteLine();
    }

    public LoadPlayerMenu(List<GameSave> gameSaves, out GameSave gameSave)
    {
        PrintAvailableSaves(gameSaves);
        var input = GetUserInputNumber();
        DisplayCharakterOrStartMenu(input, gameSaves, out gameSave);
    }

    private List<LoadedSaveGameActionsEnum> GetLoadedSaveGameActions()
    {
        return Enum.GetValues<LoadedSaveGameActionsEnum>().Cast<LoadedSaveGameActionsEnum>().ToList();
    }

    private void DisplayCharakterOrStartMenu(int input, List<GameSave> gameSaves, out GameSave gameSave)
    {
        gameSave = new GameSave();
        if (input == 0)
        {
            Menu nextMenu = new CharakterMenu(gameSave);
        }
        else if (gameSaves.Count + 1 > input)
        {
            gameSave = gameSaves[input - 1];
            Menu nextMenu = new StartMenu(gameSaves[input - 1], GetLoadedSaveGameActions());
        }
        else if (gameSaves.Count + 1 == input)
        {
            Environment.Exit(0);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Falscher Input");
            Console.ForegroundColor = ConsoleColor.White;
            input = GetUserInputNumber();
            DisplayCharakterOrStartMenu(input, gameSaves, out gameSave);
        }
    }

    private void PrintAvailableSaves(List<GameSave> gameSaves)
    {
        Console.WriteLine($"[0] Neuen Spieler erstellen");
        for (int i = 0; i < gameSaves.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"[{i + 1}] ");
            Console.Write($"Spielername: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{gameSaves[i].Player.Name} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Gold: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{gameSaves[i].Player.Inventory.Gold} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Klasse: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{gameSaves[i].Player.Class.ClassName}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine($"[{gameSaves.Count + 1}] Beenden");
    }
}