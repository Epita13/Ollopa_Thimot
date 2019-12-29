using Godot;
using System;
using System.Collections.Generic;


public class Chunk
{
    /* Constante */
    private const int chunk_size = 16;
    private const int chunk_max = 140;
    private const int chunk_min = -40;
    private const int sea_level = 0;
    private const float mult_amplified = 0.7F;
    private const int chunk_minLimit_generation = -2;


    private int chunk_mid;
    private List<List<Block>> blocks;
    private int chunk_x;

    private Chunk chunk_left;
    private Chunk chunk_right;

    public Chunk(int chunk_x, Chunk chunk_left = null, Chunk chunk_right = null){
        this.chunk_x = chunk_x;
        this.chunk_left = chunk_left;
        this.chunk_right = chunk_right;
        this.chunk_mid = GetChunkYGround();
        blocks = InitBlocksList();
    }

    public void Generate()
    {

        FonctionCourbe courbe = null;
        courbe = new FonctionCourbe(chunk_size, IsInvert(), chunk_mid, chunk_minLimit_generation, mult_amplified);

        for (int x = 0; x < chunk_size; x++)
        {
            int ground_colon = courbe.Get(x);
            for (int y = chunk_min; y <= chunk_max; y++)
            {
                /*Temporaire*/
                if (y == ground_colon)
                    blocks[x].Add(new Block(1,x+chunk_x,y));
                else if (y < ground_colon && y>ground_colon-4)
                    blocks[x].Add(new Block(2,x+chunk_x,y));
                else if (y <= ground_colon-4){
                    //draw cell
                    blocks[x].Add(new Block(0,x+chunk_x,y));
                }
            }
        }
        DrawCells();
    }

    private void DrawCells()
    {
        TileMap Ground = World.Tileset_Ground;
        foreach (var colon in blocks)
        {
            foreach (var block in colon)
            {
                Ground.SetCell(block.x, -block.y, block.tileId);
            }
        }
    }


    private int GetChunkYGround()
    {
        if (chunk_left==null && chunk_right==null)
            return sea_level + 5;
        if (chunk_left!=null)
            return chunk_left.GetRightMaxY();
        return chunk_right.GetLeftMaxY();
    }

    private List<List<Block>> InitBlocksList()
    {
        List<List<Block>> tmp = new List<List<Block>>();
        for (int i = 0; i < chunk_size; i++){
            tmp.Add(new List<Block>());
        }
        return tmp;
    }
    private bool IsInvert()
    {
        return chunk_right!=null;
    }

    private int GetMaxYColon(int x)
    {   
        List<Block> lst = blocks[x];
        return lst[lst.Count-1].y;
    }
    public int GetLeftMaxY()
    {
        return GetMaxYColon(0);
    }
    public int GetRightMaxY()
    {
        return GetMaxYColon(chunk_size-1);
    }

}


class FonctionCourbe
{
        float distance; // Distance en radian
        float begin; // Debut en radian
        float mult; // multiplicateur en fonction de la distance
        double step;
        int chunk_mid;

        int chunk_size; // taille en x du chunk
        int min_generation;
    public FonctionCourbe(int chunk_size, bool invert, int chunk_theoric_mid, int min_generation, float mult_amplified)
    {
        this.chunk_size = chunk_size;
        this.min_generation = min_generation;
        do {
        distance = (float)RandomRange(Mathf.Pi/4, Mathf.Pi);
        begin = (float)(RandomRange(0,2*Mathf.Pi));
        //mult = (float)((16/(3*Math.PI)) * distance + (14/3)) * mult_amplified;
        mult = (float)((16/(3*Math.PI)) * distance + (14/3)) * mult_amplified;
        step = distance / chunk_size;
        if (!invert)
            chunk_mid = chunk_theoric_mid - (int)(Math.Sin((0*step)+begin) * mult);
        else
            chunk_mid = chunk_theoric_mid - (int)(Math.Sin(((chunk_size-1)*step)+begin) * mult);
        } while (!isGood());
    }
    private bool isGood()
    {
        if (GetMidHeight() < min_generation)
            return false;
        return true;
    }
    public int GetMidHeight()
    {
        return (Get(0) + Get(chunk_size-1)) / 2;
    }
    public int Get(int x) 
    {
        return (int)(Math.Sin((x*step)+begin) * mult) + chunk_mid;   
    }
    /* Genere un double aleatoire entre min et max inclus */
    private double RandomRange(double min, double max)
    {
        Random random = World.random;
        double dif = max - min;
        return (random.NextDouble() * dif + min);
    }
}
