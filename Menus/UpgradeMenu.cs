using Game.Combat;
using Game.Enteties;
using Game.Helper;
using Game.Utilities;
using static Programm;
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

        Console.Write($"[0] Attack        + 1    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopAttackStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Attack]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[1] Defense       + 1    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopDefenseStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Defense]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[2] Wisdom        + 0.1  (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopWisdomStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Wisdom]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[3] Health        + 5    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopHealthStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.Health]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[4] HealthPotion  + 5    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopHealthPotionStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.HealthPotion]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write($"[5] PoisenPotion  + 1    (Aktueller Wert: {gameSave.Player.ShopBonusStats.BonusShopPoisonPotionStat}) \t");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Preis: {Shop.Instance.Prices[BaseValue.PoisenPotion]} Gold!");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("[6] Zurück");
        int input = GetUserInputNumber();
        UpgradeBaseValueFromUserInput(gameSave, input);
    }

    private List<GameActionsEnum> GetLoadedSaveGameActions()
    {
        return Enum.GetValues<GameActionsEnum>().Cast<GameActionsEnum>().ToList();
    }

    private void UpgradeBaseValueFromUserInput(GameSave gameSave, int input)
    {
        UpgradedStatsFromShop bonusStats = new UpgradedStatsFromShop(0, 0, 0, 0, 0, 0);
        switch (input)
        {
            case 0:
                gameSave.Player.UpgradeBaseValue(BaseValue.Attack);
                Menu nextMenu = new UpgradeMenu(gameSave);
                Programm.SelectNextMenuFromUserInput(gameSave);
                break;
            case 1:
                gameSave.Player.UpgradeBaseValue(BaseValue.Defense);
                nextMenu = new UpgradeMenu(gameSave);
                Programm.SelectNextMenuFromUserInput(gameSave);
                break;
            case 2:
                gameSave.Player.UpgradeBaseValue(BaseValue.Wisdom);
                nextMenu = new UpgradeMenu(gameSave);
                Programm.SelectNextMenuFromUserInput(gameSave);
                break;
            case 3:
                gameSave.Player.UpgradeBaseValue(BaseValue.Health);
                nextMenu = new UpgradeMenu(gameSave);
                Programm.SelectNextMenuFromUserInput(gameSave);
                break;
            case 4:
                gameSave.Player.UpgradeBaseValue(BaseValue.HealthPotion);
                nextMenu = new UpgradeMenu(gameSave);
                Programm.SelectNextMenuFromUserInput(gameSave);
                break;
            case 5:
                gameSave.Player.UpgradeBaseValue(BaseValue.PoisenPotion);
                nextMenu = new UpgradeMenu(gameSave);
                Programm.SelectNextMenuFromUserInput(gameSave);
                break;
            case 6:
                SaveAndLoadJson.SaveGameAndWriteIDToGameSave(gameSave);
                nextMenu = new StartMenu(gameSave, GetLoadedSaveGameActions());
                Programm.SelectNextMenuFromUserInput(gameSave);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Falscher Input");
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
    }
}