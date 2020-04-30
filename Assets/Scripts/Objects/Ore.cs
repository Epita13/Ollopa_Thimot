using Godot;
using System;
using System.Collections.Generic;

public class Ore
{

    public static List<(float, string)> veins = new List<(float, string)>
    {
        (0.1f, "000,0XX,000"),
        (0.2f, "0X0,0X0,000"),
        (0.3f, "0X0,0X0,X00"),
        (0.4f, "0XX,0XX,000"),
        (0.5f, "0X0,0XX,000"),
        (0.6f, "0XX,0XX,XX0"),
        (0.7f, "0XX,XXX,XX0"),
        (0.8f, "0XX,X00,XXX"),
        (1.0f, "XX0,0X0,000")
    };
    
    public static List<Block.Type> ores = new List<Block.Type>
    {
        Block.Type.SonarOre,
        Block.Type.OspiritOre
    };
    
    public static Dictionary<Block.Type, float> probabilities = new Dictionary<Block.Type, float>
    {
        {Block.Type.SonarOre, 0.15f},
        {Block.Type.OspiritOre, 0.02f}
    };
    
    public static Dictionary<Block.Type, int> heights = new Dictionary<Block.Type, int>
    {
        {Block.Type.SonarOre, 15},
        {Block.Type.OspiritOre, 10}
    };


    public static void CreateVein(Block.Type type, int x, int y)
    {
        float p = (float)World.random.NextDouble();
        string vein = "";
        foreach (var couple in veins)
        {
            if (p <= couple.Item1)
            {
                vein = couple.Item2;
                break;
            }
        }
        string[] split = vein.Split(",");
        int offsety = -1;
        foreach (var line in split)
        {
            int offsetx = -1;
            foreach (var c in line)
            {
                if (c == 'X')
                {
                    if (World.GetBlock(x + offsetx, y + offsety).GetType == Block.Type.Stone)
                    {
                        Chunk chunk = World.GetChunk(x + offsetx);
                        chunk.AddBlock(Chunk.GetLocaleX(x+offsetx), y+offsety, type);
                    }
                }
                offsetx += 1;
            }
            offsety += 1;
        }
    }

}
