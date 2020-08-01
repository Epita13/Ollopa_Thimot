using Godot;
using System;
using System.Collections.Generic;

public class SolarPanel : Building
{

    public static int nbSolarPanel;

    /* Signal pour les voyants */
    [Signal] public delegate void EnergyChange(float energy, float energyMax);
    
    private Sprite stateSprite;
    
    
    
    /*Structure de sauvegarde*/
    public struct SaveStruct
    {
        public Building.SaveStruct buildingSave;
    }

    public SaveStruct GetSaveStruct()
    {
        SaveStruct s = new SaveStruct();
        s.buildingSave = GetBuildingSaveStruct();
        return s;
    }
    /*************************/
    

    public SolarPanel() : base (100, 200.0f)
    {
    }

    public override void _EnterTree()
    {
        id = nbSolarPanel;
        nbSolarPanel+=1;
        
        stateSprite = GetNode<Sprite>("state");
        EmitSignal("EnergyChange", energy, energyMax);
        if (Environement.cycle == Environement.TimeState.DAY)
        {
            Color color = Color.Color8(66, 190, 40);
            stateSprite.Modulate = color;
            stateSprite.GetNode<Light2D>("Light").Color = color;
        }
        else
        {
            Color color = Color.Color8(255,0,0);
            stateSprite.Modulate = color;
            stateSprite.GetNode<Light2D>("Light").Color = color;
        }
    }


    public void _on_Timer_timeout()
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        if (isPlaced && Environement.sunPower>0)
        {
            Color color = Color.Color8(66, 190, 40);
            stateSprite.Modulate = color;
            stateSprite.GetNode<Light2D>("Light").Color = color;
        }
        if (isPlaced && Environement.sunPower==0)
        {
            Color color = Color.Color8(255,0,0);
            stateSprite.Modulate = color;
            stateSprite.GetNode<Light2D>("Light").Color = color;
        }
        
        AddEnergy(Environement.sunPower * timer.WaitTime);
        TransferToLink(timer.WaitTime);
        
        EmitSignal("EnergyChange", energy, energyMax);
    }
    

}
