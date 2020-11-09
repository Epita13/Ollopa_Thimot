using Godot;
using System;

public class Refinery : Building
{
	public static int nbRefinery;
	private static float power = 1f;
	private static float power2wake = 10 * power;
	private static float oilToFuel = 0.5f;
	public float oil;
	public float fuel;
	public float oilMAX = 300f;
	public float fuelMAX = 100f;
	public float togive = 0;
	public float toadd = 0;
	private static float giveSpeed = 2.5f;
	public bool on = false;

	
	/*Structure de sauvegarde*/
	public struct SaveStruct
	{
		public Building.SaveStruct buildingSave;
		public float oil;
		public float fuel;
		public float togive;
		public float toadd;
		public bool on;
	}

	public SaveStruct GetSaveStruct()
	{
		SaveStruct s = new SaveStruct();
		s.buildingSave = GetBuildingSaveStruct();
		s.togive = togive;
		s.oil = oil;
		s.fuel = fuel;
		s.toadd = toadd;
		s.@on = @on;
		return s;
	}
	/*************************/
	
	public Refinery() : base (100, 100.0f)
	{
	}

	public override void _EnterTree()
	{
		id = nbRefinery;
		nbRefinery++;
	}
	
	public void _on_Timer_timeout()
	{
		if (PlayerState.Is(PlayerState.State.Pause))
			return;
		
		if (togive >= giveSpeed && Player.inventoryLiquids.CanAdd(Liquid.Type.Fuel, giveSpeed))
		{
			Player.inventoryLiquids.Add(Liquid.Type.Fuel, giveSpeed);
			togive -= giveSpeed;
			fuel -= giveSpeed;
		}
		else if(togive > 0 && Player.inventoryLiquids.CanAdd(Liquid.Type.Fuel, togive))
		{
			Player.inventoryLiquids.Add(Liquid.Type.Fuel, togive);
			fuel -= togive;
			togive = 0;
		}
		
		if (toadd >= giveSpeed)
		{
			Player.inventoryLiquids.Remove(Liquid.Type.Oil, giveSpeed);
			toadd -= giveSpeed;
			oil += giveSpeed;
		}
		else if(toadd > 0)
		{
			Player.inventoryLiquids.Remove(Liquid.Type.Oil, toadd);
			oil += toadd;
			toadd = 0;
		}

		if (on)
		{
			float time = timer.WaitTime;
			RemoveEnergy(power * time);
			fuel += oilToFuel * time;
			oil -= time;
		}
		
		if (fuel > fuelMAX)
			fuel = fuelMAX;
		if (oil < 0)
			oil = 0;

		on = (energy >= power2wake && !on || energy > 0 && on) && fuel < fuelMAX && oil > 0;
	}
}
