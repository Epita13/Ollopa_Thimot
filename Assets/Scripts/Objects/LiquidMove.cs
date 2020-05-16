using Godot;
using System;
using System.Collections.Generic;
using Thread = System.Threading.Thread;


public class LiquidMove 
{
	/*Pour utiliser l'eau, il suffit d'appeler la fonction DrawWaterLevel(), pour les niveaux, le niveau max est
	 défini par capacity. Pour fonctionner correctement le TileSet associé doit contenir au minimum un sprite pour chaque
	 niveau. Numeroté de 1 a capacity. le sprite 0 doit OBLIGATOIREMENT etre un sprite transparent*/
	
	
	/*Petit bug que j'ai remarqué, le block contenant de l'eau ne se mets pas à jour donc si il est seul on peut mettre
	 bloc dessus sans que ca le fasse disparaitre*/
	
	public List<Tuple<int,int>> listLiquid = new List<Tuple<int,int >>{};
	private List<Tuple<int,int>> ToRemove = new List<Tuple<int,int>>{};
	private const int Capacity = Liquid.Capacity;
	private static int width; 
	private static int height;
	private readonly Liquid.Type type;
	public int[,] map;
	private readonly Thread init;
	private int i = 0;

	public LiquidMove(Liquid.Type type)
	{
		this.type = type;
		init = new Thread(Init);
		init.Start();
	}
	
	private void Init()
	{
		height = Chunk.height;
		width = World.size * Chunk.size - 1;
		map = new int[width + 1,height];
		for (int j = 0; j < map.GetLength(0); j++)
		{
			for (int k = 0; k < map.GetLength(1); k++)
			{
				map[j, k] = -1;
			}
		}
	}

	public void CloneWater(float viewportX, Vector2 origin)
	{
		Vector2 p = origin * CurrentCamera.GetXZoom();
		int viewportSizeX = Mathf.FloorToInt(viewportX * CurrentCamera.GetXZoom());
		Vector2 vecMin = Convertion.Location2World(p) * -1;
		Vector2 vecMax = Convertion.Location2World(new Vector2(p.x*-1+viewportSizeX, p.y));
		if (vecMin.x < 0)
		{
			i = (int) Mathf.Abs(vecMin.x / Chunk.size) + 1;
			i *= Chunk.size;
			foreach (Tuple<int,int> block in listLiquid)
			{
				if (block.Item1 > width - i)
				{
					Liquid.listMap[type].SetCell(block.Item1 - width - 1, height - block.Item2, map[block.Item1, block.Item2]);
				}
			}
		}
		if (vecMax.x >= World.size*Chunk.size)
		{
			i = (int) Mathf.Abs((vecMax.x - Chunk.size * World.size)/ Chunk.size) + 1;
			i *= Chunk.size;
			foreach (Tuple<int,int> block in listLiquid)
			{
				if (block.Item1 < i)
				{
					Liquid.listMap[type].SetCell(width + block.Item1 + 1, height - block.Item2, map[block.Item1, block.Item2]);
				}
			}
		}
	}

	public void Move()
	{
		if (!init.IsAlive)
		{
			VerticalWater();
			HorizontalWater();
			DrawWater();
		}
	}

	public bool Place(int x, int y)			//coordonées du jeu et pas celle de godot
	{
		map[x, y] = UpdateBlock(x, y, 'C');
		bool res = false;
		
		if (map[x, y] == -1)
		{
			if (Liquid.listMap.Count!=0)
				Liquid.listMap[type].SetCell(x, height - y, 8);
			map[x, y] = 8;
			listLiquid.Add(new Tuple<int, int>(x,y));
			res = true;
		}
		
		return res;
	}

