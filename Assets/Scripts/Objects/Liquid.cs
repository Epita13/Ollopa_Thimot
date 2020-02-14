using Godot;
using System;
using Array = Godot.Collections.Array;

public class Liquid : TileMap
{
	/*Il reste un petit soucis, c'est que de l'eau se teleporte plus loin lorsque on ne la limite plus a une cuvette*/
	
	
	/*Pour utiliser l'eau, il suffit d'appeler la fonction DrawWaterLevel(), pour les niveaux, le niveau max est
	 défini par capacity. Pour fonctionner correctement le TileSet associé doit contenir au minimum un sprite pour chaque
	 niveau. Numeroté de 1 a capacity. le sprite 0 doit OBLIGATOIREMENT etre un sprite transparent*/
	
	/*Ne prend pas en compte le y = 0 de la TileMap Watermap parce que ya des soucis avec le changement de coordonnées sinon*/
	
	
	private TileMap waterMap;
	private int capacity = 8;
	private int width; 		//Hauteur et largeur de la matrice qui gere l'eau
	private int height;
	public static int nbLiquids = 3;
	public enum Type 
	{ Water, Oil, Fuel }

	private void _on_Timer_timeout()
	{
		World.IsInitWorldTest("Liquid");
		width = World.size * Chunk.size;
		DrawWaterLevel();
	}
	
	public override void _Ready()
	{
		waterMap = this;
		height = Chunk.height;
	}
	
	public override void _Process(float delta)
	{
	}

	public void PlaceWater(int x, int y)
	{
		int y2 = Chunk.height - y;
		if (waterMap.GetCell(x, y2) == -1)
			waterMap.SetCell(x, y2, 8);
	}
	 
	 public void DrawWaterLevel()
	 {
		 /*Récupere les niveaux et emplacement d'eau puis calcule les nouveux niveau verticaux puis horizontaux*/
		 
		 int[,] map = GetWaterLevel();
		 map = VerticalWater(map);
		 DrawWater(map);
		 DrawWater(HorizontalWater(map));
	 }
	 
	 private int[,] GetWaterLevel()
	 {
		 /*Recuperation des emplacements de blocks et d'eau sur toutes les TileMap*/

		 int[,] map = new int[width, height];
		 for(int x = 0; x <= map.GetUpperBound(0); x++)
		 {
			 for (int y = 1; y <= map.GetUpperBound(1); y++)
			 {
				 if (Block.GetIDTile(World.GetBlock(x, Chunk.height - y).type) != -1)
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
		 /*Calcule la difference d'eau avec les tuiles voisines d'une tuiles contenant de l'eau.
		  Redefinit ensuite le nouveau niveau en fonction des blocks deja present*/
		 
		 for (int x = 0; x < map.GetUpperBound(0); x++)
		 {
			 for (int y = 0; y < map.GetUpperBound(1); y++)
			 {
				 if (map[x, y] > 1)
				 {
					 int differenceLeft = Difference(map, x, y, 'L');
					 int differenceRight = Difference(map, x, y, 'R');

					 if (x > 0 && x < map.GetUpperBound(0) && map[x - 1, y] != 0 && map[x + 1, y] != 0)
					 {
						 /*Cas standard, pas de bloc ou mur ni a gauche, ni a droite*/
						 if (differenceLeft > differenceRight)
							 Mouvement(ref map, x, y, 'L');
						 else if (differenceLeft < differenceRight)
							 Mouvement(ref map, x, y, 'R');
						 else if (differenceLeft == differenceRight && differenceLeft != 0)
							 Mouvement(ref map, x, y, 'R');
					 }
					 else if ((x == 0 || map[x - 1, y] == 0) && map[x + 1, y] != 0 && differenceRight != 0)
						 /*Cas bloc ou mur a gauche et PAS a gauche*/
						 Mouvement(ref map, x, y, 'R');
					 else if ((map[x + 1, y] == 0 || x == map.GetUpperBound(0)) && map[x - 1, y] != 0 &&
					          differenceLeft != 0)
						 /*Cas block ou mur a gauche et PAS a droite*/
						 Mouvement(ref map, x, y, 'L');
				 }
			 }
		 }
		 return map;
	 }  
	 
	 private int[,] VerticalWater(int[,] map)
	 {
		 /*Calcule pour toutes les tuiles vide la difference d'eau avec la tuile au dessus.
		  Transfert l'eau si possible*/
		 
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

	 private static int Difference(int[,] map, int x, int y, char side) 
	 {
		 /*Calcule la difference d'eau avec le block de droite ou de gauche selon side*/
		 int dif = 0;
		 switch (side)
		 {
			 case 'R':
			 {
				 if (x < map.GetUpperBound(0))
				 {
					 if (map[x + 1, y] > 0 && map[x, y] > map[x + 1, y])
						 dif = map[x, y] - map[x + 1, y];
					 else if (map[x + 1, y] == -1)
						 dif = map[x,y];
				 }

				 break;
			 }
			 case 'L':
			 {
				 if (x > 0)
				 {
					 if (map[x - 1, y] > 0 && map[x, y] > map[x - 1, y])
						 dif = map[x, y] - map[x - 1, y];
					 else if (map[x - 1, y] == -1)
						 dif = map[x,y]; 
				 }

				 break;
			 }
			 default:
				 throw new ArgumentException("Character different of 'R' or 'L' from Difference");
		 }

		 if (dif < 0)
			 dif = 0;
		 
		 return dif;
	 }

	 private static void Mouvement(ref int[,] map, int x, int y, char side) 
	 {
		 /*Deplace horizontalement 1 d'eau selon side*/
		 switch (side)
		 {
			 case 'R':
			 {
				 if (map[x + 1, y] == -1)
					 map[x + 1, y] = 1;
				 else 
					 map[x + 1, y]++;
				 break;
			 }
			 case 'L':
			 {
				 if (map[x - 1, y] == -1)
					 map[x - 1, y] = 1;
				 else
					 map[x - 1, y]++;
				 break;
			 }
			 default:
				 throw new ArgumentException("Character different of 'R' or 'L' from Mouvement");
		 }
		 
		 map[x, y]--;
		 if (map[x, y] == 0)
			 map[x, y] = -1;
		 
	 }
}
