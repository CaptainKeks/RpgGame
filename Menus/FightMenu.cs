using Game.Charakters;
using Game.Combat;
using Game.Helper;
using Game.Items;
using Game.Utilities;

namespace Game.Menus;
class FightMenu : Menu
{
    private FightValues fightValues = new FightValues();
    // Hier auch fight Property
    public override void DisplayMenu() { }

    // Nur in Menus Console.WriteLine verwenden
    // Kampfklasse erstellen mit nur logik ohne ConsoleWrite Line
    // Logik Funktionen returnen nur werte nicht mehr
    // Print funtionen in den Menu klassen nehmen die werte auf und printen sie raus

    private void DisplayEnteties(Entity player, Fight fight)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Kampf beginnt:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("--------------");
        Console.WriteLine();
        Console.WriteLine($"Spieler: {player.Name} ({player.Class.ClassName}) HP: {player.CurrentHealth:F2}/{player.MaxHealth:F2}");

        foreach (var enemie in fight.Enemies)
            Console.WriteLine($"Gegner: {enemie.Name} ({enemie.Class.ClassName}) HP: {enemie.CurrentHealth:F2}/{enemie.MaxHealth:F2}");

        Console.WriteLine();
        Console.WriteLine("----------");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {player.Inventory.Gold}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("----------");
    }
    public FightMenu(Player player, Fight fight)
    {
        Random rnd = new Random();

        while (!fight.isGameFinished)
        {
            var historyEntry = fight.ApplyStatusEffects();
            EnemyGenerator enemyGenerator = new EnemyGenerator(new Ork(), 1, fight);
            if (fight.isLevelFinished)
                fight.CreateNewEnemies(enemyGenerator);
            DisplayEnteties(player, fight);
            PrintMenuRoundAndTurn(fight);
            PrintStatusEffects(historyEntry);
            PrintMenuPlayerMove(fight);
            DisplayEnteties(player, fight);
            PrintMenuRoundAndTurn(fight);
            PrintMenuEnemyMove(fight);
            SaveAndLoadJson.SaveGame(new GameSaves(player, fight, Shop.Instance));
        }
    }

    private void PrintMenuRoundAndTurn(Fight fight)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
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

    private void PrintMenuEnemyMove(Fight fight)
    {
        Console.WriteLine("Der/Die Gegner ist/sind am Zug.");
        Console.WriteLine();
        var historyEntrys = fight.EnemyMoveForRound();
        foreach (var entry in historyEntrys)
            PrintHistoryEntry(entry, fight);
    }

    private void PrintMenuPlayerMove(Fight fight)
    {
        ActionHistoryEntry historyEntry = null;
        var options = fight.GetAvailableOptions();
        var choice = GetUserInputs(options, fight, out bool noItemUsed);

        if (choice.Action == ActivePlayerActionEnum.UseItem && noItemUsed)
        {
            DisplayEnteties(fight.Player, fight);
            PrintMenuRoundAndTurn(fight);
            PrintMenuPlayerMove(fight);
        }


        historyEntry = fight.PlayerMoveForRound(choice);
        PrintHistoryEntry(historyEntry, fight);
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

    private PlayerChoice GetUserInputs(AvailableOptions options, Fight fight, out bool noItemUsed)
    {
        PlayerChoice choice = null;
        Entity target = null;
        noItemUsed = false;
        int availableTargetIndex = 0;
        DisplayAvailableOptions(options);

        int choiceNumber = GetUserInputNumber();
        var action = (ActivePlayerActionEnum)choiceNumber;
        var availableTargets = fight.GetAvailableTargets(action);

        CastInputToEnum(choiceNumber);
        OpenFleeOrUseItemMenu(action, fight, out noItemUsed, out choice);

        if (!noItemUsed)
        {
            PrintOptionsForTarget(fight, availableTargets);
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
            return new PlayerChoice(action, fight.Player, [target], choice.ViewItem);
        else
            return new PlayerChoice(action, fight.Player, [target]);
    }

    /// <summary>
    /// Opens the Menu and returns the User Inputs in form of a PlayerChoice
    /// </summary>
    /// <param name="action"></param>
    /// <param name="fight"></param>
    /// <param name="noItemUsed"></param>
    /// <param name="choice"></param>
    private void OpenFleeOrUseItemMenu(ActivePlayerActionEnum action, Fight fight, out bool noItemUsed, out PlayerChoice choice)
    {
        noItemUsed = false;
        choice = null;
        if (action == ActivePlayerActionEnum.UseItem)
        {
            Menu nextMenu = new UseItemMenu(fight.Player, out noItemUsed, out choice);
        }

        if (action == ActivePlayerActionEnum.Flee)
        {
            SaveAndLoadJson.SaveGame(new GameSaves(fight.Player, fight, Shop.Instance));
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

    private void DisplayAvailableOptions(AvailableOptions options)
    {
        int i = 1;
        foreach (var action in options.Actions)
        {
            Console.WriteLine($"[{i++}] " + action);
        }
    }
}