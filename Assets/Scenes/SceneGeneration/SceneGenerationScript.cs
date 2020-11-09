using Godot;
using System;

public class SceneGenerationScript : Node
{


    public override void _Ready()
    {
        TileMap back = GetNode("Tilemaps").GetNode<TileMap>("0");
        TileMap ground = GetNode("Tilemaps").GetNode<TileMap>("1");
        TileMap uiground = GetNode("Tilemaps").GetNode<TileMap>("3");
        TileMap uiground2 = GetNode("Tilemaps").GetNode<TileMap>("2");
        World.Init(ground, uiground, uiground2, back);
        World.Draw();
        Building.Init(this);
    }


}
