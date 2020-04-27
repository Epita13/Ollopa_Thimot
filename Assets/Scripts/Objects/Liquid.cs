using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using Thread = System.Threading.Thread;
using Timer = Godot.Timer;

public class Liquid : Node2D
{
    private LiquidMove water;
    private LiquidMove oil;
    public static readonly Dictionary<Liquid.Type, LiquidMove> list = new Dictionary<Type, LiquidMove>();
    public static readonly Dictionary<Liquid.Type, TileMap> listMap = new Dictionary<Type, TileMap>();
    private Timer TimerWater;
    private Timer TimerOil;
    public static TileMap Watermap;
    public static TileMap Oilmap;
    

    public const int NbLiquids = 3;
    public const int Capacity = 8;
    public enum Type 
    { Water, Oil, Fuel }

    private int test = 0;
    public override void _Ready()
    {
        Watermap = GetNode<TileMap>("Watermap");
        Oilmap = GetNode<TileMap>("Oilmap");
        list.Add(Type.Water, new LiquidMove(Type.Water));
        list.Add(Type.Oil, new LiquidMove(Type.Oil));
        listMap.Add(Type.Water, Watermap);
        listMap.Add(Type.Oil, Oilmap);
        TimerWater = GetNode<Timer>("TimerWater");
        TimerOil = GetNode<Timer>("TimerOil");
    }


    public static bool PlaceLiquid(int x, int y, Type type)
    {
        /*Place un block de liquid, renvoie true si possible, false dans tous les autres cas*/
        bool res = false;
        try
        {
            res = list[type].Place(x,y);
        }
        catch (Exception)
        {
            Console.WriteLine("Error, maybe arg out of bounds ? PlaceWater() in Liquid.cs");
            GD.Print("Error, maybe arg out of bounds ? PlaceWater() in Liquid.cs");
        }

        return res;
    }

    public override void _Process(float delta)
    { 
       foreach (KeyValuePair<Type, LiquidMove> liquid in list)
           liquid.Value.CloneWater(GetViewport().Size.x, GetViewportTransform().origin);
    }


    private void TimeOutWater()
    {
        try
        {
             list[Type.Water].Move();
        }
        catch (Exception)
        {
            Console.WriteLine("Unknow error from LiquidMove.cs");
            GD.Print("Unknow error from LiquidMove.cs");
        }
    }
    
    private void TimeOutOil()
    {
        try
        {
            list[Type.Oil].Move();
        }
        catch (Exception)
        {
            Console.WriteLine("Unknow error from LiquidMove.cs");
            GD.Print("Unknow error from LiquidMove.cs");
        }
    }
}
