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
    public static Dictionary<Liquid.Type, LiquidMove> list = new Dictionary<Type, LiquidMove>();
    public static Dictionary<Liquid.Type, TileMap> listMap = new Dictionary<Type, TileMap>();
    private Timer TimerWater;
    private Timer TimerOil;
    public static TileMap Watermap;
    public static TileMap Oilmap;
    
    
    public const int nbLiquids = 3;
    public const int Capacity = 8;
    public enum Type 
    { Water, Oil, Fuel }
    public static Dictionary<Type, float> density = new Dictionary<Type, float>{{Type.Water, 0.7f}, {Type.Oil, 0.3f}, {Type.Fuel, 0.3f}};

    private int test = 0;
    public static void Init()
    {
        list.Add(Type.Water, new LiquidMove(Type.Water));
        list.Add(Type.Oil, new LiquidMove(Type.Oil));
    }
    public override void _EnterTree()
    {
        Watermap = GetNode<TileMap>("Watermap");
        Oilmap = GetNode<TileMap>("Oilmap");
        TimerWater = GetNode<Timer>("TimerWater");
        TimerOil = GetNode<Timer>("TimerOil");
        listMap.Add(Type.Water, Watermap);
        listMap.Add(Type.Oil, Oilmap);
    }


    public static bool PlaceLiquid(int x, int y, Type type)
    {
        /*Place un block de liquid, renvoie true si possible, false dans tous les autres cas*/
        bool res = false;
        try
        {
            res = list[type].Place(x,y);
        }
        catch (Exception e)
        {
            GD.Print(e.Message);
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
        catch (Exception e)
        {
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
