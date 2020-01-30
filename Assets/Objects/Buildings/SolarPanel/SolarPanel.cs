using Godot;
using System;

public class SolarPanel : Building
{
    public static int nbSolarPanel;
    public int id;

    public static float energyMax = 200.0f;
    public float energy = 0;

    public static bool isDay = true;
    public static float sunPower = 0.5f;

    /* Signal pour les voyants */
    [Signal] public delegate void EnergyChange(float energy, float energyMax);

    private Timer timer;

    public SolarPanel() : base (100)
    {
        id = nbSolarPanel;
        nbSolarPanel+=1;
    }

    public override void _EnterTree()
    {
        timer = GetNode<Timer>("Timer");
        EmitSignal("EnergyChange", energy, energyMax);
    }


    public void _on_Timer_timeout()
    {
        if (isPlaced && isDay)
        {
            AddEnergy(sunPower*timer.WaitTime);
            PrintEnergy();
        }
        EmitSignal("EnergyChange", energy, energyMax);
    }


    private void AddEnergy(float amount)
    {
        energy += amount;
        if (energy>energyMax)
            energy = energyMax;
    }

    private void RemoveEnergy(float amount)
    {
        energy -= amount;
        if (energy<0)
            energy = 0;
    }

    public void PrintEnergy()
    {
            GD.Print("Le panneau solaire "+id+" est a "+energy+"/"+energyMax+" d'energie.");
    }

}
