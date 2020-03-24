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

    public const int NbLiquids = 3;
    public const int Capacity = 8;
    public enum Type 
    { Water, Oil, Fuel }
    
    public override void _Ready()
    {
        /*water = new LiquidMove(Type.Water);
        oil = new LiquidMove(Type.Oil);
        list.Add(Type.Water, water);
        list.Add(Type.Oil, oil);
        TimerWater = GetNode<Timer>("TimerWater");
        TimerOil = GetNode<Timer>("TimerOil");*/
        //TimerWater.Connect("timeout()", GetNode("Liquid"), "TimeOutWater");

    }


    public void Placewater(int x, int y, Liquid.Type type)
    {
        list[type].PlaceWater(x,y);
    }

    private void TimeOutWater()
    {
        /*list[Type.Water].Move();
        list[Type.Water].PlaceWater(10, 80);*/
    }
    
    private void TimeOutOil()
    {
       // list[Type.Oil].Move();
    }
}
