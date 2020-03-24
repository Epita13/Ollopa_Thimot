using Godot;
using System;

public class Game : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TileMap back = GetNode("Tilemaps").GetNode<TileMap>("0");
        TileMap ground = GetNode("Tilemaps").GetNode<TileMap>("1");
        TileMap uiground = GetNode("Tilemaps").GetNode<TileMap>("3");
        TileMap uiground2 = GetNode("Tilemaps").GetNode<TileMap>("2");
        World.Init(20, ground, uiground, uiground2, back);
        World.Draw();
        Building.Init(this);
        Loot.Init(this);
    }


}
