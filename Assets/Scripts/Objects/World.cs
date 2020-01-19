using Godot;
using System;
using System.Collections.Generic;

public static class World
{
    public static TileMap tilemp_blocks;
    public static Random random;

    
    public static int size;
    public static List<Chunk> chunks;

    /// Initialise le monde et le calcule.
    public static void Init(int size, TileMap tilemp_blocks, int seed = -1)
    {
        // Random et seed
        if (seed==-1)
            World.random = new Random();
        else
            World.random = new Random(seed);
        World.size = size;
        World.tilemp_blocks = tilemp_blocks;
        World.chunks = new List<Chunk>();
        World.Generate();
    }
    
    /// Calcule le monde en fonction des parametres.
    private static void Generate()
    {
        Chunk previus_chunk = null;
        for (int x = 0; x < size; x++)
        {
            Chunk instance_chunk = new Chunk(x*Chunk.chunk_size, previus_chunk);
            previus_chunk = instance_chunk;
            chunks.Add(instance_chunk);
        }
    }

    /// Dessine le monde sur la scene.
    public static void Draw()
    {
        foreach (Chunk chunk in chunks)
            chunk.Draw();
    }
}
