using Godot;
using System;
using System.Collections.Generic;


public class Chunk
{
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
                    blocks[x].Add(new Block(Block.Type.Stone, x+(id*size), y));
                }else if (y<=maximumY-1)
                {
                    blocks[x].Add(new Block(Block.Type.Dirt, x+(id*size), y));
                }else if (y==maximumY)
                {
                    blocks[x].Add(new Block(Block.Type.Grass, x+(id*size), y));
                }else
                {
                    blocks[x].Add(new Block(Block.Type.Air, x+(id*size), y));
                }
            }
        }
        
    }

    /// Affiche le chunk sur le tilemap de la scene
    public void Draw()
    {
        TileMap Ground = World.tilemp_blocks;
        foreach (var colon in blocks)
        {
            foreach (var block in colon)
            {
                DrawBlock(block);
            }
        }
    }

    /// Cache le chunk du tilemap de la scene
    public void Hide()
    {
        TileMap Ground = World.tilemp_blocks;
        foreach (var colon in blocks)
        {
            foreach (var block in colon)
            {
                HideBlock(block);
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

    /// Retourne le blocks de la coordonée (coordonées locales). retourne null si il n'y a pas de block
    public Block GetBlock(int x, int y)
    {
        if (x<0 || x>=size || y<0 || y>(chunkMax-chunkMin))
            return null;
        return blocks[x][y];
    }

    /// Ajoute un block au Chunk (coordonées locales)
    public void AddBlock(int x, int y, Block.Type type)
    {
        if (x<0 || x>=size || y<0 || y>(chunkMax-chunkMin))
            return;
        blocks[x][y] = new Block(type, x+(id*size), y);
        DrawBlock(blocks[x][y]);
    }

    /// Enleve un block au Chunk (coordonées locales)
    public void RemoveBlock(int x, int y)
    {
        if (x<0 || x>=size || y<0 || y>(chunkMax-chunkMin))
            return;
        HideBlock(blocks[x][y]);
        blocks[x][y] = new Block(Block.Type.Air, x+(id*size), y);
    }

    /// Affiche un block au Chunk
    public void DrawBlock(Block b)
    {
        World.tilemp_blocks.SetCell(b.x, -b.y+height, Block.GetIDTile(b.type));
    }

    /// Cache un block au Chunk
    public void HideBlock(Block b)
    {
        World.tilemp_blocks.SetCell(b.x, -b.y+height, Block.GetIDTile(Block.Type.Air));
    }

    public static int GetLocaleX(int x)
    {
        return x % size;
    }

}