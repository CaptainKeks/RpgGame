
namespace Game.Menus.FinishMenus;

class LoosingMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Du hast Verloren!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Drücke [Enter] um auf den Hauptbildschirm zu gelangen.");
        Console.ReadKey();
    }
    public LoosingMenu()
    {       
        Programm.Main();
    }
}