using Godot;
using System;

public class SceneGenerationScript : Node
{

    public override void _Ready()
    {
        TileMap ground = (TileMap)GetNode("Ground");
        World.Init(16, ground);
        World.Draw();
        Building.Init(this);
    }


}
