using Godot;
using System;

public class Grass : AnimatedSprite
{
    public static void Spawn(int x, int y)
    {
        Grass s = (Grass) GD.Load<PackedScene>("res://Assets/Objects/Autres/Grass/Grass.tscn").Instance();
        Game.root.AddChild(s);
        s.Material = (Material) s.Material.Duplicate();
        s.Material.Set("shader_param/x", x);
        s.Position = Convertion.World2Location(new Vector2(x, y));
        s.x = x;
        s.y = y;
    }


    private int x;
    private int y;

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        if (World.GetBlock(x, y).GetType != Block.Type.Air || World.GetBlock(x, y - 1).GetType != Block.Type.Grass)
        {
            QueueFree();
        }
    }

    public void _on_ZONE_body_entered(Node body)
    {
        if (body.GetGroups().Contains("Player"))
        {
            Animation = "other";
        }
    }
    public void _on_ZONE_body_exited(Node body)
    {
        if (body.GetGroups().Contains("Player"))
        {
            Animation = "normal";
        }
    }

}