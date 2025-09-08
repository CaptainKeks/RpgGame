using Game.Combat;
using Game.Enteties;
using System.Runtime.CompilerServices;

namespace Game.Menus;
class UseItemMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.WriteLine("Inventar: ");
        Console.WriteLine("----------");
        Console.WriteLine();
        Console.WriteLine("wähle ein Item aus:");
    }

    public UseItemMenu(Entity player, out bool noItemUsed, out PlayerChoice choice)
    {
        var content = player.Inventory.GetInventoryContents();
        for (int i = 0; i < content.Length; i++)
            Console.WriteLine($"[{i}] {content[i].Name} ({content[i].Count}) <{content[i].Description}>");
        Console.WriteLine($"[{content.Length}] zurück zum Kampf");
        // historyEntry = HandleInput(player, out noItemUsed);
        int input = GetUserInputNumber();
        choice = CreateUseItemPlayerChoiceFromUserInput(content, player, input, out noItemUsed);
    }


    private PlayerChoice CreateUseItemPlayerChoiceFromUserInput(Inventory.ViewItem[] content, Entity player, int input, out bool noItemUsed)
    {
        PlayerChoice choice = null;
        noItemUsed = false;
        for (int i = 0; i < content.Length; i++)
        {
            if (content.Length > input)
            {
                if (content[i].Name == player.Inventory.Items[input].Item.Name)
                {
                    noItemUsed = false;
                    choice = new PlayerChoice(ActivePlayerActionEnum.UseItem, player, [], player.Inventory.Items[i].GetViewItem());
                }
            }
            else if (content.Length == input)
            {
                noItemUsed = true;
                return choice;
            }
            else
            {
                noItemUsed = true;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Falscher Input");
                Console.ForegroundColor = ConsoleColor.White;
                input = GetUserInputNumber();
                choice = CreateUseItemPlayerChoiceFromUserInput(content, player, input, out noItemUsed);
                return choice;
            }
        }
        return choice;
    }

    private int GetUserInputNumber()
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