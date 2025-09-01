
using Game.Combat;

namespace Game.Charakters;

public class EnemyGenerator
{
    public List<Enemy> Enemies { get; private set; } = [];
    public EnemyGenerator(Class @class, int amount, Fight fight)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy enemy = new Enemy();
            InitializeEnemie(enemy, @class, fight);
            Enemies.Add(enemy);
        }
    }

    private void InitializeEnemie(Enemy enemy, Class @class, Fight fight)
    {
        Random rnd = new Random();
        enemy.Name = "Orga";
        enemy.Class = @class;
        enemy.MaxHealth = (enemy.GetMaxHealthValue() - 8) + (4 * fight.Level);
        enemy.CurrentHealth = enemy.MaxHealth;
        enemy.Class.AttackModifier = rnd.Next((int)(enemy.Class.AttackModifier + (fight.Level * 1.5) - 2), (int)(enemy.Class.AttackModifier + (fight.Level * 1.5) + 2));
    }
}