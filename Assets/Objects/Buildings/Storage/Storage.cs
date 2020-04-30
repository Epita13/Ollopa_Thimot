using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Policy;

public class Storage : Building
{

	/* Signal pour les voyants */
	[Signal] public delegate void EnergyChange(float energy, float energyMax);
	[Signal] public delegate void OxygeneChange(float energy, float energyMax);

	private Node2D LED;
	private Sprite oxygeneSprite;
	
	//oxygéne
	public float oxygene = 0;
	public static float maxOxygene = 1000f;
	 
	
	//ID de chaque batiment
	public static int nbStorage = 0;



	//Initialisation
    public Storage() : base (150, 750.0f)
    {
    }
	
    public override void _EnterTree()
    {
	    this.id = nbStorage;
	    nbStorage += 1;
	    
	    EmitSignal("OxygeneChange", energy, energyMax);
	    EmitSignal("EnergyChange", energy, energyMax);
	    LED = GetNode<Node2D>("LED");
	    oxygeneSprite = GetNode<Sprite>("Image/oxygene");
	    RefreshLED();
	    RefreshOxygene();
    }
	 
    
    
    public void _on_Timer_timeout()
    {
	    TransferToLink(timer.WaitTime);
	    EmitSignal("EnergyChange", energy, energyMax);
    }

	 private void RefreshOxygene()
	 {
		 Color color = oxygeneSprite.Modulate;
		 int alpha = (int)(oxygene * 255 / maxOxygene);
		 color.a8 = alpha;
		 oxygeneSprite.Modulate = color;
	 }
	 
	 private void RefreshLED()
	 {
		 if (energy >= energyMax && oxygene >= maxOxygene)
		 {
			 Color color = Color.Color8(255,0,0);
			 LED.GetNode<Sprite>("led").Modulate = color;
			 LED.GetNode<Light2D>("Light").Color = color;
		 }
		 else
		 {
			 Color color = Color.Color8(66, 190, 40);
			 LED.GetNode<Sprite>("led").Modulate = color;
			 LED.GetNode<Light2D>("Light").Color = color;
		 }
	 }
	 
	public float AddEnergy(float amount)
	{
		energy += amount;
		float reste = 0;
		if (energy >= energyMax)
		{
			reste = energy - energyMax;
			energy = energyMax;
		}
		RefreshLED();
		return reste;
	}
	
	public bool RemoveEnergy(float amount)
	{
		bool verif = false;
		if (energy - amount >= 0)
		{ 
			energy -= amount;
			verif = true;
		}
		RefreshLED();
		return verif;
	}
	
}
