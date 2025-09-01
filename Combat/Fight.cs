
using Game.Charakters;
using Game.Helper;
using Game.Items;
using Game.Menus;
using Game.Menus.FinishMenus;

namespace Game.Combat;

public class Fight
{
    // Zustand über Fight über public getter
    // Totzdem alle teilnehme player + enemies als property
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

    public Fight(Player player, List<Enemy> enemies) // Enemies vielleicht hier erzeugen -> EnemyGenerator reinreichen
    {
        // Teilnehmer befüllen -> enemies hinzufügen
        Entities.Add(player);
        Player = player;
        Entities.AddRange(enemies);
        Enemies.AddRange(enemies);
    }

    public Fight(Player player, EnemyGenerator enemies) // Enemies vielleicht hier erzeugen -> EnemyGenerator reinreichen
    {
        // Teilnehmer befüllen -> mit übergebenen generator erzeugen
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
        // switch je nach Action
        // 
        // aktion ausführen trigger und target übergeben

        // turns + round entsprechend anpassen

        // EnemyMove();
        // SelectTarget();
        // nach aufruf ist der Zustand hier wieder anders 
        // und kann dargestellt werden
        // turns + round entsprechend anpassen

        // ApplyStatusEffects();
    }

    public List<ActionHistoryEntry> EnemyMoveForRound()
    {
        List<ActionHistoryEntry> list = new List<ActionHistoryEntry>();
        ActionHistoryEntry historyEntry = null;
        TryIfHealthIsZero(Player, Enemies);

        foreach (var enemy in Enemies)
        {
            var rnd = new Random();
            var enemyMove = rnd.Next(1, 4);
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
                    historyEntry = enemy.Inventory[0].UseItem(enemy, enemy, out bool _);
                    list.Add(historyEntry);
                    continue;
            }
        }
        Round++;
        Turn = 1;
        return list;
    }

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

    public void CreateNewEnemies(EnemyGenerator enemyGenerator)
    {
        Entities = [Player];
        Enemies = enemyGenerator.Enemies;
        Entities.AddRange(enemyGenerator.Enemies);
    }

    // Hier wird die Entscheidung gespeichert
    // z.B Action -> angriff (PlayerActionEnum)
    // verursacher + target (sind bei IEntity)
    // Hinweis für später
    // später vielleicht statt enum (Attack, SpecialAttack, Defend) ... -> IAction
    // So kann man verschiedene Attacken, bzw. Items gleich mit übergeben
    // so fällt auch arg Item weg
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
    public record AvailableOptions(Player Player, List<ActivePlayerActionEnum> Actions);

    // Werte in einen Record speichern um Angriffswerte anzeigen lassen, aus dem charakter löschen kein print in der logik funktion außer debugging
    public record ActionHistoryEntry(ActivePlayerActionEnum? ActiveAction, PassiveActionEnum? PassiveAction, Entity? Initiator, List<Entity> Target, Item? Item = null, int? Duration = 0, double? Value = 0);
}