namespace Game.Menus;

public abstract class Menu
{
    public Menu()
    {
        Console.Clear();
        DisplayMenu();
    }

    public abstract void DisplayMenu();
}