	private void HorizontalWater()
	 {
		 /*Calcule la difference d'eau avec les tuiles voisines d'une tuiles contenant de l'eau.
		  Redefinit ensuite le nouveau niveau en fonction des blocks deja present*/

		 int lgr = listLiquid.Count;
		 for (int i = 0; i < lgr; i++)
		 {
			 Tuple<int, int> block = listLiquid[i];

			 map[block.Item1, block.Item2] = UpdateBlock(block.Item1, block.Item2, 'C');
			 if(block.Item1 > 0)
				 map[block.Item1 - 1, block.Item2] = UpdateBlock(block.Item1, block.Item2, 'L');
			 else
				 map[width, block.Item2] = UpdateBlock(block.Item1, block.Item2, 'L');
			 
			 if (block.Item1 < width)
				 map[block.Item1 + 1, block.Item2] = UpdateBlock(block.Item1, block.Item2, 'R');
			 else
				 map[0, block.Item2] = UpdateBlock(block.Item1, block.Item2, 'R');
			 
			 if(block.Item2 > 0)
				 map[block.Item1, block.Item2 - 1] = UpdateBlock(block.Item1, block.Item2, 'D');
				 
			 int differenceLeft = Difference(block.Item1, block.Item2, 'L');
			 int differenceRight = Difference(block.Item1, block.Item2, 'R');

			 if (block.Item1 == 0)
			 {
				 /*Cas début de map*/
				 
				 if (map[width, block.Item2] != 0 && map[block.Item1 + 1, block.Item2] != 0 
				     && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
				 {
					 /*Cas standard, pas de bloc ou mur ni a gauche, ni a droite*/
					 if (differenceLeft > differenceRight)
						 Mouvement(block.Item1, block.Item2, 'L');
					 else if (differenceLeft < differenceRight)
						 Mouvement(block.Item1, block.Item2, 'R');
					 else if (differenceLeft == differenceRight && differenceLeft != 0)
					 {
						 Random side = new Random();
						 if(side.Next(10) > 5)
							 Mouvement(block.Item1, block.Item2, 'R');
						 else
							 Mouvement(block.Item1, block.Item2, 'L');
					 }
				 }
				 else if (map[width, block.Item2] == 0 && map[block.Item1 + 1, block.Item2] != 0 && 
				          differenceRight != 0 && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
					 /*Cas block ou mur a gauche et PAS a gauche*/
					 Mouvement(block.Item1, block.Item2, 'R');
				 else if (map[block.Item1 + 1, block.Item2] == 0 && map[width, block.Item2] != 0 &&
				          differenceLeft != 0 && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
					 /*Cas block ou mur a gauche et PAS a droite*/
					 Mouvement(block.Item1, block.Item2, 'L');
			 }
			 else if (block.Item1 == width)
			 {
				 /*Cas fin de map*/
				 
				 if (map[block.Item1 - 1, block.Item2] != 0 && map[0, block.Item2] != 0 
				     && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
				 {
					 /*Cas standard, pas de bloc ou mur ni a gauche, ni a droite*/
					 if (differenceLeft > differenceRight)
						 Mouvement(block.Item1, block.Item2, 'L');
					 else if (differenceLeft < differenceRight)
						 Mouvement(block.Item1, block.Item2, 'R');
					 else if (differenceLeft == differenceRight && differenceLeft != 0)
					 {
						 Random side = new Random();
						 if(side.Next(10) > 5)
							 Mouvement(block.Item1, block.Item2, 'R');
						 else
							 Mouvement(block.Item1, block.Item2, 'L');
					 }
				 }
				 else if (map[block.Item1 - 1, block.Item2] == 0 && map[0, block.Item2] != 0 && differenceRight != 0 && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
					 /*Cas block ou mur a gauche et PAS a gauche*/
					 Mouvement(block.Item1, block.Item2, 'R');
				 else if (map[0, block.Item2] == 0 && map[block.Item1 - 1, block.Item2] != 0 &&
				          differenceLeft != 0 && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
					 /*Cas block ou mur a gauche et PAS a droite*/
					 Mouvement(block.Item1, block.Item2, 'L');
			 }
			 else if (map[block.Item1 - 1, block.Item2] != 0 && map[block.Item1 + 1, block.Item2] != 0 
				     && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
			 {
				 /*Cas standard, pas de bloc ou mur ni a gauche, ni a droite*/
				 if (differenceLeft > differenceRight)
					 Mouvement(block.Item1, block.Item2, 'L');
				 else if (differenceLeft < differenceRight)
					 Mouvement(block.Item1, block.Item2, 'R');
				 else if (differenceLeft == differenceRight && differenceLeft != 0)
				 {
					 Random side = new Random();
					 if(side.Next(10) > 5)
						 Mouvement(block.Item1, block.Item2, 'R');
					 else
						 Mouvement(block.Item1, block.Item2, 'L');
				 }
			 }
			 else if (map[block.Item1 - 1, block.Item2] == 0 && map[block.Item1 + 1, block.Item2] != 0 && differenceRight != 0 && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
				 /*Cas block ou mur a gauche et PAS a gauche*/
				 Mouvement(block.Item1, block.Item2, 'R');
			 else if (map[block.Item1 + 1, block.Item2] == 0 && map[block.Item1 - 1, block.Item2] != 0 &&
			          differenceLeft != 0 && (map[block.Item1, block.Item2 - 1] == 8 || map[block.Item1, block.Item2 - 1] == 0))
				 /*Cas block ou mur a gauche et PAS a droite*/
				 Mouvement(block.Item1, block.Item2, 'L');
		 }
	 }  
	 
	 private void VerticalWater()
	 {
		 /*Calcule pour toutes les tuiles vide la difference d'eau avec la tuile au dessus.
		  Transfert l'eau si possible*/

		 int lgr = listLiquid.Count;
		 for(int i = 0; i < lgr; i++)
		 {
			 Tuple<int, int> block = listLiquid[i];
			 map[block.Item1, block.Item2] = UpdateBlock(block.Item1, block.Item2, 'C');
			 if(block.Item2 > 0)
				map[block.Item1, block.Item2 - 1] = UpdateBlock(block.Item1, block.Item2, 'D');
			 
			 if (block.Item2 > 0 && map[block.Item1, block.Item2] > 0 && map[block.Item1, block.Item2 - 1] != 0 && map[block.Item1, block.Item2 - 1] < 8)
			 {
				 if (map[block.Item1, block.Item2 - 1] <= -1)
				 {
					 map[block.Item1, block.Item2 - 1] += map[block.Item1, block.Item2] + 1;
					 map[block.Item1, block.Item2] = -1;
					 ToRemove.Add(block);
					 listLiquid.Add(new Tuple<int, int>(block.Item1, block.Item2 - 1));
				 }
				 else
				 {
					 int difference = Capacity - map[block.Item1, block.Item2 - 1];
					 if(map[block.Item1, block.Item2] < difference)
					 {
						 map[block.Item1, block.Item2 - 1] += map[block.Item1, block.Item2];
						 map[block.Item1, block.Item2] = -1;  
						 ToRemove.Add(block);
					 }
					 else
					 {
						 map[block.Item1, block.Item2 - 1] = Capacity;
						 map[block.Item1, block.Item2] -= difference;
					 }
				 }
				 
				 if (map[block.Item1, block.Item2] == 0)
					 map[block.Item1, block.Item2] = -1;
				 if (map[block.Item1, block.Item2 - 1] == 0)
					 map[block.Item1, block.Item2 - 1] = -1;
			 }
			 else if (map[block.Item1, block.Item2] < 1)
			 {
				 ToRemove.Add(block);
			 }
		 }
	 }

	 
	 private void DrawWater()
	 {
		 /*Dessine sur la Tilemap les niveaux d'eau correspondant à la matrice*/

			foreach (Tuple<int, int> block in ToRemove)
		 {
			 Liquid.listMap[type].SetCell(block.Item1, height - block.Item2, -1);
			 listLiquid.Remove(block);
			 if(block.Item1 > width - i)
				 Liquid.listMap[type].SetCell( block.Item1 - width - 1, height - block.Item2, -1);
			 else if(block.Item1 < i)
				 Liquid.listMap[type].SetCell(width + block.Item1 + 1, height - block.Item2, -1);
		 }
		 ToRemove.Clear();

		 foreach (Tuple<int,int> block in listLiquid)
			 Liquid.listMap[type].SetCell(block.Item1, height - block.Item2, map[block.Item1, block.Item2]);
	 }

	 private int Difference(int x, int y, char side) 
	 {
		 /*Calcule la difference d'eau avec le block de droite ou de gauche selon side*/
		 int dif = 0;
		 switch (side)
		 {
			 case 'R':
			 {
				 if (x < width)
				 {
					 if (map[x + 1, y] > 0 && map[x, y] > map[x + 1, y])
						 dif = map[x, y] - map[x + 1, y];
					 else if (map[x + 1, y] == -1)
						 dif = map[x,y];
				 }
				 else
				 {
					 if (map[0, y] > 0 && map[x, y] > map[0, y])
						 dif = map[x, y] - map[0, y];
					 else if (map[0, y] == -1)
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
				 else
				 {
					 if (map[width, y] > 0 && map[x, y] > map[width, y])
						 dif = map[x, y] - map[width, y];
					 else if (map[width, y] == -1)
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

	 private void Mouvement(int x, int y, char side) 
	 {
		 /*Deplace horizontalement 1 d'eau selon side*/
		 switch (side)
		 {
			 case 'R':
			 {
				 if (x < width)
				 {
					if (map[x + 1, y] == -1)
					{
						map[x + 1, y] = 1;
						listLiquid.Add(new Tuple<int, int>(x + 1, y));
					}
					else 
						map[x + 1, y]++; 
				 }
				 else
				 {
					 if (map[0, y] == -1)
					 {
						 map[0, y] = 1;
						 listLiquid.Add(new Tuple<int, int>(0, y));
					 }
					 else 
						 map[0, y]++; 
				 }
				 
				 break;
			 }
			 case 'L':
			 {
				 if (x > 0)
				 {
					if (map[x - 1, y] == -1)
					{
						map[x - 1, y] = 1;
						listLiquid.Add(new Tuple<int, int>(x - 1, y));
					}
					else
						map[x - 1, y]++; 
				 }
				 else
				 {
					 if (map[width, y] == -1)
					 {
						 map[width, y] = 1;
						 listLiquid.Add(new Tuple<int, int>(width, y));
					 }
					 else
						 map[width, y]++;
				 }
				 break;
			 }
			 default:
				 throw new ArgumentException("Character different of 'R' or 'L' from Mouvement");
		 }
		 
		 map[x, y]--;
		 if (map[x, y] == 0)
		 {
			 map[x, y] = -1;
			 ToRemove.Add(new Tuple<int, int>(x, y));
		 }
	 }

	 private int UpdateBlock(int x, int y, char side)
	 {
		 /*Mets à jour la valeur du block donnée par side, L : (x + 1, y), R : (x - 1, y), D : (x, y - 1), U : (x, y + 1), C : (x, y) */
		 int res = 0;
		 switch (side)
		 {
			 case'L':
				 if (x > 0)
				 {
					 if (Block.GetIDTile(World.GetBlock(x - 1, y).GetType) == -1 && map[x - 1, y] == 0)
						 res = -1;
					 else if (Block.GetIDTile(World.GetBlock(x - 1, y).GetType) != -1)
						 res = 0;
					 else
						 res = map[x - 1, y];
				 }
				 else
				 {
					 if (Block.GetIDTile(World.GetBlock(width, y).GetType) == -1 && map[width, y] == 0)
						 res = -1;
					 else if (Block.GetIDTile(World.GetBlock(width, y).GetType) != -1)
						 res = 0;
					 else
						 res = map[width, y];
				 }
				 break;
			 case'R':
				 if (x < width)
				 {
					 if (Block.GetIDTile(World.GetBlock(x + 1, y).GetType) == -1 && map[x + 1, y] == 0)
						 res = -1;
					 else if (Block.GetIDTile(World.GetBlock(x + 1, y).GetType) != -1)
						 res = 0;
					 else
						 res = map[x + 1, y];
				 }
				 else
				 {
					 if (Block.GetIDTile(World.GetBlock(0, y).GetType) == -1 && map[0, y] == 0)
						 res = -1;
					 else if (Block.GetIDTile(World.GetBlock(0, y).GetType) != -1)
						 res = 0;
					 else
						 res = map[0, y];
				 }
				 break;
			 case 'D':
				 if (y > 0)
				 {
					 if (Block.GetIDTile(World.GetBlock(x, y - 1).GetType) == -1 && map[x, y - 1] == 0)
						 res = -1;
					 else if (Block.GetIDTile(World.GetBlock(x, y - 1).GetType) != -1)
						 res = 0;
					 else
						 res = map[x, y - 1];
				 }
				 break;
			 case 'U':
				 if (y < height)
				 {
					 if (Block.GetIDTile(World.GetBlock(x, y + 1).GetType) == -1 && map[x, y + 1] == 0)
						 res = -1;
					 else if (Block.GetIDTile(World.GetBlock(x, y + 1).GetType) != -1)
						 res = 0;
					 else
						 res = map[x, y + 1];
				 }
				 break;
			 case 'C':
				 if (Block.GetIDTile(World.GetBlock(x, y).GetType) == -1 && map[x, y] == 0)
						 res = -1;
				 else if (Block.GetIDTile(World.GetBlock(x, y).GetType) != -1)
					 res = 0;
				 else
					 res = map[x, y];
				 break;
			 default:
				 throw new ArgumentException("side is diferent of 'L', 'R', 'D', 'U', UpdateBlock() in LiquidMove.cs");
		 }
		 return res;
	 }
}
