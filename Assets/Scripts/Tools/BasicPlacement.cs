using Godot;
using System;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class BasicPlacement : Node2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	//liste des batiments poses
	private static List<Building> placedBuilding = new List<Building>();

	/// Verifie si le block en x y est de l'air
	private static bool IsAir(int x, int y)
	{
		bool res = true;
		Block b = World.GetBlock(x, y);
		if (b == null || b.type != Block.Type.Air)
			res = false;
		return res;
	}

	/// Verifie si la case est sur le sol
	private static bool IsOnFloor(int x, int y)
	{
		bool res = true;
		Block b = World.GetBlock(x, y - 1);
		if (b==null || b.type == Block.Type.Air)
			res = false;
		return res;
	}

	/// Verifie si la case contient un batiment
	private static bool IsNoBuilding(int x, int y)
	{
		bool res = true;
		int i = 0;
		int l = placedBuilding.Count;
		while (res && i < l)
		{
			if ((int) placedBuilding[i].corners[0].x == x && (int) placedBuilding[i].corners[0].y == y)
				res = false;
			if ((int) placedBuilding[i].corners[1].x == x && (int) placedBuilding[i].corners[1].y == y + 1)
				res = false;
			if ((int) placedBuilding[i].corners[2].x == x + 1 && (int) placedBuilding[i].corners[2].y == y + 1)
				res = false;
			if ((int) placedBuilding[i].corners[3].x == x + 1 && (int) placedBuilding[i].corners[3].y == y)
				res = false;
			i++;
		}
		return res;
	}
	

	/// Verifie si le batiment est placable a droite
	public static bool IsPlacableRight(int x, int y, int sizeX, int sizeY)
	{
		bool res = true;
		int i = 0;
		while (i < sizeX && res)
		{
			res = res && IsOnFloor(x+i,y) && IsAir(x+i, y) && IsNoBuilding(x+i,y);
			i++;
		}

		int j = 1;

		while (j < sizeY && res)
		{
			i = 0;
			while (i < sizeX && res)
			{
				res = res && IsAir(x+i, y+j) && IsNoBuilding(x+i,y+j);;
				i++;
			}
			j++;
		}
		return res;
	}
	/// Verifie si le batiment est placable a gauche
	public static bool IsPlacableLeft(int x, int y, int sizeX, int sizeY)
	{
		bool res = true;
		int i = 0;
		while (i < sizeX && res)
		{
			res = res && IsOnFloor(x-i,y) && IsAir(x-i, y) && IsNoBuilding(x-i,y);
			i++;
		}

		int j = 1;

		while (j < sizeY && res)
		{
			i = 0;
			while (i < sizeX && res)
			{
				res = res && IsAir(x-i-1, y+j) && IsNoBuilding(x-i,y+j);;
				i++;
			}
			j++;
		}
		return res;
	}


	///place un batiment selon les regles
	public static void PlaceWithMouse(Building building,Vector2 mouse,bool right)
	{
		GD.Print("fe");
		Vector2 mouseC = Convertion.Location2WorldFloor(mouse);
		if (right)
		{
			if (IsPlacableRight((int) mouseC.x, (int) mouseC.y, building.size, building.size))
			{
				mouseC.x += building.size / 2;
				mouseC.y += building.size / 2;
				building.Place(mouseC);
				placedBuilding.Add(building);

			}
		}
		else
		{
			if (IsPlacableLeft((int) mouseC.x, (int) mouseC.y, building.size, building.size))
			{
				mouseC.x -= building.size / 2 - 1;
				mouseC.y += building.size / 2;
				building.Place(mouseC);
				placedBuilding.Add(building);
			}
		}
	}
	

}

