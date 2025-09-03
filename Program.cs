using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Items;
using Game.Menus;

class Programm
{
    public static void Main()
    {
        var player = SaveAndLoadJson.LoadGame(out _);
        if (player == null)
            player = new Player(new Mage());
        Menu startMenu = new StartMenu(player);
        HandleInput(player);
    }

    public static void HandleInput(Player? player = null)
    {
        Fight fight = new Fight();
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
                    player = SaveAndLoadJson.LoadGame(out bool succeeded);
                    if (!succeeded)
                    {
                        Console.ReadKey();
                        break;
                    }
                    fight = SaveAndLoadJson.LoadFight();
                    nextMenu = new FightMenu(player, fight);
                    validInput = true;
                    break;
                case "3":
                    // Upgrade durchführen
                    if (player != null)
                    {
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
                    SaveAndLoadJson.SaveGame(player);
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