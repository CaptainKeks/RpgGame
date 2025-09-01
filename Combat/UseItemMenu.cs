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
        for (int i = 0; i < player.Inventory.Count; i++)
            Console.WriteLine($"[{i}] {player.Inventory[i].Name} ({player.Inventory[i].Count}x) <{player.Inventory[i].Description}> ");
        Console.WriteLine($"[{player.Inventory.Count}] zurück zum Kampf");
        historyEntry = HandleInput(player, out noItemUsed);
    }

    private PlayerChoice HandleInput(Entity player, out bool noItemUsed)
    {
        PlayerChoice historyEntry = null;
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
                    historyEntry = new PlayerChoice(ActivePlayerActionEnum.UseItem, player, [], player.Inventory[id]);
                    validInput = true;
                    break;
                case "1":
                    historyEntry = new PlayerChoice(ActivePlayerActionEnum.UseItem, player, [], player.Inventory[id]);
                    validInput = true;
                    break;
                case "2":
                    historyEntry = null;
                    noItemUsed = true;
                    validInput = true;
                    break;
                default:
                    historyEntry = null;
                    validInput = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Falscher Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        return historyEntry;
    }
}