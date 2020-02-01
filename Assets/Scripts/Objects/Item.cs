using Godot;
using System;
using System.Collections.Generic;

public class Item
{
    public static int nbItems = 4;
    public enum Type
    {
        Composite,
        Wood,
        Stone,
        Dirt
    }

    public static Dictionary<int, Texture> textures = new Dictionary<int, Texture>
    {
        {(int)Type.Composite, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Composite.png")},
        {(int)Type.Wood, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Wood.png")},
        {(int)Type.Stone, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Stone.png")},
        {(int)Type.Dirt, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Wood.png")}
    };

}
