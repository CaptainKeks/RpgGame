using Game.Charakters;
using Game.Enteties;
using Game.Helper;
using Game.Menus;
using Game.Menus.FinishMenus;
using Game.Utilities;

namespace Game.Combat;

public class Fight
{
    public int Turn { get; set; } = 1;
    public int Round { get; set; } = 1;
    public int Level { get; set; } = 1;
    public int EnemyCount { get; set; } = 0;
    public int MaxLevel { get; set; } = 3;
    public List<Entity> Entities { get; set; } = [];
    public List<Enemy> Enemies { get; set; } = [];
    public Player Player { get; set; } = new();
    public bool isGameFinished { get; set; } = false;
    public bool isLevelFinished { get; set; } = false;

    public Fight() { }

    /// <summary>
    /// Befüllt Player, erzeugt neue Enemies und befüllt Enteties
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemies"></param>
    public Fight(Player player, EnemyGenerator enemies)
    {
        Entities.Add(player);
        Player = player;
        Entities.AddRange(enemies.Enemies);
        Enemies.AddRange(enemies.Enemies);
    }

    public AvailableOptions GetAvailableOptions()
    {
        List<ActivePlayerActionEnum> playerActionEnums = Enum.GetValues<ActivePlayerActionEnum>().Cast<ActivePlayerActionEnum>().ToList();
        return new AvailableOptions(Player, playerActionEnums);
    }

    /// <summary>
    /// Gibt eine Liste von Enteties zurück wer Ausgewählt werden kann je nach Aktion
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public List<Entity> GetAvailableTargets(ActivePlayerActionEnum action)
    {
        if (action == ActivePlayerActionEnum.Attack)
            return [.. Enemies];

        if (action == ActivePlayerActionEnum.SpecialAttack)
            return [.. Enemies];

        if (action == ActivePlayerActionEnum.Defend)
            return [Player];

        else if (action == ActivePlayerActionEnum.UseItem)
            return Entities;

        else
            return Entities;
    }

    /// <summary>
    /// Gibt einen History Eintrag zurück und führt die Ausgewählte Aktion aus die der Spieler ausgewählt hat
    /// </summary>
    /// <param name="decision"></param>
    /// <returns></returns>
    public ActionHistoryEntry PlayerMove(PlayerChoice decision, GameSave gameSave)
    {
        if (decision == null)
            return null;

        ActionHistoryEntry historyEntry = null;
        TryIfHealthIsZero(gameSave);

        switch (decision.Action)
        {
            case ActivePlayerActionEnum.Attack:
                historyEntry = Player.Attack(decision.Target.Last());
                break;

            case ActivePlayerActionEnum.SpecialAttack:
                historyEntry = Player.SpecialAttack(decision.Target.Last());
                break;

            case ActivePlayerActionEnum.Defend:
                historyEntry = Player.GetInDefensePosition();
                break;

            case ActivePlayerActionEnum.UseItem:
                historyEntry = Player.Inventory.UseItem(decision.ViewItem, Player, decision.Target.Last());
                break;
        }
        Turn++;
        return historyEntry;
    }

    /// <summary>
    /// Führt eine Aktion aus die durch Zufall generiert wird. Und zählt die Runde eins höher
    /// </summary>
    /// <returns></returns>
    public List<ActionHistoryEntry> EnemyMove(GameSave gameSave)
    {
        List<ActionHistoryEntry> list = new List<ActionHistoryEntry>();
        ActionHistoryEntry historyEntry = null;
        TryIfHealthIsZero(gameSave);

        foreach (var enemy in Enemies)
        {
            var enemyMove = 0;
            var rnd = new Random();
            if (enemy.Inventory.Items.Count > 0)
                enemyMove = rnd.Next(1, 5);
            else
                enemyMove = rnd.Next(1, 4);

            switch (enemyMove)
            {
                case 1:
                    historyEntry = enemy.Attack(Player);
                    list.Add(historyEntry);
                    continue;

                case 2:
                    historyEntry = enemy.SpecialAttack(Player);
                    list.Add(historyEntry);
                    continue;

                case 3:
                    historyEntry = enemy.GetInDefensePosition();
                    list.Add(historyEntry);
                    continue;

                case 4:
                    var items = enemy.Inventory.GetInventoryContents();
                    historyEntry = enemy.Inventory.UseItem(items[0], enemy, Player);
                    list.Add(historyEntry);
                    continue;
            }
        }
        Round++;
        Turn = 1;
        return list;
    }

