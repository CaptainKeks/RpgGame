using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Utilities;
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
    public UpgradeMenu(GameSave gameSave)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {gameSave.Player.Inventory.Gold}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("------------");
        Console.WriteLine();
        Console.WriteLine();

        Console.Write($"[1] Attack        + 1    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopAttackStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Attack]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[2] Defense       + 1    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopDefenseStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Defense]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[3] Wisdom        + 0.1  (Aktueller Wert: {gameSave.Player.ShopBonusStats.BBonusShopWisdomStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Wisdom]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[4] Health        + 5    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopHealthStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Health]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[5] HealthPotion  + 5    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopHealthPotionStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.HealthPotion]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[6] PoisenPotion  + 1    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopPoisonPotionStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.PoisenPotion]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("[7] Zurück");
        HandleInput(gameSave);
    }

    private void HandleInput(GameSave gameSave)
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
                    gameSave.Player.UpgradeBaseValue(BaseValue.Attack);
                    Menu nextMenu = new UpgradeMenu(gameSave);
                    Programm.HandleInput(gameSave);
                    validInput = true;
                    break;
                case "2":
                    gameSave.Player.UpgradeBaseValue(BaseValue.Defense);
                    nextMenu = new UpgradeMenu(gameSave);
                    Programm.HandleInput(gameSave);
                    validInput = true;
                    break;
                case "3":
                    gameSave.Player.UpgradeBaseValue(BaseValue.Wisdom);
                    nextMenu = new UpgradeMenu(gameSave);
                    Programm.HandleInput(gameSave);
                    validInput = true;
                    break;
                case "4":
                    gameSave.Player.UpgradeBaseValue(BaseValue.Health);
                    nextMenu = new UpgradeMenu(gameSave);
                    Programm.HandleInput(gameSave);
                    validInput = true;
                    break;
                case "5":
                    gameSave.Player.UpgradeBaseValue(BaseValue.HealthPotion);
                    nextMenu = new UpgradeMenu(gameSave);
                    Programm.HandleInput(gameSave);
                    validInput = true;
                    break;
                case "6":
                    gameSave.Player.UpgradeBaseValue(BaseValue.PoisenPotion);
                    nextMenu = new UpgradeMenu(gameSave);
                    Programm.HandleInput(gameSave);
                    validInput = true;
                    break;
                case "7":
                    SaveAndLoadJson.SaveGameAndWriteIDToGameSave(gameSave);
                    nextMenu = new StartMenu(gameSave);
                    Programm.HandleInput(gameSave);
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