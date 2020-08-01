using Godot;
using System;
using System.Net.Configuration;

public class Thermogenerator : Building
{
	public static int nbGenerator;
	
	public float time = 0;
	public int wood = 0;
	public float oil = 0;
	public static int woodMax = 20;
	public static float oilMax = 100F;

	
	/*Structure de sauvegarde*/
	public struct SaveStruct
	{
		public Building.SaveStruct buildingSave;
		public int wood;
		public float oil;
		public float time;
	}

	public SaveStruct GetSaveStruct()
	{
		SaveStruct s = new SaveStruct();
		s.buildingSave = GetBuildingSaveStruct();
		s.wood = wood;
		s.oil = oil;
		s.time = time;
		return s;
	}
	/*************************/
	

	public Thermogenerator() : base (100, 200.0f)
	{
		
	}

	public override void _EnterTree()
	{
		id = nbGenerator;
		nbGenerator+=1;
	}
	
	
	
	public void _on_Timer_timeout()
	{
		if (PlayerState.Is(PlayerState.State.Pause))
			return;
		
		if (time < 1)
		{
			if (wood > 0)
			{
				wood -= 1;
				time += 30;
			}
			else if (oil > 0)
			{
				oil -= 1;
				time += 15;
			}
		}

		if (time > 0)
		{
			AddEnergy(0.35F * timer.WaitTime);
			time -= 1 * timer.WaitTime;
		}

		TransferToLink(timer.WaitTime);
		
	}

	public void AddWood(int nb)
	{
		wood += nb;
	}
	public void AddOil(float nb)
	{
		oil += nb;
	}
}
