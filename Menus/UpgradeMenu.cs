using Game.Charakters;
using Game.Helper;
namespace Game.Menus;

class UpgradeMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Upgrade Menu:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("-------------");
    }
    public UpgradeMenu(Player player)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {player.MetaProgression.Gold}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("------------");
        Console.WriteLine();
        Console.WriteLine($"Preis: {player.MetaProgression.Price} Gold!");
        Console.WriteLine();
        Console.WriteLine($"[1] Attack       + 1   (Aktueller Wert: {player.MetaProgression.Attack})");
        Console.WriteLine($"[2] Defense      + 1   (Aktueller Wert: {player.MetaProgression.Defense})");
        Console.WriteLine($"[3] Wisdom       + 0.1 (Aktueller Wert: {player.MetaProgression.Wisdom})");
        Console.WriteLine($"[4] Health       + 5   (Aktueller Wert: {player.MetaProgression.Health})");
        Console.WriteLine($"[5] HealthPotion + 5   (Aktueller Wert: {player.MetaProgression.HealthPotion})");
        Console.WriteLine($"[6] PoisenPotion + 1   (Aktueller Wert: {player.MetaProgression.PoisonPotion})");
        Console.WriteLine("[7] Zurück");
        HandleInput(player);
    }

    private void HandleInput(Player player)
    {
        string input;
        bool validInput = false;
        while (!validInput)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    player.UpgradeBaseValue(BaseValue.Attack);
                    Menu nextMenu = new UpgradeMenu(player);
                    Programm.HandleInput(player);
                    validInput = true;
                    break;
                case "2":
                    player.UpgradeBaseValue(BaseValue.Defense);
                    nextMenu = new UpgradeMenu(player);
                    Programm.HandleInput(player);
                    validInput = true;
                    break;
                case "3":
                    player.UpgradeBaseValue(BaseValue.Wisdom);
                    nextMenu = new UpgradeMenu(player);
                    Programm.HandleInput(player);
                    validInput = true;
                    break;
                case "4":
                    player.UpgradeBaseValue(BaseValue.Health);
                    nextMenu = new UpgradeMenu(player);
                    Programm.HandleInput(player);
                    validInput = true;
                    break;
                case "5":
                    player.UpgradeBaseValue(BaseValue.HealthPotion);
                    nextMenu = new UpgradeMenu(player);
                    Programm.HandleInput(player);
                    validInput = true;
                    break;
                case "6":
                    player.UpgradeBaseValue(BaseValue.PoisenPotion);
                    nextMenu = new UpgradeMenu(player);
                    Programm.HandleInput(player);
                    validInput = true;
                    break;
                case "7":
                    SaveAndLoadJson.SaveGame(player);
                    nextMenu = new StartMenu(player);
                    Programm.HandleInput(player);
                    validInput = true;
                    break;
                default:
                    validInput = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}