using Godot;
using System;

public class PauseMenu : Control
{

    private static PackedScene s = GD.Load<PackedScene>("res://Assets/Objects/UI/Menus/PauseMenu.tscn");
    private static PauseMenu pm;
    
    public static void Open()
    {
        if (pm==null)
        {
            pm = (PauseMenu)s.Instance();
            Game.root.GetNode("CanvasLayer").AddChild(pm);
        }
    }

    public static void Close()
    {
        if (pm!=null)
        {
            pm.QueueFree();
            pm = null;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
}
