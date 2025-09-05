using Game.Charakters;
using Game.Combat;
using Game.Utilities;

namespace Game;

public class GameSave
{
    public GameSave(Player player, Fight fight, Shop shop)
    {
        Player = player;
        Fight = fight;
        Shop = shop;
    }
    public GameSave() { }


    public Player Player { get; set; } = new Player();
    public Fight Fight { get; set; } = new Fight();
    public Shop Shop { get; set; } = Shop.Instance;
    public Guid ID { get; set; } = Guid.Empty;
}