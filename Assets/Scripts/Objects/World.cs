using Godot;
using System;
using System.Collections.Generic;

public class World
{
    public static TileMap Tileset_Ground;
    public static Random random;

    
    private int world_size;
    private List<Chunk> chunks;


    public World(int world_size)
    {
        random = new Random();
        this.world_size = world_size;
        chunks = new List<Chunk>();
    }

    public void InitTilemaps(TileMap Ground)
    {
        Tileset_Ground = Ground;
    }

    public void CreateWorld()
    {
        Chunk previus_chunk = null;
        for (int x = 0; x < world_size; x++)
        {
            Chunk instance_chunk = new Chunk(x*16, previus_chunk);
            instance_chunk.Generate();
            previus_chunk = instance_chunk;
            chunks.Add(instance_chunk);
        }
    }
}
