
using Game.Combat;
using Game.Items;

namespace Game.Charakters;

public class EnemyGenerator
{
    public List<Enemy> Enemies { get; private set; } = [];
    public EnemyGenerator(Class @class, int amount, Fight fight)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy enemy = new Enemy(@class, fight.Level);
            Enemies.Add(enemy);
        }
    }
}