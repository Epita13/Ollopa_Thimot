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
    private Thread waterMove;
    private Thread oilMove;
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
        waterMove = new Thread(list[Type.Water].Move);
        oilMove = new Thread(list[Type.Oil].Move);
    }


    public void Placewater(int x, int y, Liquid.Type type)
    {
        try
        {
            list[type].PlaceWater(x,y);
        }
        catch (Exception )
        {
            Console.WriteLine("Error, maybe arg out of bounds ? PlaceWater() in Liquid.cs");
            GD.Print("Error, maybe arg out of bounds ? PlaceWater() in Liquid.cs");
        }
        
    }

    private void TimeOutWater()
    {
        if (test < 50 && !waterMove.IsAlive)
        {
            Placewater(test, 40, Type.Water);
            test++;
        }
        
        if (!waterMove.IsAlive)
        {
            waterMove = new Thread(list[Type.Water].Move);
            try
            {
                waterMove.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("Unknow error from LiquidMove.cs");
                GD.Print("Unknow error from LiquidMove.cs");
            }
        }
    }
    
    private void TimeOutOil()
    {
        if (!oilMove.IsAlive)
        {
            oilMove = new Thread(list[Type.Oil].Move);
            try
            {
                oilMove.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("Unknow error from LiquidMove.cs");
                GD.Print("Unknow error from LiquidMove.cs");
            }
        }
    }
}
