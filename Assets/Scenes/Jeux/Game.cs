using Godot;
using System;

public class Game : Node2D
{

    public static bool load = false;
    public static string saveName = "";


    public override void _EnterTree()
    {
        if (!load)
        {
            TileMap back = GetNode("Tilemaps").GetNode<TileMap>("0");
            TileMap ground = GetNode("Tilemaps").GetNode<TileMap>("1");
            TileMap uiground = GetNode("Tilemaps").GetNode<TileMap>("3");
            TileMap uiground2 = GetNode("Tilemaps").GetNode<TileMap>("2");
            Camera2D camera = GetNode<Camera2D>("Player/Camera2D");
            Building.Init(this);
            Loot.Init(this);
            Tree.Init(this);
            CurrentCamera.Init(camera);
            BuildingInterface.Init(GetNode("CanvasLayer"));
            World.SetSize(10);
            World.Init(ground, uiground, uiground2, back);
        }
        else
        {
            TileMap back = GetNode("Tilemaps").GetNode<TileMap>("0");
            TileMap ground = GetNode("Tilemaps").GetNode<TileMap>("1");
            TileMap uiground = GetNode("Tilemaps").GetNode<TileMap>("3");
            TileMap uiground2 = GetNode("Tilemaps").GetNode<TileMap>("2");
            Camera2D camera = GetNode<Camera2D>("Player/Camera2D");
            Loot.Init(this);
            Tree.Init(this);
            Building.Init(this);
            CurrentCamera.Init(camera);
            BuildingInterface.Init(GetNode("CanvasLayer"));
            World.Init(ground, uiground, uiground2, back, false);
            Save._Load(saveName);
        }
    }


}
