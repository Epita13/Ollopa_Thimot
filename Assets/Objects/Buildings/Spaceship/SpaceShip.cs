using Godot;
using System;

public class SpaceShip : Node2D
{
    /*public const float ENERGYWIN = 1000.0f; 
    public const float FUELWIN = 500.0f;
    public const int COMPOSITEWIN = 2500;*/

    private static PackedScene spaceShip = GD.Load<PackedScene>("res://Assets/Objects/Buildings/Spaceship/SpaceShip.tscn");
    
    
    public const float ENERGYWIN = 0.0f; 
    public const float FUELWIN = 0.0f;
    public const int COMPOSITEWIN = 0;
    
    
    public static SpaceShip instance;
    
    public static int composite = 0;
    public static float fuel = 0;
    public static float energy = 0;
    private static Sprite image;
    private static Control inventory;
    private static Vector2 pos = new Vector2(0,0);
    public static bool ShipSelected = false;
    public static bool inventoryOpen = false;
    public static Node canvas;
    private static SpaceShipInterface Interface = new SpaceShipInterface();
    
    public override void _EnterTree()
    {
        canvas = Game.root.GetNode("CanvasLayer");
        instance = this;
        image = GetNode<Sprite>("Image");
        image.Visible = false;
        pos = Structure.Generate(20, 19);
        Generate(Convertion.World2Location(new Vector2(pos.x + 4, pos.y + 1)));
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

    public void mouse_entered()
    {
        if (!ShipSelected && PlayerState.Is(PlayerState.State.Normal, PlayerState.State.Build))
        {
            SetOutline(0.5f, Color.Color8(0, 150, 255));
            ShipSelected = true;
        }
    }

    public void mouse_exit()
    {
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
