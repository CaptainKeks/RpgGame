using Game.Enteties;

namespace Game.Utilities;

public class Shop
{
    private static Shop _instance;

    private Shop()
    {
        foreach (var value in Enum.GetValues<BaseValue>())
            Prices[value] = 20;
    }

    public static Shop Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Shop();
            return _instance;
        }
    }

    public static void Overwrite(Shop newShop)
    {
        _instance = newShop ?? _instance;
    }

    /// <summary>
    /// Erstellt eine neue Instanz von Shop.
    /// </summary>
    public static Shop CreateNewShop()
    {
        _instance = new Shop();
        return _instance;
    }

    public Dictionary<BaseValue, int> Prices { get; set; } = new Dictionary<BaseValue, int>();

    public record ShopValueUpgrade(BaseValue ValueToUpgrade, int value);
}