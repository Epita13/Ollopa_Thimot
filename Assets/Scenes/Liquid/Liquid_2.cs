using Godot;
using System;
using Array = Godot.Collections.Array;

public class Liquid_2 : TileMap
{
	/*Il reste un petit soucis, c'est que de l'eau se teleporte plus loin lorsque on ne la limite plus a une cuvette*/
	
	
	/*Pour utiliser l'eau, il suffit d'appeler la fonction DrawWaterLevel(), pour les niveaux, le niveau max est
	 défini par capacity. Pour fonctionner correctement le TileSet associé doit contenir au minimum un sprite pour chaque
	 niveau. Numeroté de 1 a capacity. le sprite 0 doit OBLIGATOIREMENT etre un sprite transparent*/
	
	private float Sdelta = 0;
	private TileMap ground;
	private TileMap waterMap;
	private int capacity = 8;
	public enum Type 
	{ WATER, OIL }

	
	
	public override void _Ready() /*doit appeller chaque tilemap presente sur le jeux, pas encore fait, surement mettre une liste*/
	{
	   ground = (TileMap) this.GetParent();
	   waterMap = this;
	}
	
	public override void _Process(float delta)
	 {
		 Sdelta += delta;
		 DrawWaterLevel();
		 if (Sdelta >= 1)
		 {
			 
			 Sdelta = 0;
		 }
	 }
	 
	 public void DrawWaterLevel()
	 {
		 /*Récupere les niveaux et emplacement d'eau puis calcule les nouveux niveau verticaux puis horizontaux*/
		 
		 int[,] map = GetWaterLevel();
		 map = VerticalWater(map);
		 DrawWater(map);
		 DrawWater(HorizontalWater(map));
	 }
	 
	 private int[,] GetWaterLevel()			/*ajouter le prise en compte de toute les TileMap*/
	 {
		 /*Recuperation des emplacements de blocks et d'eau sur toutes les TileMap*/
		 
		 int[,] map = new int[50,50]; 		//a modifier pour prendre la taille window*/
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
		 /*Calcule la difference d'eau avec les tuiles voisines d'une tuiles contenant de l'eau.
		  Redefinit ensuite le nouveau niveau en fonction des blocks deja present*/
		 
		 /*Peut surement être simplifier*/
		 for (int x = 0; x < map.GetUpperBound(0); x++)
		 {
			 for (int y = 0; y < map.GetUpperBound(1); y++)
			 {
				 if (map[x, y] > 0)
				 {
					 int differenceLeft = 0;
					 int differenceRight = 0;
					 
					 if (x > 0)
					 {
						 if (map[x + 1, y] == -1)
							 differenceRight = capacity;
						 else if (map[x + 1, y] != 0 && map[x + 1,y] < map[x,y])
							 differenceRight = capacity - map[x + 1, y];
					 }
					 
					 if(x < map.GetUpperBound(0))
					 {
						 if (map[x - 1, y] == -1)
							 differenceLeft = capacity;
						 else if (map[x - 1, y] != 0 && map[x - 1,y] < map[x,y])
							 differenceLeft = capacity - map[x - 1, y];
					 }
					 

					 if (x > 0 && x < map.GetUpperBound(0) && map[x - 1, y] != 0 && map[x + 1, y] != 0)
					 {
						 if (differenceLeft > differenceRight)
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
						 else if (differenceLeft <= differenceRight)
						 {
							 if (map[x + 1, y] > 0 && map[x + 1, y] < 8)
							 {
								 map[x + 1, y]++;
								 map[x, y]--;
							 }
							 else if (map[x + 1, y] == -1)
							 {
								 map[x + 1, y] = 1;
								 map[x, y]--;
							 }
						 }
						 else if (differenceLeft == 1 && differenceRight == 1)
						 {
							 if (map[x + 1, y] > 0 && map[x + 1, y] < 8)
							 {
								 map[x + 1, y]++;
								 map[x, y]--;
							 }
							 else if (map[x + 1, y] == -1)
							 {
								 map[x + 1, y] = 1;
								 map[x, y]--;
							 }
						 }
					 }
					 else if ((x == 0 || map[x - 1, y] == 0) && map[x + 1, y] != 0 && differenceRight != 0)
					 {
						 if (map[x + 1, y] > 0 && map[x + 1, y] < 8)
						 {
							 map[x + 1, y]++;
							 map[x, y]--;
						 }
						 else if (map[x + 1, y] == -1)
						 {
							 map[x + 1, y] = 1;
							 map[x, y]--;
						 }
					 }
					 else if ((map[x + 1, y] == 0 || x == map.GetUpperBound(0)) && map[x - 1, y] != 0 && differenceLeft != 0)
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
}
