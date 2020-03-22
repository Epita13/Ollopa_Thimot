using Godot;
using System;
using System.Collections.Generic;
using Array = Godot.Collections.Array;

public class LiquidMove : TileMap
{
	/*Il reste un petit soucis, c'est que de l'eau se teleporte plus loin lorsque on ne la limite plus a une cuvette*/
	
	
	/*Pour utiliser l'eau, il suffit d'appeler la fonction DrawWaterLevel(), pour les niveaux, le niveau max est
	 défini par capacity. Pour fonctionner correctement le TileSet associé doit contenir au minimum un sprite pour chaque
	 niveau. Numeroté de 1 a capacity. le sprite 0 doit OBLIGATOIREMENT etre un sprite transparent*/
	
	/*Ne prend pas en compte le y = 0 de la TileMap Watermap parce que ya des soucis avec le changement de coordonnées sinon*/
	
	
	private List<Tuple<int,int,int>> map = new List<Tuple<int , int, int>>();
	private const int Capacity = Liquid.Capacity;
	private int width; 		//Hauteur et largeur de la matrice qui gere l'eau
	private readonly int height;
	public readonly Liquid.Type type;

	private int test = 0;

	public void Move()
	{
		World.IsInitWorldTest("Liquid." + type);
		width = World.size * Chunk.size;
		DrawWaterLevel();
		test++;
	}

	public LiquidMove(Liquid.Type type)
	{
		height = Chunk.height;
		this.type = type;
	}
	

	public void PlaceWater(int x, int y)
	{
		if(Block.GetIDTile(World.GetBlock(x,y).type) == -1)
			map.Add(new Tuple<int, int, int>(x, y, 8));
	}

	private void DrawWaterLevel()
	 {
		 /*Récupere les niveaux et emplacement d'eau puis calcule les nouveux niveau verticaux puis horizontaux*/
		 Verification();
		 VerticalWater();
		 Verification();
		 DrawWater();
		 HorizontalWater();
		 Verification();
		 DrawWater();
	 }
	 
	 private void Verification()
	 {
		 /*Recuperation des emplacements de blocks et d'eau sur toutes les TileMap*/

		 foreach (Tuple<int,int,int> block in map)
		 {
			 int x = block.Item1;
			 int y = block.Item2;
			 int h = block.Item3;
			 SetCell(x,y, -1);
			 if (Block.GetIDTile(World.GetBlock(x, y).type) != -1 || h <= 0)
				 map.Remove(block);
		 }
	 }

	 private void HorizontalWater()
	 {
		 /*Calcule la difference d'eau avec les tuiles voisines d'une tuiles contenant de l'eau.
		  Redefinit ensuite le nouveau niveau en fonction des blocks deja present*/

		 foreach (Tuple<int,int,int> block in map)
		 {
			 int x = block.Item1;
			 int y = block.Item2;
			 int h = block.Item3;
			 Tuple<int, int, int> blockinf = Find(x, y - 1);
			 int differenceLeft = Difference(x, y, 'L');
			 int differenceRight = Difference(x, y, 'R');


			 if (y > 0 && Block.GetIDTile(World.GetBlock(x - 1, y).type) == -1 && Block.GetIDTile(World.GetBlock(x + 1, y).type) == -1 && (blockinf.Item3 == 8 || blockinf.Item3 == -1))
			 {
				 /*Cas standard, pas de bloc ou mur ni a gauche, ni a droite*/
				 if (differenceLeft > differenceRight)
					 Mouvement(x, y, 'L');
				 else if (differenceLeft < differenceRight)
					 Mouvement(x, y, 'R');
				 else if (differenceLeft == differenceRight && differenceLeft != 0)
					 Mouvement(x, y, 'R');
			 }
			 else if (y > 0 && Block.GetIDTile(World.GetBlock(x - 1, y).type) != -1 && Block.GetIDTile(World.GetBlock(x + 1, y).type) == 0 && differenceRight != 0 && (blockinf.Item3 == 8 || blockinf.Item3 == -1))
				 /*Cas bloc ou mur a gauche et PAS a gauche*/
				 Mouvement(x, y, 'R');
			 else if (y > 0 && Block.GetIDTile(World.GetBlock(x + 1, y).type) != -1 && Block.GetIDTile(World.GetBlock(x - 1, y).type) == -1 && differenceLeft != 0 && (blockinf.Item3 == 8 || blockinf.Item3 == -1))
				 //map[x, y + 1] == 8 || map[x, y + 1] == 0
				 /*Cas block ou mur a gauche et PAS a droite*/
				 Mouvement(x, y, 'L');
		 }
	 }  
	 
