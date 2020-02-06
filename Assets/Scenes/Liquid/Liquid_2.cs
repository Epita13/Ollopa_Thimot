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
		 
		 if (Sdelta >= 1)
		 {
			 DrawWaterLevel();
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
	 
	 private int[,] GetWaterLevel()
	 {
		 /*Recuperation des emplacements de blocks et d'eau sur toutes les TileMap*/

		 int[,] map = new int[500, 500];		//a modifier pour prendre la taille window*/
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

	 private int[,] HorizontalWater(int[,] map)		/*Bug qui vient je ne sais pas d'où*/
	 {
		 /*Calcule la difference d'eau avec les tuiles voisines d'une tuiles contenant de l'eau.
		  Redefinit ensuite le nouveau niveau en fonction des blocks deja present*/
		 
		 int[,] mapbuffer = (int[,]) map.Clone(); //Sert de tampon pour enregistrer les nouvelles valeurs sans ecraser les anciennes

		 for (int z = 12; z <= 22; z++)
			 GD.Print(map[z,8]);
		 GD.Print(' ');
		 for (int x = 0; x < map.GetUpperBound(0); x++)
		 {
			 for (int y = 0; y < map.GetUpperBound(1); y++)
			 {
				 if (map[x, y] > 0)
				 {
					int differenceLeft = Difference(map, x, y, 'L');
					int differenceRight = Difference(map, x, y, 'R');
					
					if (x > 0 && x < map.GetUpperBound(0) && map[x - 1, y] != 0 && map[x + 1, y] != 0)
					{
						if (differenceLeft > differenceRight)
						{
							
							Mouvement(map,  ref mapbuffer, x,y, 'L');
						}
						else if (differenceLeft < differenceRight )
						{
							
							Mouvement(map, ref mapbuffer, x,y, 'R');
						}
						else if (differenceLeft == differenceRight && differenceLeft != 0)
						{
							
							Mouvement(map, ref mapbuffer, x,y, 'R');
						}
					}
					else if ((x == 0 || map[x - 1, y] == 0) && map[x + 1, y] != 0 && differenceRight != 0)
					{
						Mouvement(map, ref mapbuffer, x,y, 'R');
					}
					else if ((map[x + 1, y] == 0 || x == map.GetUpperBound(0)) && map[x - 1, y] != 0 && differenceLeft != 0)
					{
						Mouvement(map, ref mapbuffer, x,y, 'L');
					}
				 }
			 }
		 }
		 return mapbuffer;
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

	 private int Difference(int[,] map, int x, int y, char side) 
	 {
		 /*Calcule la difference d'eau avec le block de droite ou de gauche selon side*/
		 int dif = 0;
		 if (side == 'R')
		 {
			 if (x < map.GetUpperBound(0))
			 {
				 if (map[x + 1, y] > 0 && map[x, y] > map[x + 1, y])
					 dif = map[x, y] - map[x + 1, y];
				 else if (map[x + 1, y] == -1)
					 dif = map[x,y];
			 }
		 }
		 else if (side == 'L')
		 {
			 if (x > 0)
			 {
				if (map[x - 1, y] > 0 && map[x, y] > map[x - 1, y])
					dif = map[x, y] - map[x - 1, y];
				else if (map[x - 1, y] == -1)
					dif = map[x,y]; 
			 }
		 }
		 else
			throw new ArgumentException("Character different of 'R' or 'L' from Difference");

		 if (dif < 0)
			 dif = 0;
		 
		 return dif;
	 }

	 private void Mouvement(int[,] map,ref int[,] mapbuffer, int x, int y, char side) 
	 {
		 /*Deplace horizontalement 1 d'eau selon side*/
		 if (side == 'R')
		 {
			 if (map[x + 1, y] == -1)
				 mapbuffer[x + 1, y] = 1;
			 else 
				 mapbuffer[x + 1, y]++;
		 }
		 else if (side == 'L')
		 {
			 if (map[x - 1, y] == -1)
				 mapbuffer[x - 1, y] = 1;
			 else
				 mapbuffer[x - 1, y]++;
		 }
		 else
			 throw new ArgumentException("Character different of 'R' or 'L' from Mouvement");
		 
		 mapbuffer[x, y]--;
		 if (mapbuffer[x, y] == 0)
			 mapbuffer[x, y] = -1;
		 
	 }
}
