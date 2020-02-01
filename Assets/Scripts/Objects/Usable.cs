using Godot;
using System;
using System.Collections.Generic;

public class Usable
{
    public static int nbUsables = 2;
    public enum Type{
        laser,
        dirt
    }

    public enum Category
    {
        Tool,
        Block
    }

    public static Dictionary<int, Texture> textures = new Dictionary<int, Texture>
    {
        {(int)Type.laser, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/laser.png")},
        {(int)Type.dirt, GD.Load<Texture>("res://Assets/Ressources/Imgs/Usables/dirt.png")}
    };

    public static Dictionary<int, Category> category = new Dictionary<int, Category>
    {
        {(int)Type.laser, Category.Tool},
        {(int)Type.dirt, Category.Block}
    };

}
