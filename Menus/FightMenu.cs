using Game.Charakters;
using Game.Combat;
using Game.Helper;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static Game.Combat.Fight;

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
        Console.WriteLine($"Gold: {player.MetaProgression.Gold}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("----------");
    }
    public FightMenu(Entity player, Fight fight) // -> zusätzlich Fight übergeben
    {
        // vor der entscheidung fight property darstellen
        // -> alle informationen die nötig sind für spieler 
        // unter anderm auch mögliche Entscheidungen 

        // Spieler füllt alle relavaten entscheidungen für Runde -> Entscheidungs object oder einfacher

        // Entscheidungs bzw. Options Objekt an Fight übergeben 
        // Fight bearbeitet die Entscheidung -> berechnet Konsequenzen

        // -> ausirkungen werden in nächster Loop oder ende je nach entscheidung dargestellt

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
            SaveAndLoadJson.SaveFight(fight);
        }
    }
    public FightMenu(Entity player, FightValues newFightValues)
    {
        this.fightValues = newFightValues;
        bool isFinished = false;
        bool isLevelFinished = true;

        List<Entity> newEnemies = [];
        while (!isFinished)
        {
            if (isLevelFinished)
            {

            }
            isLevelFinished = false;
            fightValues.Turn = 0;
            fightValues.Turn++;
            if (isLevelFinished)
            {
                fightValues.Level++;
                fightValues.Turn = 0;
                fightValues.Round = 0;
            }
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
            Console.Write($"{historyEntry.Value}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" Giftschaden wurden {historyEntry.Target[0].Name} hinzugefügt. (Runden: {historyEntry.Duration}) verbleibend.");
            Console.WriteLine();
        }
    }

    private void PrintMenuEnemyMove(Fight fight)
    {
        Console.WriteLine("Der/Die Gegner ist/sind am Zug.");
        Console.WriteLine();
        var historyEntrys = fight.EnemyMoveForRound();
        foreach (var entry in historyEntrys)
            PrintHistoryEntry(entry);
    }

    private void PrintMenuPlayerMove(Fight fight)
    {
        ActionHistoryEntry historyEntry = null;
        var options = fight.GetAvailableOptions();
        var choice = GetUserInputs(options, fight, out PlayerChoice itemMenuChoice, out bool noItemUsed);

        if (choice.Action == ActivePlayerActionEnum.UseItem && noItemUsed)
        {
            DisplayEnteties(fight.Player, fight);
            PrintMenuRoundAndTurn(fight);
            PrintMenuPlayerMove(fight);
        }

        if (choice.Action == ActivePlayerActionEnum.UseItem)
            historyEntry = fight.PlayerMoveForRound(itemMenuChoice);

        if (choice.Action != ActivePlayerActionEnum.UseItem)
            historyEntry = fight.PlayerMoveForRound(choice);
        PrintHistoryEntry(historyEntry);
    }

    private void PrintHistoryEntry(ActionHistoryEntry historyEntry)
    {
        if (historyEntry == null)
            return;

        if (historyEntry.PassiveAction != null)
        {
            switch (historyEntry.PassiveAction)
            {
                case PassiveActionEnum.ApplyStatusEffect:

                    break;
            }
        }



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
                    Console.WriteLine($"Ich {historyEntry.Initiator.Name} nutze {historyEntry.Item.Name} mit dem Wert: {historyEntry.Item.Value}.");
                    Console.WriteLine();
                    Console.Write("Drücke [Enter] für den nächsten Zug.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private PlayerChoice GetUserInputs(AvailableOptions options, Fight fight, out PlayerChoice choice, out bool noItemUsed)
    {
        choice = null;
        noItemUsed = false;
        int availableTargetIndex = 0;
        // Available Options darstellen
        DisplayAvailableOptions(options);

        // User Input holen
        int choiceNumber = GetUserInputNumber();
        while (!Enum.IsDefined(typeof(ActivePlayerActionEnum), choiceNumber))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Gib eine Gültige Zahl ein!");
            Console.ForegroundColor = ConsoleColor.White;
            choiceNumber = GetUserInputNumber();
        }
        var action = (ActivePlayerActionEnum)choiceNumber;

        var availableTargets = fight.GetAvailableTargets(action);

        if (action == ActivePlayerActionEnum.UseItem)
        {
            Menu nextMenu = new UseItemMenu(fight.Player, out noItemUsed, out choice);
        }

        if (action == ActivePlayerActionEnum.Flee)
        {
            SaveAndLoadJson.SaveFight(fight);
            SaveAndLoadJson.SaveGame(fight.Player);
            Programm.Main();
        }
        if (!noItemUsed)
        {
            // Player options für Target darstellen
            Console.Clear();
            DisplayEnteties(fight.Player, fight);
            PrintMenuRoundAndTurn(fight);
            DisplayAvailableTargets(availableTargets);

            // Get player input for Target
            availableTargetIndex = GetUserInputNumber();
        }
        Entity target = null;
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

        // User input zu Player Choice verwandln
        return new PlayerChoice(action, fight.Player, [target]);
    }

    private void DisplayAvailableTargets(List<Entity> availableTargets)
    {
        int i = 0;
        Console.WriteLine("Wähle ein Target aus:");
        foreach (Entity target in availableTargets)
            Console.WriteLine($"[{i++}]{target.GetShortInfo()}");
    }

    private static int GetUserInputNumber()
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