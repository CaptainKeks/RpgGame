using Game.Charakters;
using Game.Combat;
using Newtonsoft.Json;

namespace Game.Helper;

static class SaveAndLoadJson
{
    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    public static void SaveGame(Entity player)
    {
        string text = JsonConvert.SerializeObject(player, settings);
        File.WriteAllText(AppContext.BaseDirectory + "savegame.json", text);
    }

    public static void SaveGame(GameSaves gameSaves)
    {
        string text = JsonConvert.SerializeObject(gameSaves, settings);
        File.WriteAllText(AppContext.BaseDirectory + "gamesave.json", text);
    }

    public static void SaveFight(Fight fight)
    {
        string text = JsonConvert.SerializeObject(fight, settings);
        File.WriteAllText(AppContext.BaseDirectory + "saveFight.json", text);
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

    public static Player LoadGame(out bool succeded)
    {
        try
        {
            string text = File.ReadAllText(AppContext.BaseDirectory + "savegame.json");
            Player player = JsonConvert.DeserializeObject<Player>(text, settings);
            succeded = true;
            return player;
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
            return null;
        }
    }
}