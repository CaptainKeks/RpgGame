namespace Game.Menus;

public abstract class Menu
{
    public Menu()
    {
        Console.Clear();
        DisplayMenu();
    }

    public abstract void DisplayMenu();
    public virtual int GetUserInputNumber()
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