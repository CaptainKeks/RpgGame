namespace Game.Menus;

public class SelectNameMenu : Menu
{
    public SelectNameMenu(out string userInput)
    {
        userInput = GetUserInputString();
    }


    public override void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("RPG Kampf-Simulator:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.WriteLine("Gib einen Namen für deinen Charakter ein:");
    }

    private string GetUserInputString()
    {
        string? input = default;
        Console.Write("> ");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            input = "Aria";
        return input;
    }
}