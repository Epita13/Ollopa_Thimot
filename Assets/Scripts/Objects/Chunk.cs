using Godot;
using System;
using System.Collections.Generic;


public class Chunk
{

    /*
        Object :  Chunk

        /!\ Initialisation static : NON NECESSAIRE.
        /!\ Classe Initialisées necessaire : World

        Description de l'object :
            Un Chunk est une partie rectangulaire du monde de taille [size,height] blocks, 
            Ainsi un ensemble chunk consiste a faire une partition du monde.
            Les coordonnées manipulées dans cette classe sont strictement locale, cad x = [0,size-1] et y = [ChunkMin,ChunkMax].
            Chaque Chunk possede une sorte de matrix dynamique de Block ("blocks").

        Description des parametres:
            (static) int size : est la taille en blocks du chunk en x.
            int id : L'id definis la du chunk position dans le monde. (=> [id*size, (id*size)+size[)
            (const) int ChunkMin, ChunkMax : definissent les limites en y (inclusives) des Chunks et donc du Monde.
            (const) int height : est la hauteur du Monde. 
            (const) int seaLevel : est la position y theorique du niveau de la mer.
            (const) int minYGeneration, maxYGeneration : sont les positions y des limites (inclusives) de la generation.
    */



    /* Constante */
    public static readonly int size = 16;
    public int id;

    public const int chunkMax = 100;
    public const int chunkMin = 0;
    public const int height = (chunkMax-chunkMin)+1;
    public const int seaLevel = chunkMin + 20;
    public const int minYGeneration = seaLevel - 5;
    public const int maxYGeneration = seaLevel + 20;


    private List<List<Block>> blocks;

    public Chunk(int id)
    {
        this.id = id;
        Generate();
    }

    public void Generate()
    {

        OpenSimplexNoise noise = World.noise;
        blocks = new List<List<Block>>();
        
        for (int x = 0; x < size; x++)
        {
            int maximumY = GetMaximumY(x, noise);
            blocks.Add(new List<Block>());
            for (int y = chunkMin; y <= chunkMax; y++)
            {
                if (y<=maximumY-6)
                {
                    blocks[x].Add(new Block(Block.Type.Stone, x+(id*size), y, true));
                }else if (y<=maximumY-1)
                {
                    blocks[x].Add(new Block(Block.Type.Dirt, x+(id*size), y, true));
                }else if (y==maximumY)
                {
                    blocks[x].Add(new Block(Block.Type.Grass, x+(id*size), y, true));
                }else
                {
                    blocks[x].Add(new Block(Block.Type.Air, x+(id*size), y));
                }
            }
        }
        
    }
    
    private int GetMaximumY(int x, OpenSimplexNoise noise)
    {
        float rayon = (World.size*size) / (2*Mathf.Pi);
        float step = (2*Mathf.Pi)/ (World.size*size);
        float angle = (x+(id*size))*step;
        Vector2 point = new Vector2(Mathf.Cos(angle)*rayon, Mathf.Sin(angle)*rayon);
        float grayLevel = noise.GetNoise2dv(point) + 1.0f; // [0,2.0]
        float a = (maxYGeneration-minYGeneration)/2; // coeficient directeur
        float b = minYGeneration; // image a l'origine
        return (int)((a*grayLevel)+b);

    }

    /// Affiche le chunk sur la tilemap de la scene
    public void Draw()
    {
        foreach (var colon in blocks)
        {
            foreach (var block in colon)
            {
                DrawBlock(block);
                DrawBlockBack(block);
            }
        }
    }

    /// Cache le chunk du tilemap de la scene
    public void Hide()
    {
        foreach (var colon in blocks)
        {
            foreach (var block in colon)
            {
                HideBlock(block);
                HideBlockBack(block);
            }
        }
    }

    /// Verifie si les coordonées sont correct
    private bool IsInChunk(int x, int y)
    {
        return (x>=0 && x<size && y>=0 && y<=(chunkMax-chunkMin));
    }

    /// Retourne le blocks de la coordonée (coordonées locales). retourne null si il n'y a pas de block
    public Block GetBlock(int x, int y)
    {
        if (!IsInChunk(x,y))
            throw new OutOfBoundsException2D("GetBlock", x, 0, size-1, y, chunkMin, chunkMax);
        return blocks[x][y];
    }

    /// Ajoute un block au Chunk (coordonées locales)
    public void AddBlock(int x, int y, Block.Type type)
    {
        if (!IsInChunk(x,y))
            throw new OutOfBoundsException2D("AddBlock", x, 0, size-1, y, chunkMin, chunkMax);
        blocks[x][y].type = type;
        DrawBlock(blocks[x][y]);
    }

    /// Enleve un block au Chunk (coordonées locales)
    public void RemoveBlock(int x, int y)
    {
        if (!IsInChunk(x,y))
            throw new OutOfBoundsException2D("RemoveBlock", x, 0, size-1, y, chunkMin, chunkMax);
        HideBlock(blocks[x][y]);
        blocks[x][y].type = Block.Type.Air;
    }

    /// Affiche un block au Chunk
    public void DrawBlock(Block b)
    {
        World.IsInitWorldTest("DrawBlock");
        World.BlockTilemap.SetCell(b.x, -b.y+height, Block.GetIDTile(b.type));
    }

    /// Cache un block au Chunk
    public void HideBlock(Block b)
    {
        World.IsInitWorldTest("HideBlock");
        World.BlockTilemap.SetCell(b.x, -b.y+height, Block.GetIDTile(Block.Type.Air));
    }

    public void DrawBlockBack(Block b)
    {
        if (b.isAutoGenerated)
            World.BackBlockTilemap.SetCell(b.x, -b.y+height, 0);
    }
    /// Cache l'arriere d'un block
    public void HideBlockBack(Block b)
    {
        World.BackBlockTilemap.SetCell(b.x, -b.y+height, -1);
    }

    /// Renvoie la position x local a partir de la position x globale (=> modulo la taille d'un chunk)
    public static int GetLocaleX(int x)
    {
        return x % size;
    }

}