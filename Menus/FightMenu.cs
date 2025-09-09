using Game.Charakters;
using Game.Combat;
using Game.Enteties;
using Game.Helper;
using Game.Utilities;
using Game.Classes;

namespace Game.Menus;
class FightMenu : Menu
{
    // Hier auch fight Property
    public override void DisplayMenu() { }

    private void DisplayEnteties(Entity player, Fight fight)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Kampf beginnt:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------");
        Console.WriteLine();
        Console.WriteLine($"Spieler: {player.Name,-12} ({player.Class.ClassName,-7}) HP: {player.CurrentHealth:F2}/{player.MaxHealth:F2}");

        foreach (var enemie in fight.Enemies)
            Console.WriteLine($"Gegner:  {enemie.Name,-12} ({enemie.Class.ClassName,-7}) HP: {enemie.CurrentHealth:F2}/{enemie.MaxHealth:F2}");

        Console.WriteLine();
        Console.WriteLine("----------");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {player.Inventory.Gold}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("----------");
    }
    public FightMenu(GameSave gameSave)
    {
        Random rnd = new Random();
        while (!gameSave.Fight.isGameFinished)
        {
            var historyEntry = gameSave.Fight.ApplyStatusEffects();
            EnemyGenerator enemyGenerator = new EnemyGenerator(new Ork(), rnd.Next(1, 4), gameSave.Fight);
            if (gameSave.Fight.isLevelFinished)
                gameSave.Fight.CreateNewEnemies(enemyGenerator);
            DisplayEnteties(gameSave.Fight.Player, gameSave.Fight);
            PrintMenuRoundAndTurn(gameSave.Fight);
            PrintStatusEffects(historyEntry);
            PrintMenuPlayerMove(gameSave);
            DisplayEnteties(gameSave.Fight.Player, gameSave.Fight);
            PrintMenuRoundAndTurn(gameSave.Fight);
            PrintMenuEnemyMove(gameSave);
            SaveAndLoadJson.SaveGameAndWriteIDToGameSave(gameSave);
        }
    }

    private void PrintMenuRoundAndTurn(Fight fight)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Level: {fight.Level}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("---------");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Runde: {fight.Round} Zug: {fight.Turn}                          ");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void PrintStatusEffects(List<ActionHistoryEntry> historyEntrys)
    {
        foreach (var historyEntry in historyEntrys)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{historyEntry.StatusEffektValue}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" Giftschaden wurden {historyEntry.Target[0].Name} hinzugefügt. (Runden: {historyEntry.StatusEffektDuration}) verbleibend.");
            Console.WriteLine();
        }
    }

    private void PrintMenuEnemyMove(GameSave gameSave)
    {
        Console.WriteLine("Der/Die Gegner ist/sind am Zug.");
        Console.WriteLine();
        var historyEntrys = gameSave.Fight.EnemyMove(gameSave);
        foreach (var entry in historyEntrys)
            PrintHistoryEntry(entry, gameSave.Fight);
    }

    private void PrintMenuPlayerMove(GameSave gameSave)
    {
        ActionHistoryEntry historyEntry = null;
        var options = gameSave.Fight.GetAvailableOptions();
        var choice = GetUserInputs(options, gameSave, out bool noItemUsed);

        if (choice.Action == ActivePlayerActionEnum.UseItem && noItemUsed)
        {
            DisplayEnteties(gameSave.Player, gameSave.Fight);
            PrintMenuRoundAndTurn(gameSave.Fight);
            PrintMenuPlayerMove(gameSave);
        }

        historyEntry = gameSave.Fight.PlayerMove(choice, gameSave);
        PrintHistoryEntry(historyEntry, gameSave.Fight);
    }

    private void PrintHistoryEntry(ActionHistoryEntry historyEntry, Fight fight)
    {
        if (historyEntry == null)
            return;

        if (historyEntry.ActiveAction != null)
        {
            switch (historyEntry.ActiveAction)
            {
                case ActivePlayerActionEnum.Attack:
                    Console.WriteLine($"Ich {historyEntry.Initiator.Name} greife mit: {historyEntry.Initiator.GetAttackValue():F2} AttackDamage {historyEntry.Target[0].Name} an!");
                    Console.Write($"{historyEntry.Target[0].Name} bekommt: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{historyEntry.Initiator.ActualDamage:F2} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("AttackDamage!");
                    Console.WriteLine();
                    Console.Write("Drücke [Enter] für den nächsten Zug.");
                    Console.ReadKey();
                    break;
                case ActivePlayerActionEnum.SpecialAttack:
                    Console.WriteLine($"Ich {historyEntry.Initiator.Name} greife mit meiner Spezialattacke und: {historyEntry.Initiator.GetSpecialAttackValue():F2} AttackDamage {historyEntry.Target[0].Name} an!");
                    Console.Write($"{historyEntry.Target[0].Name} bekommt: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{historyEntry.Initiator.ActualDamage:F2} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("AttackDamage!");
                    Console.WriteLine();
                    Console.Write("Drücke [Enter] für den nächsten Zug.");
                    Console.ReadKey();
                    break;

                case ActivePlayerActionEnum.Defend:
                    Console.WriteLine($"Ich {historyEntry.Initiator.Name} gehe in die AbwehrPosition und bekomme bei meinem nächsten Angriff nur 50% Schaden.");
                    Console.WriteLine();
                    Console.Write("Drücke [Enter] für den nächsten Zug.");
                    Console.ReadKey();
                    break;

                case ActivePlayerActionEnum.UseItem:
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    if (historyEntry.Item.Name == "HeilTrank")
                    {
                        if (historyEntry.healed <= 0)
                            Console.WriteLine("Du kannst dich nicht Heilen du hast schon volles Leben!");
                        Console.WriteLine($"Heiltrank hat {historyEntry.Initiator.Name} um {historyEntry.healed:F2} geheilt.");
                    }

                    if (historyEntry.Item.Name == "GiftTrank")
                    {
                        Console.WriteLine($"Wirke {historyEntry.Item.Name} auf {historyEntry.Target[0].Name} für {historyEntry.Item.Duration} Runden, mit Jeweils {historyEntry.Item.Value} Schaden.");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Drücke [Enter] für den nächsten Zug.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private PlayerChoice GetUserInputs(AvailableOptions options, GameSave gameSave, out bool noItemUsed)
    {
        PlayerChoice choice = null;
        Entity target = null;
        noItemUsed = false;
        int availableTargetIndex = 0;
        DisplayAvailableOptions(options);

        int choiceNumber = GetUserInputNumber();
        var action = (ActivePlayerActionEnum)choiceNumber;
        var availableTargets = gameSave.Fight.GetAvailableTargets(action);

        CastInputToEnum(choiceNumber);
        FleeOrOpenUseItemMenu(action, gameSave, out noItemUsed, out choice);

        if (!noItemUsed)
        {
            PrintOptionsForTarget(gameSave.Fight, availableTargets);
            availableTargetIndex = GetUserInputNumber();
        }

        while (availableTargetIndex <= availableTargets.Count)
            try
            {
                target = availableTargets.ElementAt(availableTargetIndex);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gib eine Gültige Zahl ein.");
                availableTargetIndex = GetUserInputNumber();
                target = availableTargets.ElementAt(availableTargetIndex);
            }

        if (choice != null)
            return new PlayerChoice(action, gameSave.Fight.Player, [target], choice.ViewItem);
        else
            return new PlayerChoice(action, gameSave.Fight.Player, [target]);
    }

    /// <summary>
    /// Opens the Menu and returns the User Inputs in form of a PlayerChoice
    /// </summary>
    /// <param name="action"></param>
    /// <param name="fight"></param>
    /// <param name="noItemUsed"></param>
    /// <param name="choice"></param>
    private void FleeOrOpenUseItemMenu(ActivePlayerActionEnum action, GameSave gameSave, out bool noItemUsed, out PlayerChoice choice)
    {
        noItemUsed = false;
        choice = null;
        if (action == ActivePlayerActionEnum.UseItem)
        {
            Menu nextMenu = new UseItemMenu(gameSave.Fight.Player, out noItemUsed, out choice);
        }

        if (action == ActivePlayerActionEnum.Flee)
        {
            SaveAndLoadJson.SaveGameAndWriteIDToGameSave(gameSave);
            Programm.Main();
        }
    }

    /// <summary>
    /// Player options für Target darstellen
    /// </summary>
    /// <param name="fight"></param>
    /// <param name="availableTargets"></param>
    private void PrintOptionsForTarget(Fight fight, List<Entity> availableTargets)
    {
        Console.Clear();
        DisplayEnteties(fight.Player, fight);
        PrintMenuRoundAndTurn(fight);
        DisplayAvailableTargets(availableTargets);
    }

    private void CastInputToEnum(int choiceNumber)
    {
        while (!Enum.IsDefined(typeof(ActivePlayerActionEnum), choiceNumber))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Gib eine Gültige Zahl ein!");
            Console.ForegroundColor = ConsoleColor.White;
            choiceNumber = GetUserInputNumber();
        }
    }

    private void DisplayAvailableTargets(List<Entity> availableTargets)
    {
        int i = 0;
        Console.WriteLine("Wähle ein Target aus:");
        foreach (Entity target in availableTargets)
            Console.WriteLine($"[{i++}]{target.GetShortInfo()}");
    }

    private void DisplayAvailableOptions(AvailableOptions options)
    {
        int i = 0;
        foreach (var action in options.Actions)
        {
            Console.WriteLine($"[{i++}] " + action);
        }
    }
}