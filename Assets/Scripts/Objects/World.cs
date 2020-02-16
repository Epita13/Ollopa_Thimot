using Godot;
using System;
using System.Collections.Generic;

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
    /*********/

    public static Random random;

    // SimplexNoise
    public static OpenSimplexNoise noise = new OpenSimplexNoise();
    private static int seed;
    private const int octave = 3;
    private const float periode = 20.0f;
    private const float persistence = 0.25f;
    private const float lacunarity = 3.5f;
    
    public static int size;
    private static List<Chunk> chunks;

    private static bool isInit = false;
    public static bool IsInit => isInit;
    public static void IsInitWorldTest(string funcName)
    {
        if (!isInit)
            throw new UninitializedException(funcName, "World");
    } 


    /// Initialise le monde et le calcule.
    public static void Init(int size, TileMap BlockTilemap, TileMap UIBlockTilemap, TileMap BackBlockTilemap, int seed = -1)
    {
        isInit = true;
        if (seed==-1){
            World.random = new Random();
            World.seed = random.Next();
        }else{
            World.random = new Random(seed);
            World.seed = seed;
        }

        World.size = size;
        World.BlockTilemap = BlockTilemap;
        World.UIBlockTilemap = UIBlockTilemap;
        World.BackBlockTilemap = BackBlockTilemap;
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
        IsInitWorldTest("GetChunk");
        if (x<0 || x>=size*Chunk.size)
            throw new OutOfBoundsException1D("GetChunk", x, 0, size*Chunk.size-1);
        return chunks[x/Chunk.size];
    }
    /// Retourne le chunk correspondant a l'id
    public static Chunk GetChunkWithID(int id)
    {
        IsInitWorldTest("GetChunkWithID");
        if (id<0 || id>=size)
            throw new ArgumentException("GetChunkWithID: id is out of bounds.");
        return chunks[id];
    }

    /// Retourne le block aux coordonées misent en parametres. retourne null si pas de block
    public static Block GetBlock(int x, int y)
    {
        IsInitWorldTest("GetBlock");
        Chunk c = GetChunk(x);
        return c.GetBlock(x%Chunk.size,y);
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
