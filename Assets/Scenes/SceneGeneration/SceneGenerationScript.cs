using Godot;
using System;

public class SceneGenerationScript : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TileMap ground = (TileMap)GetTree().GetRoot().GetNode("SceneGeneration").GetNode("Ground");
        World w = new World(100);
        w.InitTilemaps(ground);
        w.CreateWorld(); 
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
