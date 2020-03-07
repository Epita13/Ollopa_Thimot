using Godot;
using System;

public class TextureRect : Godot.TextureRect
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(float delta)
    {
        int i = 0;
        i++;
        
        string test = "res://Assets/Objects/UI/HUD/ImageTest/12.jpg";
        //this.Texture.ResourcePath = test;
        //GD.Print(Texture.ResourcePath);

        
    }
}
