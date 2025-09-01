using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Menus;

class Programm
{
    public static void Main()
    {
        var player = SaveAndLoadJson.LoadGame(out _);
        Menu startMenu = new StartMenu(player);
        HandleInput(player);
    }

    public static void HandleInput(Entity player)
    {
        Player newPlayer = new Player();
        Fight fight = new Fight(new Player(), [new Enemy()]);
        newPlayer.MetaProgression = player.MetaProgression;
        newPlayer.CurrentHealth = newPlayer.MaxHealth;
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
                    Menu nextMenu = new CharakterMenu(newPlayer);
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
                    player.IsLoadedFromFile = true;
                    nextMenu = new FightMenu(player, fight);
                    validInput = true;
                    break;
                case "3":
                    // Upgrade durchführen
                    nextMenu = new UpgradeMenu(player);
                    validInput = true;
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