using Game.Charakters;
using Game.Combat;

namespace Game;

public class GameSaves
{
    public GameSaves(ShopBonusStats metaProgression, Player player, Fight fight)
    {
        MetaProgression = metaProgression;
        Player = player;
        Fight = fight;
    }

    public ShopBonusStats MetaProgression { get; set; }
    public Player Player { get; set; }
    public Fight Fight { get; set; }
}
