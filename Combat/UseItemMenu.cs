using Game.Charakters;
using Game.Items;
using Game.Menus;
using static Game.Combat.Fight;

namespace Game.Combat;
class UseItemMenu : Menu
{
    public override void DisplayMenu()
    {
        Console.WriteLine("Inventar: ");
        Console.WriteLine("----------");
        Console.WriteLine();
        Console.WriteLine("wähle ein Item aus:");
    }

    public UseItemMenu(Entity player, out bool noItemUsed, out PlayerChoice historyEntry)
    {
       var content = player.Inventory.GetInventoryContents();
        for (int i = 0; i < content.Length; i++)
            Console.WriteLine($"[{i}] {content[i].Name} ({content[i].Count}) <{content[i].Description}>");
        Console.WriteLine($"[{content.Length}] zurück zum Kampf");
        historyEntry = HandleInput(player, out noItemUsed);
    }

    private PlayerChoice HandleInput(Entity player, out bool noItemUsed)
    {
        PlayerChoice choice = null;
        noItemUsed = false;
        string input = "";
        bool validInput = false;

        while (!validInput)
        {
            Console.Write("> ");
            input = Console.ReadLine();
            int id = Convert.ToInt32(input);

            switch (input)
            {
                case "0":
                    choice = new PlayerChoice(ActivePlayerActionEnum.UseItem, player, [], player.Inventory.Items[id].Item);
                    validInput = true;
                    break;
                case "1":
                    choice = new PlayerChoice(ActivePlayerActionEnum.UseItem, player, [], player.Inventory.Items[id].Item);
                    validInput = true;
                    break;
                case "2":
                    choice = null;
                    noItemUsed = true;
                    validInput = true;
                    break;
                default:
                    choice = null;
                    validInput = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        return choice;
    }
}