using Game;
using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Items;
using Game.Menus;
using Game.Utilities;

class Programm
{
    public static void Main()
    {
        var (player, fight) = SaveAndLoadJson.LoadGame(out _, true);
        Menu startMenu = new StartMenu(player);
        HandleInput(player, fight);
    }

    public static void HandleInput(Player? player = null, Fight? fight = null)
    {
        List<Entity> enemies;
        string input;
        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            bool validInput = false;
            switch (input)
            {
                case "1":
                    // Neues Spiel erstellen
                    validInput = true;
                    Menu nextMenu = new CharakterMenu();
                    break;
                case "2":
                    // Spiel Laden
                    (player, fight) = SaveAndLoadJson.LoadGame(out bool succeeded);
                    if (!succeeded)
                    {
                        break;
                    }
                    nextMenu = new FightMenu(player, fight);
                    validInput = true;
                    break;
                case "3":
                    // Upgrade durchführen
                    if (player != null)
                    {
                        SaveAndLoadJson.LoadGame(out _);
                        nextMenu = new UpgradeMenu(player);
                        validInput = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Es ist kein Spieler geladen erstelle bitte zuerst ein neues Spiel.");
                        Console.ForegroundColor = ConsoleColor.White;
                        validInput = false;
                    }
                    break;
                case "4":
                    SaveAndLoadJson.SaveGame(new GameSaves(player, new Fight(), Shop.Instance));
                    Environment.Exit(0);
                    validInput = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            if (validInput)
                break;
        }
    }
}