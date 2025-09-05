using Game;
using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Menus;

class Programm
{
    public static void Main()
    {
        var gameSaves = SaveAndLoadJson.LoadGamesAndCreateFolder(out _, true);
        Menu startMenu = new LoadPlayerMenu(gameSaves, out GameSave gameSave);
        HandleInput(gameSave);
    }

    public static void HandleInput(GameSave gameSave)
    {
        Random rnd = new Random();
        string input;
        bool validInput = false;
        while (!validInput)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    // Neuen Run erstellen
                    validInput = true;
                    gameSave.Fight = new Fight(gameSave.Player, new EnemyGenerator(new Ork(), rnd.Next(1, 4), gameSave.Fight));
                    Menu nextMenu = new FightMenu(gameSave);
                    break;
                case "2":
                    // Spiel Laden
                    nextMenu = new FightMenu(gameSave);
                    validInput = true;
                    break;
                case "3":
                    // Upgrade durchführen
                    if (gameSave.Player != null)
                    {
                        nextMenu = new UpgradeMenu(gameSave);
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
                    SaveAndLoadJson.SaveGameAndWriteIDToGameSave(gameSave);
                    var gameSaves = SaveAndLoadJson.LoadGamesAndCreateFolder(out _);
                    Menu startMenu = new LoadPlayerMenu(gameSaves, out gameSave);
                    HandleInput(gameSave);
                    validInput = true;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}