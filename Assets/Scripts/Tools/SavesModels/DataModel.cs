using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;







public class EnvironementDataModel 
{
  public float time { get; set; }
  public List<float> sunPowerhistory { get; set; }

  public void GetValues()
  {
      time = Environement.time;
      sunPowerhistory = Environement.sunPowerhistory;
  }

  public void SetValues()
  {
      Environement.time = time;
      Environement.sunPowerhistory = sunPowerhistory;
  }

  public static EnvironementDataModel Deserialize(string json)
  {
      return JsonConvert.DeserializeObject<EnvironementDataModel>(json);
  }
}








public class WorldDataModel
{
    public int size { get; set; }
    public List<Chunk> chunks { get; set; }
    public List<Tree.SaveStruct> trees { get; set; } 
    
    
    public void GetValues()
    {
        size = World.size;
        chunks = World.chunks;
        trees = new List<Tree.SaveStruct>();
        foreach (var tree in World.trees)
        {
            trees.Add(tree.GetSaveStruct());
        }
    }

    public void SetValues()
    {
        World.size = size;
        World.chunks = chunks;
        foreach (var tree in World.trees)
        {
            tree.QueueFree();
        }
        World.trees.Clear();
        foreach (var saveTree in trees)
        {
            Tree t = (Tree)GD.Load<PackedScene>("res://Assets/Objects/Autres/Tree/Tree.tscn").Instance();
            World.trees.Add(t);
            t.treeNumber = saveTree.treeNumber;
            t.treeSize = saveTree.treeSize;
            t.Place((int)saveTree.location.x, (int)saveTree.location.y);
        }
    }

    
    
    public static WorldDataModel Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<WorldDataModel>(json);
    }
}



public class PlayerDataModel
{
    public static float healthMax;
    public static float health;
    public static float oxygeneMax;
    public static float oxygene;
    public static float energyMax;
    public static float energy;
    
    public Dictionary<Usable.Type, int> storageUsables;
    public Dictionary<Item.Type, int> storageItems;
    public Dictionary<Liquid.Type, float> storageLiquids;
    public Dictionary<Building.Type, int> storageBuilding;
    
    public void GetValues()
    {

        healthMax = Player.healthMax;
        health = Player.health;
        oxygeneMax = Player.oxygeneMax;
        oxygene = Player.oxygene;
        energyMax = Player.energyMax;
        energy = Player.energy;
    
        storageUsables = Player.inventoryUsables.stokage;
        storageItems = Player.inventoryItems.stokage;
        storageLiquids = Player.inventoryLiquids.stokage;
        storageBuilding = Player.inventoryBuildings.storage;
    }
    
    public void SetValues()
    {
        Player.healthMax = healthMax;
        Player.health = health;
        Player.oxygeneMax = oxygeneMax;
        Player.oxygene = oxygene;
        Player.energyMax = energyMax;
        Player.energy = energy;
        
        Player.inventoryUsables.stokage = storageUsables;
        Player.inventoryItems.stokage = storageItems;
        Player.inventoryLiquids.stokage = storageLiquids;
        Player.inventoryBuildings.storage = storageBuilding;
    }
    
    public static PlayerDataModel Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<PlayerDataModel>(json);
    }
}