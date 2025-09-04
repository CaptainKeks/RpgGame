
using Game.Charakters;

namespace Game.Menus;

public class LoadPlayerMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("RPG Kampf-Simulator:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.WriteLine("---------------------");
        Console.WriteLine("Wähle einen Spieler: ");
        Console.WriteLine("---------------------");
        Console.WriteLine();
    }

    public LoadPlayerMenu(List<Entity> saveGames)
    {
        for (int i = 0; i < saveGames.Count; i++)
        {
            Console.WriteLine($"[{i}] Spielstand {i}");
        }
    }
}
