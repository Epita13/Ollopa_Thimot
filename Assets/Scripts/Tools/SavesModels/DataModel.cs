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
    
    public Dictionary<Liquid.Type, List<((int,int),int)>> liquids { get; set; }
    
    
    public void GetValues()
    {
        size = World.size;
        chunks = World.chunks;
        trees = new List<Tree.SaveStruct>();
        foreach (var tree in World.trees)
        {
            trees.Add(tree.GetSaveStruct());
        }
        
        liquids = new Dictionary<Liquid.Type, List<((int, int), int)>>();
        foreach (var lq in Liquid.list)
        {
            List<((int, int), int)> liquid = new List<((int, int), int)>();
            foreach (var coord in lq.Value.listLiquid)
            {
                liquid.Add(((coord.Item1, coord.Item2), Liquid.list[lq.Key].map[coord.Item1, coord.Item2]));
            }
            liquids.Add(lq.Key,liquid);
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
        
        Liquid.Init();
        foreach (var lq in Liquid.list)
        {
            int[,] map = new int[World.size * Chunk.size + 1, Chunk.height];
            foreach (var values in liquids[lq.Key])
            {
                Liquid.list[lq.Key].listLiquid
                    .Add(new Tuple<int, int>(values.Item1.Item1, values.Item1.Item2));
                map[values.Item1.Item1, values.Item1.Item2] = values.Item2;
            }
            Liquid.list[lq.Key].map = map;
        }
    }

    
    
    public static WorldDataModel Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<WorldDataModel>(json);
    }
}



public class PlayerDataModel
{
    public float healthMax;
    public float health;
    public float oxygeneMax;
    public float oxygene;
    public float energyMax;
    public float energy;

    public Vector2 playerPosition;
    
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
        
        playerPosition = new Vector2(PlayerMouvements.GetX(), PlayerMouvements.GetY());
    
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

        PlayerMouvements.initialPosition = playerPosition;
        
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
