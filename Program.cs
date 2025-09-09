using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Menus;
using Game.Utilities;
using Game.Classes;

public class Programm
{
    public enum GameActionsEnum
    {
        Neuer_Run,
        Run_Laden,
        Upgrades,
        Zurück
    }

    public static void Main()
    {
        var gameSaves = SaveAndLoadJson.LoadGamesAndCreateFolder(out _, true);
        Menu startMenu = new LoadPlayerMenu(gameSaves, out GameSave gameSave);
        SelectNextMenuFromUserInput(gameSave);
    }

    public static void SelectNextMenuFromUserInput(GameSave gameSave)
    {
        Random rnd = new Random();
        bool validInput = false;
        var userInput = GetUserInputNumber();
        var action = (GameActionsEnum)userInput;
        switch (action)
        {
            case GameActionsEnum.Neuer_Run:
                // Neuen Run erstellen
                validInput = true;
                LoadPlayerMenu.ReloadUpgradedStats([gameSave]);
                gameSave.Player.CurrentHealth = gameSave.Player.MaxHealth;
                gameSave.Fight = new Fight(gameSave.Player, new EnemyGenerator(new Ork(), rnd.Next(1, 4), gameSave.Fight));
                Menu nextMenu = new FightMenu(gameSave);
                break;
            case GameActionsEnum.Run_Laden:
                // Spiel Laden
                LoadPlayerMenu.ReloadUpgradedStats([gameSave]);
                nextMenu = new FightMenu(gameSave);
                validInput = true;
                break;
            case GameActionsEnum.Upgrades:
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
            case GameActionsEnum.Zurück:
                SaveAndLoadJson.SaveGameAndWriteIDToGameSave(gameSave);
                var gameSaves = SaveAndLoadJson.LoadGamesAndCreateFolder(out _);
                Menu startMenu = new LoadPlayerMenu(gameSaves, out gameSave);
                SelectNextMenuFromUserInput(gameSave);
                validInput = true;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Falscher Input");
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
    }

    private static int GetUserInputNumber()
    {
        int result = default;
        Console.Write("> ");
        string? input = Console.ReadLine();
        while (!int.TryParse(input, out result))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Gib eine Gültige Zahl ein.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("> ");
            input = Console.ReadLine();
        }
        return result;
    }
}