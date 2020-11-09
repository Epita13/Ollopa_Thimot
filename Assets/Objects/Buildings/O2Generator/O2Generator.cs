using Godot;
using System;

public class O2Generator : Building
{
    public static int nbO2Generator;
    private static float power = 1f;
    private static readonly float O2produc = 2f;
    public float o2MAX = 200f;
    public float o2 = 0;
    public float togive = 0;
    public bool on = false;
    private static float power2wake = 2 * power;
    private static float giveSpeed = 2.5f;

    
    
    /*Structure de sauvegarde*/
    public struct SaveStruct
    {
        public Building.SaveStruct buildingSave;
        public float o2;
        public bool on;
        public float togive;
    }

    public SaveStruct GetSaveStruct()
    {
        SaveStruct s = new SaveStruct();
        s.buildingSave = GetBuildingSaveStruct();
        s.togive = togive;
        s.o2 = o2;
        s.@on = @on;
        return s;
    }
    /*************************/
    
    public override void _EnterTree()
    {
        id = nbO2Generator;
        nbO2Generator++;
    }

    public void _on_Timer_timeout()
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        
        if (togive >= giveSpeed)
        {
            Player.AddOxygene(giveSpeed);
            togive -= giveSpeed;
            o2 -= giveSpeed;
        }
        else if(togive > 0)
        {
            Player.AddOxygene(togive);
            o2 -= togive;
            togive = 0;
        }

        if (on)
        {
            RemoveEnergy(power * timer.WaitTime);
            o2 += O2produc * timer.WaitTime;   
        }

        if (o2 > o2MAX)
            o2 = o2MAX;

        on = (energy >= power2wake && !on || energy > 0 && on) && o2 < o2MAX;
    }

    public O2Generator() : base(100, 150f)
    {
        
    }
}
