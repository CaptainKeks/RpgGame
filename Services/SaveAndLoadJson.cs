using Game.Charakters;
using Game.Combat;
using Game.Utilities;
using Newtonsoft.Json;

namespace Game.Helper;

static class SaveAndLoadJson
{
    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        // die settings anpassen
    };

    public static void SaveGame(GameSaves gameSaves)
    {
        string text = JsonConvert.SerializeObject(gameSaves, settings);
        File.WriteAllText(AppContext.BaseDirectory + "gamesave.json", text);
    }

    public static Fight LoadFight()
    {
        try
        {
            var fight = new Fight();
            string text = File.ReadAllText(AppContext.BaseDirectory + "saveFight.json");
            fight = JsonConvert.DeserializeObject<Fight>(text, settings);
            return fight;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadKey();
            return new Fight();
        }
    }

    public static (Player, Fight) LoadGame(out bool succeded, bool firstLoad = false)
    {
        succeded = false;
        GameSaves gameSaves = new GameSaves();
        try
        {
            if (!File.Exists(AppContext.BaseDirectory + "gamesave.json") && firstLoad)
                return (new Player(), new Fight());
            string text = File.ReadAllText(AppContext.BaseDirectory + "gamesave.json");
            gameSaves = JsonConvert.DeserializeObject<GameSaves>(text, settings);
            succeded = true;
            return DeserealizeToPlayerAndFight(gameSaves);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Fehler: ");
            Console.WriteLine(ex.Message);
            Console.WriteLine("Erstelle zuerst ein Neues Spiel.");
            Console.ForegroundColor = ConsoleColor.White;
            succeded = false;
            Console.ReadKey();
            return (new Player(), new Fight());
        }
    }

    private static (Player, Fight) DeserealizeToPlayerAndFight(GameSaves gameSaves)
    {
        Player player = gameSaves.Player;
        Fight fight = gameSaves.Fight;
        Shop.Overwrite(gameSaves.Shop);
        return (player, fight);
    }

}