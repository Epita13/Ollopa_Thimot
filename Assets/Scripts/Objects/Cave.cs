using Godot;
using System;

public class Cave
{
    private static double initchance = 0.432F;
    private static int xmax = World.size * Chunk.size;
    private static int ymax = Chunk.maxYGeneration;
    private static int deathlimit = 4;
    private static int birthlimit = 4;
    private static int numberOfSteps = 7;

    public static bool[,] InitBasicCave()
    {
        Random rand = new Random();
        bool[,] map = new bool[xmax,ymax];
        for (int i = 0; i < xmax; i++)
        {
            for (int j = 0; j < ymax; j++)
            {
                if (rand.NextDouble() < initchance)
                    map[i, j] = true;
            }
        }
        
        return map;
    }
    
    public static bool[,] InitCave()
    {
        bool[,] map = InitBasicCave();

        for (int i = 0; i < numberOfSteps; i++)
        {
            map = DoGeneration(map);
        }

        return map;
    }

    private static int CountNeighbours(bool[,] map,int x,int y)
    {
        int res = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbour_x = x+i;
                int neighbour_y = y+j;

                if (!(i == 0 && j == 0))
                {
                    if(neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= xmax || neighbour_y >= ymax)
                        res += 1;
                    else if (map[neighbour_x, neighbour_y])
                        res += 1;
                }
            }
        }
        return res;
    }

    public static bool[,] DoGeneration(bool[,] map)
    {
        bool[,] res = new bool[xmax,ymax];
        for (int i = 0; i < xmax; i++)
        {
            for (int j = 0; j < ymax; j++)
            {
                int nb = CountNeighbours(map, i, j);
                if (map[i, j])
                {
                    if (nb < deathlimit)
                    {
                        res[i, j] = false;
                    }
                    else
                    {
                        res[i, j] = true;
                    }
                }
                else
                {
                    if (nb < birthlimit)
                    {
                        res[i, j] = false;
                    }
                    else
                    {
                        res[i, j] = true;
                    }
                }
            }
        }
        return res;
    }
}
