using Godot;
using System;
using System.Collections.Generic;

public class UI_breakingBlocks : TileMap
{

    private static UI_breakingBlocks instance;
    public static UI_breakingBlocks GetInstance() => instance;
    
    public static List<Block> damagedBlocks = new List<Block>();
    
    public static List<Block> blocks2delete = new List<Block>();
    public static void RemoveUI(Block b)
    {
        if (damagedBlocks.Contains(b))
        {
            blocks2delete.Add(b);
        }
    }
    
    public static void AddUI(Block b)
    {
        if (!damagedBlocks.Contains(b))
        {
            damagedBlocks.Add(b);
        }
    }
    
    public override void _Ready()
    {
        instance = this;
    }

    public override void _Process(float delta)
    {
        foreach (var b in damagedBlocks)
        {
            b.Heal(Block.healStrength*delta);
            int tile_id = Mathf.FloorToInt(((16 - 23) / Block.durabilities[b.GetType]) * b.health + 23);
            SetCell(b.x, -b.y+Chunk.height, tile_id);
            SetCell(b.x+World.size*Chunk.size, -b.y+Chunk.height, tile_id);
            SetCell(b.x-World.size*Chunk.size, -b.y+Chunk.height, tile_id);
        }

        while (blocks2delete.Count != 0)
        {
            Block b = blocks2delete[0];
            SetCell(b.x, -b.y+Chunk.height, -1);
            SetCell(b.x+World.size*Chunk.size, -b.y+Chunk.height, -1);
            SetCell(b.x-World.size*Chunk.size, -b.y+Chunk.height, -1);
            
            damagedBlocks.Remove(b);
            blocks2delete.Remove(b);
        }

    }
}
