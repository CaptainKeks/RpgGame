
using Game.Charakters;
using Game.Helper;
using Game.Items;
using Game.Menus;
using Game.Menus.FinishMenus;

namespace Game.Combat;

public class Fight
{
    public int Turn { get; set; } = 1;
    public int Round { get; set; } = 1;
    public int Level { get; set; } = 1;
    public int EnemyCount { get; set; }
    public int MaxLevel { get; set; } = 3;
    public List<Entity> Entities { get; set; } = [];
    public List<Enemy> Enemies { get; set; } = [];
    public Player Player { get; set; }
    public bool isGameFinished { get; set; } = false;
    public bool isLevelFinished { get; set; }

    public Fight()
    {

    }
    /// <summary>
    /// Befüllt Player, Enemies und Enteties
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemies"></param>
    public Fight(Player player, List<Enemy> enemies)
    {
        Entities.Add(player);
        Player = player;
        Entities.AddRange(enemies);
        Enemies.AddRange(enemies);
    }

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
        List<ActivePlayerActionEnum> playerActionEnums = Enum.GetValues<ActivePlayerActionEnum>()
                                                       .Cast<ActivePlayerActionEnum>().ToList();

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
    public ActionHistoryEntry PlayerMoveForRound(PlayerChoice decision)
    {
        if (decision == null)
            return null;

        ActionHistoryEntry historyEntry = null;
        TryIfHealthIsZero(Player, Enemies);

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
                historyEntry = decision.Item.UseItem(Player, Enemies[0], out _);
                break;
        }
        Turn++;
        return historyEntry;
    }

    /// <summary>
    /// Führt eine Aktion aus die durch Zufall generiert wird. Und zählt die Runde eins höher
    /// </summary>
    /// <returns></returns>
    public List<ActionHistoryEntry> EnemyMoveForRound()
    {
        List<ActionHistoryEntry> list = new List<ActionHistoryEntry>();
        ActionHistoryEntry historyEntry = null;
        TryIfHealthIsZero(Player, Enemies);

        foreach (var enemy in Enemies)
        {
            var enemyMove = 0;
            var rnd = new Random();
            if (enemy.Inventory.Where(i => i.Name == "GiftTrank").Count() > 0)
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
                    historyEntry = enemy.Inventory[0].UseItem(enemy, Player, out bool _);
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
    private void TryIfHealthIsZero(Player player, List<Enemy> enemies)
    {
        isLevelFinished = false;
        foreach (var enemy in enemies)
            if (enemy.CurrentHealth <= 0)
            {
                player.MetaProgression.Gold += 12;
                enemies.Remove(enemy);
                Entities.Remove(enemy);
                break;
            }

        bool playerLooses = player.CurrentHealth <= 0;
        bool playerLevelWin = enemies.Count == 0;
        bool playerWins = playerLevelWin && Level == MaxLevel;

        if (playerWins)
        {
            player.MetaProgression.Wins += 1;
            SaveAndLoadJson.SaveGame(player);
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
            player.MetaProgression.Losses += 1;
            SaveAndLoadJson.SaveGame(player);
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

    /// <summary>
    /// Speichert die Action eines Spielers zum Darstellen in der UI.
    /// </summary>
    /// <param name="Action"></param>
    /// <param name="Initiator"></param>
    /// <param name="Target"></param>
    /// <param name="Item"></param>
    public record PlayerChoice(ActivePlayerActionEnum Action, Entity Initiator, List<Entity> Target, Item? Item = null);

    public enum ActivePlayerActionEnum
    {
        Attack = 1,
        SpecialAttack = 2,
        Defend = 3,
        UseItem = 4,
        Flee = 5
    }

    public enum PassiveActionEnum
    {
        ApplyStatusEffect = 0
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
    /// <param name="Duration"></param>
    /// <param name="Value"></param>
    public record ActionHistoryEntry(ActivePlayerActionEnum? ActiveAction, PassiveActionEnum? PassiveAction, Entity? Initiator, List<Entity> Target, Item? Item = null, int Duration = 0, double Value = 0);
}