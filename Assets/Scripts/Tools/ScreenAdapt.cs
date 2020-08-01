using Godot;
using System;

public class ScreenAdapt : Node2D
{
    private int dpi = OS.GetScreenDpi(); 
    private Viewport viewport;
    private Vector2 size = new Vector2(ratio * OS.WindowSize.x / 100, ratio * OS.WindowSize.y / 100);
    private Vector2 position = OS.WindowPosition;
    private static readonly int ratio = 95;            //taille en pourcent de la fenetre après avoir enlever le plein ecran
    private bool button = false;

    public override void _Ready()
    {
        OS.WindowMaximized = true;
        //OS.WindowFullscreen = true;
        viewport = GetTree().Root;
    }

    public override void _Process(float delta)
    {
        if (!OS.WindowMaximized && !button)
        {
            OS.WindowSize = size;
            button = true;
            OS.WindowPosition = new Vector2(position.x + 25 * size.x / (ratio * 10) ,position.y + 25 * size.y / (ratio * 10));
        }
        
        if (OS.WindowMaximized && button)
        {
            button = false;
            size.x = 19 * OS.WindowSize.x / 20;
            size.y = 19 * OS.WindowSize.y / 20;
            position = OS.WindowPosition;
        }
            
    }
    
    public void ResizeWindow(Vector2 size)
    {
        OS.WindowSize = size;
    }
    
    public void ResizeViewport(Vector2 size)
    {
        viewport.Size = size;
    }
}
