using Game.Charakters;
using Game.Combat;
using Game.Utilities;

namespace Game;

public class GameSaves
{
    public GameSaves(Player player, Fight fight, Shop shop)
    {
        Player = player;
        Fight = fight;
        Shop = shop;
    }
    public GameSaves() { }


    public Player Player { get; set; } = new Player();
    public Fight Fight { get; set; } = new Fight();
    public Shop Shop { get; set; } = Shop.Instance;
}