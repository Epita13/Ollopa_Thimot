using Godot;
using System;
using System.Collections.Generic;

public class Usable
{
    public static int nbUsables = 7;
    public enum Type{
        Laser,
        Dirt,
        Grass,
        Stone,
        BedRock,
        WarningBlock,
        IronBlock,
    }

    public enum Category
    {
        Tool,
        Block
    }

    public static Dictionary<int, Texture> textures = new Dictionary<int, Texture>
    {
        {(int)Type.Laser, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/Tools/Raygun/Raygun.png")},
        {(int)Type.Dirt, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/Blocks/dirt.png")},
        {(int)Type.Grass, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/Blocks/grass.png")},
        {(int)Type.Stone, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/Blocks/stone.png")},
        {(int)Type.BedRock, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/Blocks/BedRock.png")},
        {(int)Type.WarningBlock, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/Blocks/warning_block.png")},
        {(int)Type.IronBlock, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/Blocks/iron_block.png")}
    };

    public static Dictionary<Type, Block.Type> blocks = new Dictionary<Type, Block.Type>
    {
        {Type.Dirt, Block.Type.Dirt},
        {Type.Grass, Block.Type.Grass},
        {Type.Stone, Block.Type.Stone},
        {Type.BedRock, Block.Type.BedRock},
        {Type.WarningBlock, Block.Type.WarningBlock},
        {Type.IronBlock, Block.Type.IronBlock}
    };

    public static Dictionary<int, Category> category = new Dictionary<int, Category>
    {
        {(int)Type.Laser, Category.Tool},
        {(int)Type.Dirt, Category.Block},
        {(int)Type.Grass, Category.Block},
        {(int)Type.Stone, Category.Block},
        {(int)Type.BedRock, Category.Block},
        {(int)Type.WarningBlock, Category.Block},
        {(int)Type.IronBlock, Category.Block}
    };

}
