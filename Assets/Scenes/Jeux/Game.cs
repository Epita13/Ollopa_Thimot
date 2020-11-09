using Godot;
using System;

public class Game : Node2D
{

    public const string VERSION = "1.1";
    
    public static bool load = false;
    public static string saveName = "";

    public static Node2D root;
    public static float WorldScreenSizeX;

    public static float timePlayed = 0;

    public static string GetTimePlayedString()
    {
        int hours = Mathf.FloorToInt(timePlayed/3600);
        int minutes = Mathf.FloorToInt((timePlayed%3600)/60);
        int seconds = Mathf.FloorToInt(timePlayed % 60);
        return hours + "hour " + minutes + "min " + seconds + "sec";
    }
    

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
            Liquid.Init();
            World.Init(ground, uiground, uiground2, back);
            Structure.Init();
            SpaceShip.Init();
            PlayerMouvements.initialPosition = World.spawn;
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
        Player.inventoryBuildings.Add(Building.Type.Printer3D, 1);
        Player.inventoryBuildings.Add(Building.Type.SolarPanel, 1);
        Player.inventoryUsables.Add(Usable.Type.IronBlock, 20);

    }


    public override void _Process(float delta)
    {
        WorldScreenSizeX = GetViewport().Size.x * CurrentCamera.GetXZoom();
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        timePlayed += delta;
    }

    public static float GetScreenMinX() => PlayerMouvements.GetX() - (Convertion.Location2World(new Vector2(Game.WorldScreenSizeX/2, 0))).x;
    public static float GetScreenMaxX() => PlayerMouvements.GetX() + (Convertion.Location2World(new Vector2(Game.WorldScreenSizeX/2, 0))).x;


    public void _on_BTNSAVE_button_down()
    {
        GD.Print(saveName + " de");
        Save._Save(saveName);
    }
}
