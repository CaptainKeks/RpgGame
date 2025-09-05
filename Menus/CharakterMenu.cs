using Game.Charakters;
using Game.Combat;
using Game.Utilities;

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
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("[1] Warrior  HP: Hoch, Blocken, Waffe: Schwert");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("[2] Mage     HP: Mittel, Waffe: Feuerball, Ressource: Mana");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public CharakterMenu(GameSave gameSave)
    {
        HandleInput(gameSave);
    }

    private void HandleInput(GameSave gameSave)
    {
        Menu nextMenu;
        string input;
        bool validInput = false;
        gameSave.Fight = new Fight();
        Random rnd = new Random();

        while (true)
        {
            Console.Write("> ");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Shop.CreateNewShop();
                    gameSave.Player = new Player(new Warrior());
                    gameSave.Fight = new Fight(gameSave.Player, new EnemyGenerator(new Ork(), rnd.Next(1, 4), gameSave.Fight));
                    nextMenu = new FightMenu(gameSave);
                    validInput = true;
                    break;
                case "2":
                    Shop.CreateNewShop();
                    gameSave.Player = new Player(new Mage());
                    gameSave.Fight = new Fight(gameSave.Player, new EnemyGenerator((new Ork()), rnd.Next(1, 4), gameSave.Fight));
                    nextMenu = new FightMenu(gameSave);
                    validInput = true;
                    break;
                case "3":
                    validInput = true;
                    break;
                default:
                    validInput = false;
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