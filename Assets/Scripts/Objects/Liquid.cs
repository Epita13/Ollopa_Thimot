using Godot;
using System;
using System.Collections.Generic;

public class Liquid : Node2D
{
    private LiquidMove water;
    private LiquidMove oil;
    private readonly Dictionary<Liquid.Type, LiquidMove> list = new Dictionary<Type, LiquidMove>();
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
        TimerWater = GetNode<Timer>("TimerWater");
        TimerOil = GetNode<Timer>("TimerOil");
    }


    public void Placewater(int x, int y, Liquid.Type type)
    {
        list[type].PlaceWater(x,y);
    }

    private void TimeOutWater()
    {
        if (test < 50)
        {
           list[Type.Water].PlaceWater(10, 60);
           test++;
        }
        list[Type.Water].Move();
        /// intégrer si ca dépasse la hauteur du chunk
    }
    
    private void TimeOutOil()
    {
        //list[Type.Oil].Move();
    }
}
