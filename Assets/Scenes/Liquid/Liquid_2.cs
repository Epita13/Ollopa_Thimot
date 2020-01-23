using Godot;
using System;
using Array = Godot.Collections.Array;

public class Liquid_2 : TileMap
{
    public enum Type 
    { WATER, OIL }

    private TileMap ground;
    
    public override void _Ready()
    {
        
    }

    public void Liquid_3(Type type)
    {
        ground = (TileMap) GetTree().GetRoot().GetNode("Liquid_2").GetNode("Ground");
    }
    
    private void Neighbour()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     { 
         TileSet water = (TileSet) GetTree().GetRoot().GetNode("Liquid_2").GetNode("Liquid");
        Array test = water.GetTilesIds();
        ground.SetCell(1,2, (int) test[0]);
    }
}
