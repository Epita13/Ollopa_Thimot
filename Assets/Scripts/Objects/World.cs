using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public static class World
{



    /*
        Object static:  World

        /!\ Initialisation static : STRICTEMENT NECESSAIRE.
            - Utiliser la fonction Init()
            - Verification d'initialisation : le getter IsInit

         /!\ Classe Initialisées necessaire : None
         
        Description de l'object :
            Le World est une map constituée, de plusieurs chunks formant une partition de celui-ci.
            Ca creation repose sur une generation aleatoire dependant d'un SimplexNoise "noise".
            Le World fait une taille de "size" chunks et une hauteur de Chunk.height
            Les coordonnées manipulées dans cette classe sont strictement globale, cad x = [0,size*Chunk.size-1] et y = [Chunk.ChunkMin,Chunk.ChunkMax].

        Description des parametres:
            Les TileMap :
                - (static) Tilemap BlockTilemap :  principale, pour les blocks possedant des collisions.
            Le SimplexNoise :
                - (static) OpenSimplexNoise noise : (module Godot)
                - (static) int seed : represente la graine du monde (chaque seed represente une generation differente)
                - (const) ... : plus d'information https://godot-es-docs.readthedocs.io/en/latest/classes/class_opensimplexnoise.html
            (static) Random random :  utilisé pour toute utilisation de systems aleatoires.
            (static) int size : represente la taille en Chunk du monde.
            (static) List<Chunk> chunks : est la liste contenant tous la partition du monde, possedant tous les chunk du monde
                
    */



    /*TileMaps*/
    public static TileMap BackBlockTilemap;
    public static TileMap BlockTilemap;
    public static TileMap UIBlockTilemap;
    public static TileMap UI2BlockTilemap;
    /*********/

    public static Random random;

    // SimplexNoise
    public static OpenSimplexNoise noise = new OpenSimplexNoise();
    private static int seed=-1;
    private const int octave = 3;
    private const float periode = 20.0f;
    private const float persistence = 0.1f;
    private const float lacunarity = 3.5f;
    
    public static int size = 10;
    public static List<Chunk> chunks;
    public static List<Chunk> visibleChunks = new List<Chunk>();
    
    public static List<Tree> trees = new List<Tree>();

    private static bool isInit = false;
    public static bool IsInit => isInit;
    public static void IsInitWorldTest(string funcName)
    {
        if (!isInit)
            throw new UninitializedException(funcName, "World");
    }


    public static void SetSize(int size)
    {
        World.size = size;
    }
    public static void SetSeed(int seed)
    {
        World.seed = seed;
    }

    /// Initialise le monde et le calcule.
    public static void Init(TileMap BlockTilemap, TileMap UIBlockTilemap, TileMap UI2BlockTilemap, TileMap BackBlockTilemap, bool generate = true)
    {
        isInit = true;

        World.BlockTilemap = BlockTilemap;
        World.UIBlockTilemap = UIBlockTilemap;
        World.UI2BlockTilemap = UI2BlockTilemap;
        World.BackBlockTilemap = BackBlockTilemap;
        
        if (seed==-1){
            World.random = new Random();
            World.seed = random.Next();
        }else{
            World.random = new Random(seed);
            World.seed = seed;
        }
        
        if (!generate)
            return;
        
        
        if (size < 3)
            throw new OutOfBoundsException1D("Init", size, 3, 9999);
        

        World.chunks = new List<Chunk>();

        // Initialisation du SimplexNoise
        noise.Seed = World.seed;
        noise.Octaves = octave;
        noise.Period = periode;
        noise.Persistence = persistence;
        noise.Lacunarity = lacunarity;

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
        OreGenerate();
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
        while (visibleChunks.Count > 0)
        {
            HideChunkc(visibleChunks[0]);
        }
    }



    /// Retourne le chunk correspondant a la position x
    public static Chunk GetChunk(int x)
    {
        IsInitWorldTest("GetChunk");
        if (x < 0)
            x = size * Chunk.size + x;
        else if (x >= size * Chunk.size)
            x = x - size * Chunk.size;
        return chunks[x/Chunk.size];
    }
    
    /// Retourne le chunk correspondant a l'id
    public static Chunk GetChunkWithID(int id)
    {
        IsInitWorldTest("GetChunkWithID");
        if (id < 0)
            id = size + id;
        else if (id >= size)
            id = id - size;
        return chunks[id];
    }

    /// Retourne le block aux coordonées misent en parametres. retourne null si pas de block
    public static Block GetBlock(int x, int y)
    {
        IsInitWorldTest("GetBlock");
        Chunk c = GetChunk(x);
        if (x < 0)
            x = size * Chunk.size + x;
        else if (x >= size * Chunk.size)
            x = x - size * Chunk.size;
        return c.GetBlock(Chunk.GetLocaleX(x),y);
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


    private static void OreGenerate()
    {
        for (int x = 0; x < World.size * Chunk.size; x++)
        {
            foreach (var ore in Ore.ores)
            {
                float p = (float)random.NextDouble();
                if (p <= Ore.probabilities[ore])
                {
                    int height = random.Next(1, Ore.heights[ore] + 1);
                    if (World.GetBlock(x, height).GetType == Block.Type.Stone)
                    {
                        Ore.CreateVein(ore, x, height);
                    }
                }
            }
        }
    }
    
}
