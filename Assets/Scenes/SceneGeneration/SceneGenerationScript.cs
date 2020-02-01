using Godot;
using System;

public class SceneGenerationScript : Node
{

    public override void _Ready()
    {
        TileMap ground = (TileMap)GetTree().GetRoot().GetNode("SceneGeneration").GetNode("Ground");
        World.Init(10, ground);
        World.Draw();
        Building.Init(this);
    }


}
