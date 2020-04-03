using Godot;
using System;
using System.Collections.Generic;

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


    public void Placewater(int x, int y, Liquid.Type type)
    {
        list[type].PlaceWater(x,y);
    }

    private void TimeOutWater()
    {
        if (test < 3)
        {
           list[Type.Water].PlaceWater(10, 39);
           test++;
        }
        list[Type.Water].Move();
    }
    
    private void TimeOutOil()
    {
        //list[Type.Oil].Move();
    }
}
