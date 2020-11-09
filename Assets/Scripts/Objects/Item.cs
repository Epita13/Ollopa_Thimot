using Godot;
using System;
using System.Collections.Generic;

public class Item
{
    public static int nbItems = 6;
    public enum Type
    {
        Composite,
        Wood,
        Stone,
        Dirt,
        Sonar,
        Ospirit,
    }

    public static Dictionary<int, Texture> textures = new Dictionary<int, Texture>
    {
        {(int)Type.Composite, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Composite.png")},
        {(int)Type.Wood, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Wood.png")},
        {(int)Type.Stone, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Stone.png")},
        {(int)Type.Dirt, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Dirt.png")},
        {(int)Type.Sonar, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Sonar.png")},
        {(int)Type.Ospirit, GD.Load<Texture>("res://Assets/Ressources/Imgs/Items/Ospirit.png")}
    };
    
    public static List<Type> item2heal = new List<Type>
    {
        Type.Sonar,
        Type.Wood,
        Type.Ospirit,
    };

    public static Dictionary<Type, float> healingPower = new Dictionary<Type, float>
    {
        {Type.Sonar, 40f},
        {Type.Wood, 20f},
        {Type.Ospirit, 50f},
    };
    
    public static List<Type> drillable = new List<Type>
    {
        Type.Stone,
        Type.Sonar,
        Type.Dirt,
        Type.Ospirit
    };
    
    public static List<Type> grindable = new List<Type>
    {
        Type.Wood,
        Type.Stone,
        Type.Dirt,
        Type.Sonar,
        Type.Ospirit,
    };
    
    public static Dictionary<Type, int> ToComposite = new Dictionary<Type, int>
    {
        {Type.Wood, 2},
        {Type.Stone, 3},
        {Type.Dirt, 5},
        {Type.Sonar, 1},
        {Type.Ospirit, 1},
    };
}
