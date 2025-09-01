
namespace Game.Menus.FinishMenus;

class WinnerMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Du hast Gewonnen!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Drücke [Enter] um auf den Hauptbildschirm zu gelangen.");
        Console.ReadKey();
    }
    public WinnerMenu()
    {
        Programm.Main();
    }
}