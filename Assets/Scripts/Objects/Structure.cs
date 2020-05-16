using Godot;
using System;
using System.Collections.Generic;

public static class Structure
{
    /*Faire des structure de longueur multiple de n - 1 pour faire joli*/

    private static int n = 4;
    public static Vector2 Generate(int x, int lgr)
    {
        int modulo = n;
        if (lgr > World.size * Chunk.size)
            lgr = World.size * Chunk.size;

        if (lgr < modulo)
            modulo = lgr;

        List<Tuple<int,int>> heights = new List<Tuple<int,int>>();
        int Y = 0;
        int nb = 1;
      
      
        for (int i = x; i < x + lgr; i++)
        {
            heights.Add(new Tuple<int, int>(i, GetY(i)));
        }

        foreach (Tuple<int,int> block in heights)
        {
            if (block.Item2 > Y)
                Y = block.Item2;
        }

        Y += 2;

        foreach (Tuple<int,int> block in heights)
        {
            Chunk c = World.GetChunk(block.Item1);
            c.AddBlock(Chunk.GetLocaleX(block.Item1), Y, Block.Type.IronBlock);
        }

        foreach (Tuple<int,int> block in heights)
        {
            if (nb % modulo == 0)
            {
                for (int y = Y - 1; y >= block.Item2; y--)
                {
                    Chunk c = World.GetChunk(block.Item1);
                    c.AddBlock(Chunk.GetLocaleX(block.Item1), y, Block.Type.WarningBlock);
                }
            }
            nb++;
        }

        return new Vector2(x,Y);
    }
   
   
    private static int GetY(int x)
    {
        int y = Chunk.maxYGeneration + 1;
        bool ground = false;

        while (!ground)
        {
            if ((int) World.GetBlock(x, y - 1).type == -1)
                y--;
            else
                ground = true;
        }

        return y;
    }
}