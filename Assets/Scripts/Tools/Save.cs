using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Path = System.IO.Path;

public static class Save
{


    public static string savesPath = Path.Combine(OS.GetUserDataDir(), "saves");

    public static void InitDirectoriesSave()
    {
        if (!Directory.Exists(savesPath))
        {
            Directory.CreateDirectory(savesPath);
        }
    }

    public static List<string> GetSaves()
    {
        InitDirectoriesSave();
        List<string> res = new List<string>();
        foreach (var path in  Directory.GetDirectories(savesPath))
        {
            if (File.Exists(Path.Combine(path, "ollopa.save")))
            {
                res.Add(File.ReadAllText(Path.Combine(path, "ollopa.save")));
            }
        }
        return res;
    }
    
    public static void _Save(string saveName)
    {
        InitDirectoriesSave();
        string savePath = Path.Combine(savesPath, saveName);
        Directory.CreateDirectory(savePath);
        saveConfig(savePath, saveName);
        saveEnvironementData(savePath);
        saveWorldData(savePath);
        savePlayerData(savePath);
    }
    public static void _Load(string saveName)
    {
        InitDirectoriesSave();
        List<string> saves = Save.GetSaves();
        if (!saves.Contains(saveName))
        {
            throw new Exception("errorrrrrr");
        }
        LoadEnvironementData(Path.Combine(savesPath, saveName));
        LoadWorldData(Path.Combine(savesPath, saveName));
        LoadPlayerData(Path.Combine(savesPath, saveName));
    }
    
    
    

    private static void saveConfig(string path, string name)
    {
        string fileName = Path.Combine(path, "ollopa.save");
        if (File.Exists(fileName))
        {    
            File.Delete(fileName);    
        }

        using (StreamWriter sw = File.CreateText(fileName))
        {
            sw.Write(name);
        }
    }
    
    
    
    
    
    private static void saveWorldData(string path)
    {
        string fileName = Path.Combine(path, "World.data");
        if (File.Exists(fileName))
        {    
            File.Delete(fileName);    
        }
        WorldDataModel data = new WorldDataModel();
        data.GetValues();
        string json = JsonConvert.SerializeObject(data);
        using (StreamWriter sw = File.CreateText(fileName))
        {
            sw.Write(json);
        }
    }
    private static void LoadWorldData(string path)
    {
        string filePath = Path.Combine(path, "World.data");
        string jsonString = File.ReadAllText(filePath);
        WorldDataModel data = null;
        try
        {
            data = WorldDataModel.Deserialize(jsonString);
        }
        catch
        {
            throw new Exception("LoadWorldData: invalid syntaxe json");
        }
        data.SetValues();
    }
    
    
    private static void saveEnvironementData(string path)
    {
        string fileName = Path.Combine(path, "Environement.data");
        if (File.Exists(fileName))
        {    
            File.Delete(fileName);    
        }
        EnvironementDataModel data = new EnvironementDataModel();
        data.GetValues();
        string json = JsonConvert.SerializeObject(data);
        using (StreamWriter sw = File.CreateText(fileName))
        {
            sw.Write(json);
        }
    }
    private static void LoadEnvironementData(string path)
    {
        string filePath = Path.Combine(path, "Environement.data");
        string jsonString = File.ReadAllText(filePath);
        EnvironementDataModel data = null;
        try
        {
            data = EnvironementDataModel.Deserialize(jsonString);
        }
        catch
        {
            throw new Exception("saveEnvironementData: invalid syntaxe json");
        }
        data.SetValues();
    }


    private static void savePlayerData(string path)
    {
        string fileName = Path.Combine(path, "Player.data");
        if (File.Exists(fileName))
        {    
            File.Delete(fileName);    
        }
        PlayerDataModel data = new PlayerDataModel();
        data.GetValues();
        string json = JsonConvert.SerializeObject(data);
        using (StreamWriter sw = File.CreateText(fileName))
        {
            sw.Write(json);
        }
    }

    private static void LoadPlayerData(string path)
    {
        string filePath = Path.Combine(path, "Player.data");
        string jsonString = File.ReadAllText(filePath);
        PlayerDataModel data;
        try
        {
            data = PlayerDataModel.Deserialize(jsonString);
        }
        catch
        {
            throw new Exception("savePlayerData: invalid syntaxe json");
        }
        data.SetValues();
    }
}
