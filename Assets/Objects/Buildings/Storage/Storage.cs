using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Policy;

public class Storage : Building
{

	//energy
	public float energy = 0;
	public float maxEnergy  = 1000f;
	//oxygéne
	public float oxygene = 0;
	public float maxOxygene = 1000f;
	 
	
	//ID de chaque batiment
	public int nbStorage = 0;
	public int id;

	//Initialisation
    public Storage() : base (150)
    {
        this.id = nbStorage;
		nbStorage += 1;
    }
	
	 public override void _Ready()
    {
    }
	
	public void AddEnergy(float amount)
	{
		energy += amount;
		if(energy >= maxEnergy)
			energy = maxEnergy;
	}
	
	public bool RemoveEnergy(float amount)
	{
			bool verif = false;
			if (energy - amount >= 0)
			{
				energy -= amount;
				verif = true;
			}
			return verif;
	}
	public void AddOxygene(float amount)
	{
		energy += amount;
		if(energy >= maxEnergy)
			energy = maxEnergy;
	}

	public bool RemoveOxygene(float amount)
	{
		bool verif = false;
		if (energy - amount >= 0)
		{
			energy -= amount;
			verif = true;
		}

		return verif;
	}
}
