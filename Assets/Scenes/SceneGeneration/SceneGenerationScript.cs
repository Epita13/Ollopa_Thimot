using Godot;
using System;

public class SceneGenerationScript : Node
{


    public override void _Ready()
    {
        TileMap back = GetNode("Tilemaps").GetNode<TileMap>("0");
        TileMap ground = GetNode("Tilemaps").GetNode<TileMap>("1");
        TileMap uiground = GetNode("Tilemaps").GetNode<TileMap>("2");
        World.Init(16, ground, uiground, back);
        World.Draw();
        Building.Init(this);
    }


}
