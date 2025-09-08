using Game.Charakters;
using Game.Combat;
using Game.Utilities;
using Game.Enteties;
using Game.Classes;

namespace Game.Menus;

class CharakterMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Charakter Erstellen:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.WriteLine("Name: Aria");
        Console.WriteLine("Klasse Wählen:");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("[0] Warrior  HP: Hoch, Blocken, Waffe: Schwert");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("[1] Mage     HP: Mittel, Waffe: Feuerball, Ressource: Mana");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public CharakterMenu(GameSave gameSave)
    {
        int input = GetUserInputNumber();
        CreateNewPlayerFromInput(gameSave, input);
    }

    private void CreateNewPlayerFromInput(GameSave gameSave, int input)
    {
        Menu nextMenu;
        gameSave.Fight = new Fight();
        Random rnd = new Random();

        switch (input)
        {
            case 0:
                Shop.CreateNewShop();
                gameSave.Player = new Player(new Warrior());
                gameSave.Fight = new Fight(gameSave.Player, new EnemyGenerator(new Ork(), rnd.Next(1, 4), gameSave.Fight));
                nextMenu = new FightMenu(gameSave);
                break;
            case 1:
                Shop.CreateNewShop();
                gameSave.Player = new Player(new Mage());
                gameSave.Fight = new Fight(gameSave.Player, new EnemyGenerator((new Ork()), rnd.Next(1, 4), gameSave.Fight));
                nextMenu = new FightMenu(gameSave);
                break;
            case 2:
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Falscher Input");
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
    }
}