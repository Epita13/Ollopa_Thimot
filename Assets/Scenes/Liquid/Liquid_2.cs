using Godot;
using System;
using Array = Godot.Collections.Array;

public class Liquid_2 : TileMap
{
    //private int i = 70;
    //private int j = 0;
    private TileMap ground2;
    private float Sdelta = 0;
    Random rand = new Random();
    public enum Type 
    { WATER, OIL }

    private TileMap ground;
    
    public override void _Ready()
    {
       ground = (TileMap) this.GetParent();
       ground2 = this;
       //ground = (TileMap) GetTree().GetRoot().GetNode("res://Assests/Scenes/Liquid/Liquid_2");
    }

    public void Liquid_3(Type type)
    {
        
    }
    
    private void Neighbour()
    {
        
    }

    private void Map(TileMap map)
    {
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     {
         Sdelta += delta;
         for (int i = 0; i < 5; i++)
         {
             
         }
         if (Sdelta >= 1)
         {
             Dessin();
             Sdelta = 0;
         }
         
     }

     private void Dessin()
     {
       int[,] map = new int[50,90];
         for(int i = 0; i < 50; i++)
         {
             for (int j = 0; j < 90; j++)
             {
                 map[i, j] = ground.GetCell(i, j);
             }
         }
       
         for(int i = 0; i < map.GetUpperBound(0);i++)
         {
             for(int j = 0; j < map.GetUpperBound(1); j++)
             {
                 if (map[i,j] == 0 || map[i,j] == 1 || map[i,j] == 2)
                 map[i, j] = rand.Next(0, 2);
                 ground.SetCell(i,j,map[i,j]);
                 
             }
         }  
     }
}
