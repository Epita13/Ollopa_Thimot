using Godot;
using System;
using System.Collections.Generic;
using System.Net.Configuration;

public class SpaceShip : Node2D
{
    /*public const float ENERGYWIN = 1000.0f; 
    public const float FUELWIN = 500.0f;
    public const int COMPOSITEWIN = 2500;*/

    private static PackedScene spaceShip = GD.Load<PackedScene>("res://Assets/Objects/Buildings/Spaceship/SpaceShip.tscn");
    
    
    /*public const float ENERGYWIN = 300.0f; 
    public const float FUELWIN = 0.0f;
    public const int COMPOSITEWIN = 50;*/
    public const float ENERGYWIN = 600f; 
    public const float FUELWIN = 300f;
    public const int COMPOSITEWIN = 150;
    
    
    public static SpaceShip instance;
    
    public static int composite;
    public static float fuel;
    public static float energy;
    private static Sprite image;
    private static Control inventory;
    public static bool ShipSelected = false;
    public static bool inventoryOpen = false;
    public static Vector2 location;
    public static Node canvas;
    private static SpaceShipInterface Interface = new SpaceShipInterface();
    
    public static List<Vector2> blocksSpaceShip = new List<Vector2>();

    public bool mouseOn = false;
    public override void _EnterTree()
    {
        canvas = Game.root.GetNode("CanvasLayer");
        instance = this;
        image = GetNode<Sprite>("Image");
        image.Visible = false;
        Generate(Convertion.World2Location(new Vector2(Structure.structurePos.x + 4, Structure.structurePos.y + 1)));

        location = Convertion.Location2World(new Vector2(Position.x+(image.Texture.GetSize().x * Scale.x / 2), Position.y+(image.Texture.GetSize().y * Scale.y / 2)));
        
        int sizeX = Mathf.FloorToInt(image.Texture.GetSize().x * Scale.x / 16) - 2;
        int sizeY = Mathf.FloorToInt(image.Texture.GetSize().y * Scale.y / 16);
        for (int x = (int)Structure.structurePos.x + 5; x < Structure.structurePos.x + 5 + sizeX; x++)
        {
            for (int y = (int) Structure.structurePos.y + 1; y < Structure.structurePos.y + 1 + sizeY; y++)
            {
                blocksSpaceShip.Add(new Vector2(x,y));
            }
        }
    }

    public override void _Process(float delta)
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        
        if (ShipSelected)
        {
            if (!SpaceShipInRange(15))
            {
                ResetOutline();
                ShipSelected = false;
            }
        }

        if (mouseOn)
        {
            mouse_entered();
        }
    }

    public static void Init()
    {
        SpaceShip sp = (SpaceShip)spaceShip.Instance();
        Game.root.AddChild(sp);
    }
    
    public static void Generate(Vector2 pos)
    {
        instance.Position = pos;
        image.Visible = true;
    }

    public static void open_interface()
    {
        SpaceShipInterface.open_interface();
    }
    
    public static void close_interface()
    {
        SpaceShipInterface.close_interface();
    }

    
    public bool SpaceShipInRange(float range)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(PlayerMouvements.GetX() - location.x, 2) +
                                    Mathf.Pow(PlayerMouvements.GetY() - location.y, 2));
        return distance <= range;
    }
    
    public void mouse_entered()
    {
        mouseOn = true;
        if (!ShipSelected && PlayerState.Is(PlayerState.State.Normal, PlayerState.State.Build))
        {
            if (SpaceShipInRange(15))
            {
                SetOutline(0.5f, Color.Color8(0, 150, 255));
                ShipSelected = true;
            }
        }
    }

    public void mouse_exit()
    {
        mouseOn = false;
        ResetOutline();
        ShipSelected = false;
    }

    private void SetOutline(float w, Color color)
    {
        Sprite p = GetNode<Sprite>("OUTLINE");
        p.Material.Set("shader_param/outline_color",color);
        p.Material.Set("shader_param/width", w);
    }

    private void ResetOutline()
    {
        GetNode<Sprite>("OUTLINE").Material.Set("shader_param/width", 0.0f);
    }
    
    
    
    

    public static void AddFuel(float amount)
    {
        fuel += amount;
    }

    public static void AddEnergy(float amount)
    {
        energy += amount;
    }

    public static void AddComposite(int amount)
    {
        composite += amount;
    }
}
