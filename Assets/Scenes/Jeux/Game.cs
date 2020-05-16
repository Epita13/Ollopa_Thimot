using Godot;
using System;

public class Game : Node2D
{

    public static bool load = false;
    public static string saveName = "";

    public static Node2D root;
    public static float WorldScreenSizeX;
    

    public override void _EnterTree()
    {
        Camera2D camera = GetNode<Camera2D>("Player/Camera2D");
        CurrentCamera.Init(camera);
        root = this;
        WorldScreenSizeX = GetViewport().Size.x * CurrentCamera.GetXZoom();
        if (!load)
        {
            TileMap back = GetNode("Tilemaps").GetNode<TileMap>("0");
            TileMap ground = GetNode("Tilemaps").GetNode<TileMap>("1");
            TileMap uiground = GetNode("Tilemaps").GetNode<TileMap>("3");
            TileMap uiground2 = GetNode("Tilemaps").GetNode<TileMap>("2");
            Building.Init(this);
            Loot.Init(this);
            Tree.Init(this);
            BuildingInterface.Init(GetNode("CanvasLayer"));
            World.SetSize(50);
            Liquid.Init();
            World.Init(ground, uiground, uiground2, back);
            SpaceShip.Init();
            InitialiseIverntories();
        }
        else
        {
            TileMap back = GetNode("Tilemaps").GetNode<TileMap>("0");
            TileMap ground = GetNode("Tilemaps").GetNode<TileMap>("1");
            TileMap uiground = GetNode("Tilemaps").GetNode<TileMap>("3");
            TileMap uiground2 = GetNode("Tilemaps").GetNode<TileMap>("2");
            Loot.Init(this);
            Tree.Init(this);
            Building.Init(this);
            BuildingInterface.Init(GetNode("CanvasLayer"));
            World.Init(ground, uiground, uiground2, back, false);
            Save._Load(saveName);
            SpaceShip.Init();
        }
    }


    private void InitialiseIverntories()
    {
        Player.inventoryUsables.Add(Usable.Type.Dirt, 30);
        Player.inventoryUsables.Add(Usable.Type.Grass, 30);
        Player.inventoryUsables.Add(Usable.Type.Stone, 300);
        Player.inventoryBuildings.Add(Building.Type.SolarPanel, 100000);
        Player.inventoryBuildings.Add(Building.Type.Storage, 3);
        Player.inventoryBuildings.Add(Building.Type.Printer3D, 3);
        Player.inventoryBuildings.Add(Building.Type.Compactor, 3);
        Player.inventoryItems.Add(Item.Type.Composite, 120);
        Player.inventoryBuildings.Add(Building.Type.Infirmary, 1);
        Player.inventoryBuildings.Add(Building.Type.O2Generator, 3);
        Player.inventoryItems.Add(Item.Type.Sonar, 1000);
    }


    public static float GetScreenMinX() => PlayerMouvements.GetX() - (Convertion.Location2World(new Vector2(Game.WorldScreenSizeX/2, 0))).x;
    public static float GetScreenMaxX() => PlayerMouvements.GetX() + (Convertion.Location2World(new Vector2(Game.WorldScreenSizeX/2, 0))).x;

}