    /// <summary>
    /// Wirkt jeden Statuseffekt jeder Entetie
    /// </summary>
    /// <returns></returns>
    public List<ActionHistoryEntry> ApplyStatusEffects()
    {
        var list = new List<ActionHistoryEntry>();
        ActionHistoryEntry? historyEntry = null;
        foreach (Entity entity in Entities)
        {
            var statusEffects = entity.StatusEffekts;
            foreach (var effect in statusEffects)
            {
                historyEntry = effect.ApplyStatusAffect(entity);
                list.Add(historyEntry);
            }
            entity.StatusEffekts = statusEffects.Where(effect => effect.Duration > 0).ToList();
        }
        return list;
    }

    /// <summary>
    /// Überprüft ob die Enemies oder der spieler Keine Leben mehr haben. Falls sie keine mehr haben geht es in das nächste Level oder, der Winner oder Loosing Screen wird angezeigt.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemies"></param>
    private void TryIfHealthIsZero(GameSave gameSave)
    {
        isLevelFinished = false;
        foreach (var enemy in gameSave.Fight.Enemies)
            if (enemy.CurrentHealth <= 0)
            {
                gameSave.Player.Inventory.AddGold(12);
                gameSave.Fight.Enemies.Remove(enemy);
                Entities.Remove(enemy);
                break;
            }

        bool playerLooses = gameSave.Player.CurrentHealth <= 0;
        bool playerLevelWin = gameSave.Fight.Enemies.Count == 0;
        bool playerWins = playerLevelWin && Level == MaxLevel;

        if (playerWins)
        {
            gameSave.Player.Stats.Wins += 1;
            SaveAndLoadJson.SaveGameAndWriteIDToGameSave(gameSave);
            Menu nextMenu = new WinnerMenu();
            isGameFinished = true;
            isLevelFinished = true;
        }

        if (playerLevelWin)
        {
            isLevelFinished = true;
            Level++;
            Round = 0;
            Turn = 1;
        }

        if (playerLooses)
        {
            gameSave.Player.Stats.Losses += 1;
            SaveAndLoadJson.DeleteGameSaveFile(gameSave);
            Menu nextMenu = new LoosingMenu();
            isGameFinished = true;
            isLevelFinished = true;
        }
    }

    /// <summary>
    /// Befüllt die Enteties und Enemies mit neuen Gegnern.
    /// </summary>
    /// <param name="enemyGenerator"></param>
    public void CreateNewEnemies(EnemyGenerator enemyGenerator)
    {
        Entities = [Player];
        Enemies = enemyGenerator.Enemies;
        Entities.AddRange(enemyGenerator.Enemies);
    }
}

/// <summary>
/// Speichert die Action eines Spielers zum Darstellen in der UI.
/// </summary>
/// <param name="Action"></param>
/// <param name="Initiator"></param>
/// <param name="Target"></param>
/// <param name="ViewItem"></param>
public record PlayerChoice(ActivePlayerActionEnum Action, Entity Initiator, List<Entity> Target, Inventory.ViewItem? ViewItem = null);

public enum ActivePlayerActionEnum
{
    Attack,
    SpecialAttack,
    Defend,
    UseItem,
    Flee
}

/// <summary>
/// Speichert die Möglichen Aktionen für einen Spieler
/// </summary>
/// <param name="Player"></param>
/// <param name="Actions"></param>
public record AvailableOptions(Player Player, List<ActivePlayerActionEnum> Actions);

/// <summary>
/// Speichert Die Aktion mit Kampfwerten ab um in der UI Darzustellen.
/// </summary>
/// <param name="ActiveAction"></param>
/// <param name="PassiveAction"></param>
/// <param name="Initiator"></param>
/// <param name="Target"></param>
/// <param name="Item"></param>
/// <param name="StatusEffektDuration"></param>
/// <param name="StatusEffektValue"></param>
public record ActionHistoryEntry(ActivePlayerActionEnum? ActiveAction, Entity? Initiator, List<Entity> Target, Inventory.ViewItem? Item = null, int StatusEffektDuration = 0,
                                 double StatusEffektValue = 0, bool noItemUsed = false, double healed = 0);