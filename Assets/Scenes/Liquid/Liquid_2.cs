using Godot;
using System;
using Array = Godot.Collections.Array;

public class Liquid_2 : TileMap
{
    private float Sdelta = 0;
    private TileMap ground;
    private TileMap waterMap;
    private int capacity = 8;        //nombre d'etages de l'eau, 0 = pas d'eau, 8 = cube plein;
    public enum Type 
    { WATER, OIL }

    
    
    public override void _Ready()
    {
       ground = (TileMap) this.GetParent();
       waterMap = this;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     {
         Sdelta += delta;
         
         if (Sdelta >= 1)
         {
             DrawWaterLevel();
             Sdelta = 0;
         }
     }
     
     private void DrawWaterLevel()
     {
         int[,] map = GetWaterLevel();
         map = VerticalWater(map);
         DrawWater(map);
         DrawWater(HorizontalWater(map));
     }
     
     private int[,] GetWaterLevel()
     {
         /*Recuperation des valeurs*/
         int[,] map = new int[50,50];
         for(int x = 0; x <= map.GetUpperBound(0); x++)
         {
             for (int y = 0; y <= map.GetUpperBound(1); y++)
             {
                 if (ground.GetCell(x, y) != -1)
                     map[x, y] = 0;
                 else
                     map[x, y] = -1;

                 if (map[x, y] == -1 && waterMap.GetCell(x, y) > 0)
                     map[x, y] = waterMap.GetCell(x, y);
             }
             
         }
         return map;
     }

     private int[,] HorizontalWater(int[,] map)
     {
         for (int x = 0; x <= map.GetUpperBound(0); x++)
         {
             for (int y = 0; y <= map.GetUpperBound(1); y++)
             {
                 int difference_left = 0;
                 int difference_right = 0;
                 
                 if (map[x, y] != 0)
                 {
                     if (x > 0 && x < map.GetUpperBound(0))
                     {
                         if (map[x - 1, y] == -1)
                             difference_left = capacity;
                         else if (map[x - 1, y] != 0)
                             difference_left = capacity - map[x - 1, y];
                         
                         if (map[x + 1, y] == -1)
                             difference_right = capacity;
                         else if (map[x + 1, y] != 0)
                             difference_right = capacity - map[x + 1, y];
                     } 
                     else if (x == 0)
                     {
                         if (map[x + 1, y] == -1)
                             difference_right = capacity;
                         else if (map[x + 1, y] != 0)
                             difference_right = capacity - map[x + 1, y];              
                     }
                     else
                     {
                         if (map[x - 1, y] == -1)
                             difference_left = capacity;
                         else if (map[x - 1, y] != 0)
                             difference_left = capacity - map[x - 1, y];               
                     }  
                 }

                 if (difference_left != 0 && difference_right != 0)
                 {

                     if (difference_left > difference_right && map[x, y] > 1)
                     {

                         if (map[x - 1, y] > 0 && map[x - 1, y] < 8)
                         {
                             map[x - 1, y]++;
                             map[x, y]--;
                         }
                         else if (map[x - 1, y] == -1)
                         {
                             map[x - 1, y] = 1;
                             map[x, y]--;
                         }

                     }
                     else if (map[x, y] > 1)
                     {
                         if (map[x + 1, y] > 0 && map[x + 1, y] < 8)
                         {
                             map[x + 1, y]++;
                             map[x, y]--;
                         }
                         else if (map[x - 1, y] == -1)
                         {
                             map[x + 1, y] = 1;
                             map[x, y]--;
                         }
                     }
                 }

             }
         }
         return map;
     }
     
     private int[,] VerticalWater(int[,] map)
     {
         for (int x = 0; x <= map.GetUpperBound(0); x++)
         {
             for (int y = map.GetUpperBound(1); y >= 0; y--)
             {
                 if (y > 0 && map[x, y - 1] > 0 && map[x,y] != 0)
                 {
                     if (map[x, y] <= -1)
                     {
                         map[x, y] += map[x, y - 1] + 1;
                         map[x, y - 1] = -1; 
                     }
                     else
                     {
                         int difference = capacity - map[x, y];
                         if(map[x, y - 1] < difference)
                         {
                             map[x, y] += map[x, y - 1];
                             map[x, y - 1] = -1;                                                      
                         }
                         else
                         {
                             map[x, y] = capacity;
                             map[x, y - 1] -= difference;
                         }
                     }

                     if (map[x, y] == 0)
                         map[x, y] = -1;
                     if (map[x, y - 1] == 0)
                         map[x, y - 1] = -1;
                 }
             }
         }

         return map;
     }

     
     private void DrawWater(int [,] waterLevel)
     {
         /*Dessine sur la Tilemap les niveaux d'eau correspondant à la matrice*/
         
         for (int x = 0; x <= waterLevel.GetUpperBound(0); x++)
         {
             for (int y = 0; y <= waterLevel.GetUpperBound(1); y++)
             {
                 waterMap.SetCell(x,y, waterLevel[x,y]);
             }
         }
     }
}
