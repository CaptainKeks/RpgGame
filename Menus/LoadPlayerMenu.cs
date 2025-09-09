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
        ReloadUpgradedStats(gameSaves);
        PrintAvailableSaves(gameSaves);
        var input = GetUserInputNumber();
        DisplayCharakterOrStartMenu(input, gameSaves, out gameSave);
    }

    private List<GameActionsEnum> GetGameActions()
    {
        return Enum.GetValues<GameActionsEnum>().Cast<GameActionsEnum>().ToList();
    }

    private void DisplayCharakterOrStartMenu(int input, List<GameSave> gameSaves, out GameSave gameSave)
    {
        gameSave = new GameSave();
        if (input == 0)
        {
            Menu nextMenu = new SelectNameMenu(out string userInputName);
            gameSave.Player.Name = userInputName;
            gameSave.Fight.Player.Name = userInputName;
            nextMenu = new CharakterMenu(gameSave);
        }
        else if (gameSaves.Count + 1 > input)
        {
            gameSave = gameSaves[input - 1];
            Menu nextMenu = new StartMenu(gameSaves[input - 1], GetGameActions());
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
        var sortetList = gameSaves.OrderBy(i => i.Player.Inventory.Gold).ToList();
        Console.WriteLine($"[0] Neuen Spieler erstellen");
        for (int i = 0; i < sortetList.Count; i++)
        {
            var player = sortetList[i].Player;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"[{i + 1}] ");
            Console.Write("Spielername: ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{player.Name,-12}");   // -15 = linksbündig, 15 Zeichen breit

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" Gold: ");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{player.Inventory.Gold,-6}");  // rechtsbündig, 6 Stellen

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" Klasse: ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{player.Class.ClassName,-9}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" Leben: ");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{player.CurrentHealth:F2}/{player.MaxHealth,-6:F2}");

            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine($"[{sortetList.Count + 1}] Beenden");
    }

    /// <summary>
    /// Setzt die Werte die im Shop verbessert wurden auf den Player.
    /// </summary>
    /// <param name="gameSave"></param>
    public static void ReloadUpgradedStats(List<GameSave> gameSaves)
    {
        foreach (var gameSave in gameSaves)
        {
            gameSave.Player.MaxHealth = gameSave.Player.GetMaxHealthValue();
            var heilTrank = gameSave.Player.Inventory.Items.Any(i => i.item.Name == "HeilTrank" && i.Count > 0);
            var giftTrank = gameSave.Player.Inventory.Items.Any(i => i.item.Name == "GiftTrank" && i.Count > 0);

            if (heilTrank)
                gameSave.Player.Inventory.Items.Where(i => i.item.Name == "HeilTrank").Single().Item.Value = gameSave.Player.GetHealthPotionValue();
            if (giftTrank)
                gameSave.Player.Inventory.Items.Where(i => i.item.Name == "GiftTrank").Single().Item.Value = gameSave.Player.GetPoisonPotionValue();
        }
    }
}