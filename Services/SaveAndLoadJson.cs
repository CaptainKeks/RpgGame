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

    public static void DeleteGameSaveFile(GameSave gameSave)
    {
        File.Delete(AppContext.BaseDirectory + $"{gameSave.ID}.json");
    }

    public static void SaveGameAndWriteIDToGameSave(GameSave gameSave)
    {
        Guid id = Guid.Empty;
        if (gameSave.ID == Guid.Empty)
        {
            id = Guid.NewGuid();
            gameSave.ID = id;
        }
        else
            id = gameSave.ID;
        string text = JsonConvert.SerializeObject(gameSave, settings);

        var path =Path.Combine(AppContext.BaseDirectory, "GameSaves");
        File.WriteAllText(path + $"\\{id}.json", text);
    }

    public static List<GameSave> LoadGamesAndCreateFolder(out bool succeded, bool firstLoad = false)
    {
        succeded = false;
        List<GameSave> gameSaves = new List<GameSave>();
        var path =Path.Combine(AppContext.BaseDirectory, "GameSaves");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(path, "*.json"))
        {
            try
            {
                if (!File.Exists(file) && firstLoad)
                    return [];
                string text = File.ReadAllText(file);
                var save = JsonConvert.DeserializeObject<GameSave>(text, settings);
                succeded = true;
                if (save.ID != Guid.Empty)
                    gameSaves.Add(save);
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
                return [];
            }
        }
        return gameSaves;
    }
}