	 private void VerticalWater()
	 {
		 /*Calcule pour toutes les tuiles vide la difference d'eau avec la tuile au dessus.
		  Transfert l'eau si possible*/

		 foreach (Tuple<int,int,int> block in map)
		 {
			 int x = block.Item1;
			 int y = block.Item2;
			 int h = block.Item3;
			 Tuple<int, int, int> blockinf = Find(x, y - 1);
			 int h2 = blockinf.Item3;
			 
			 if (Block.GetIDTile(World.GetBlock(x, y - 1).type) == -1)
			 {
				 int difference = Capacity - h2;
				 if (h2 < 0)
					 difference = Capacity;
				 
				 map.Remove(blockinf);
				 
				 if(h < difference)
				 {
					 h2 += h;
				 }
				 else
				 {
					 h2 = Capacity;
					 h -= difference;
					 map.Add(new Tuple<int, int, int>(x, y - 1, h2));
				 }
				 
				 map.Remove(block);
				 map.Add(new Tuple<int, int, int>(x, y, h));
			 }
		 }
	 }


	 private void DrawWater()
	 {
		 /*Dessine sur la Tilemap les niveaux d'eau correspondant à la matrice*/

		 foreach (Tuple<int,int,int> block in map)
		 {
			 SetCell(block.Item1, block.Item2 - height, block.Item3);
		 }
	 }

	 private int Difference(int x, int y, char side) 
	 {
		 /*Calcule la difference d'eau avec le block de droite ou de gauche selon side*/
		 int dif = 0;
		 switch (side)
		 {
			 case 'R':
			 {
				 Tuple<int, int, int> block = Find(x, y);
				 Tuple<int, int, int> blockinf = Find(x - 1, y);
				 
				 if (blockinf.Item3 > 0 && block.Item3 > blockinf.Item3)
					 dif = block.Item3 - blockinf.Item3;
				 else if (block.Item3 > blockinf.Item3)
					 dif = block.Item3;

				 break;
			 }
			 case 'L':
			 {
				 Tuple<int, int, int> block = Find(x, y);
				 Tuple<int, int, int> blocksup = Find(x + 1, y);
				 
				 if (blocksup.Item3 > 0 && block.Item3 > blocksup.Item3)
					 dif = block.Item3 - blocksup.Item3;
				 else if (block.Item3 > blocksup.Item3)
					 dif = block.Item3;
				 
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
		 Tuple<int, int, int> block = Find(x, y);
		 int h = block.Item3;
		 
		 switch (side)
		 {
			 case 'R':
			 {
				 Tuple<int, int, int> blocksup = Find(x + 1, y);
				 int h2 = blocksup.Item3;
				 if (blocksup.Item3 == -1)
					 map.Add(new Tuple<int, int, int>(x + 1, y, 1));
				 else
				 {
					 h2++;
					 map.Remove(blocksup);
					 map.Add(new Tuple<int, int, int>(x + 1, y, h2));
				 }
				 
				 break;
			 }
			 case 'L':
			 {
				 Tuple<int, int, int> blockinf = Find(x - 1, y);
				 int h2 = blockinf.Item3;
				 if (blockinf.Item3 == -1)
					 map.Add(new Tuple<int, int, int>(x + 1, y, 1));
				 else
				 {
					 h2++;
					 map.Remove(blockinf);
					 map.Add(new Tuple<int, int, int>(x + 1, y, h2));
				 }
				 
				 break;
			 }
			 default:
				 throw new ArgumentException("Character different of 'R' or 'L' from Mouvement");
		 }
		 
		 h--;
		 if (h <= 0)
			 map.Remove(block);

	 }

	 private Tuple<int, int, int> Find(int x, int y)
	 {
		 Tuple<int, int, int> res;
		 int i = 0;
		 do
		 {
			 res = map[i];
			 i++;
		 } while ((res.Item1 != x || res.Item2 != y) && i < map.Count );
		 
		 if(i >= map.Count)
			 res = new Tuple<int, int, int>(-1,-1,-1);

		 return res;
	 }
}
