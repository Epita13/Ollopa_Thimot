using Godot;
using System;
using System.Collections.Generic;

public static class World
{
    public static TileMap tilemp_blocks;
    public static Random random;

    // SimplexNoise
    public static OpenSimplexNoise noise = new OpenSimplexNoise();
    public static int seed;
    public const int octave = 3;
    public const float periode = 20.0f;
    public const float persistence = 0.25f;
    public const float lacunarity = 3.5f;
    
    // size en nombre de chunks
    public static int size;
    public static List<Chunk> chunks;

    /// Initialise le monde et le calcule.
    public static void Init(int size, TileMap tilemp_blocks, int seed = -1)
    {
        // Random et seed
        if (seed==-1){
            World.random = new Random();
            World.seed = random.Next();
        }else{
            World.random = new Random(seed);
            World.seed = seed;
        }

        World.size = size;
        World.tilemp_blocks = tilemp_blocks;
        World.chunks = new List<Chunk>();

        // Initialisation du SimplexNoise
        noise.SetSeed(World.seed);
        noise.SetOctaves(octave);
        noise.SetPeriod(periode);
        noise.SetPersistence(persistence);
        noise.SetLacunarity(lacunarity);

        World.Generate();
    }
    
    /// Calcule le monde en fonction des parametres.
    private static void Generate()
    {
        for (int x = 0; x < size; x++)
        {
            Chunk instance_chunk = new Chunk(x);
            chunks.Add(instance_chunk);
        }
    }

    /// Affiche tous les Chunks de la map
    public static void Draw()
    {
        foreach (Chunk chunk in chunks)
            DrawChunkc(chunk);
    }

    /// Cache tous les Chunks de la map
    public static void Hide()
    {
        foreach (Chunk chunk in chunks)
            HideChunkc(chunk);
    }

    /// Retourne le chunk correspondant a la position x
    public static Chunk GetChunk(int x)
    {
        if (x<0 || x>=size*Chunk.size)
            return null;
        return chunks[x/Chunk.size];
    }
    /// Retourne le chunk correspondant a la position x
    public static Chunk GetChunkv(Vector2 location)
    {
        location = new Vector2(Mathf.Floor(location.x), Mathf.Floor(location.y));
        int x = (int)location.x;
        if (x<0 || x>=size*Chunk.size)
            return null;
        return chunks[x/Chunk.size];
    }

    /// Retourne le block aux coordonées misent en parametres. retourne null si pas de block
    public static Block GetBlock(int x, int y)
    {
        Chunk c = GetChunk(x);
        if (c==null)
            return null;
        return c.GetBlock(x%Chunk.size,y);
    }
    /// Retourne le block aux coordonées misent en parametres. retourne null si pas de block
    public static Block GetBlockv(Vector2 location)
    {
        location = new Vector2(Mathf.Floor(location.x), Mathf.Floor(location.y));
        int x = (int)location.x;
        int y = (int)location.y;
        Chunk c = GetChunk(x);
        if (c==null)
            return null;
        return c.GetBlock(x%Chunk.size,y);
    }

    /// Verifie si il y a un block
    public static bool HasBlock(int x, int y)
    {
        Block b = GetBlock(x,y);
        return (b!=null);
    }
    /// Verifie si il y a un block
    public static bool HasBlockv(Vector2 location)
    {
        Block b = GetBlockv(location);
        return (b!=null);
    }

    /// Cache le chunk d'id id
    public static void HideChunk(int id)
    {
        chunks[id].Hide();
    }
    /// Affiche le chunk d'id id
    public static void DrawChunk(int id)
    {
        chunks[id].Draw();
    }
    /// Cache le chunk d'id id
    public static void HideChunkc(Chunk c)
    {
        c.Hide();
    }
    /// Affiche le chunk d'id id
    public static void DrawChunkc(Chunk c)
    {
        c.Draw();
    }
}
