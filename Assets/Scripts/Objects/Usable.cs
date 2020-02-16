using Godot;
using System;
using System.Collections.Generic;

public class Usable
{
    public static int nbUsables = 4;
    public enum Type{
        Laser,
        Dirt,
        Grass,
        Stone
    }

    public enum Category
    {
        Tool,
        Block
    }

    public static Dictionary<int, Texture> textures = new Dictionary<int, Texture>
    {
        {(int)Type.Laser, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/laser.png")},
        {(int)Type.Dirt, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/dirt.png")},
        {(int)Type.Grass, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/grass.png")},
        {(int)Type.Stone, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/stone.png")}
    };

    public static Dictionary<Type, Block.Type> blocks = new Dictionary<Type, Block.Type>
    {
        {Type.Dirt, Block.Type.Dirt},
        {Type.Grass, Block.Type.Grass},
        {Type.Stone, Block.Type.Stone}
    };

    public static Dictionary<int, Category> category = new Dictionary<int, Category>
    {
        {(int)Type.Laser, Category.Tool},
        {(int)Type.Dirt, Category.Block},
        {(int)Type.Grass, Category.Block},
        {(int)Type.Stone, Category.Block}
    };

}
