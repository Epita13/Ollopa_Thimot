using Godot;
using System;

public class O2Generator : Building
{
    public static int nbO2Generator;
    private static float power = 1f;
    private static readonly float O2produc = 1f;
    public float o2MAX = 500f;
    public float o2 = 0;
    public float togive = 0;
    private bool on = false;
    private static float power2wake = 2 * power;
    private static float giveSpeed = 2.5f;

    public override void _EnterTree()
    {
        id = nbO2Generator;
        nbO2Generator++;
    }

    public void _on_Timer_timeout()
    {
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

    public O2Generator() : base(100, 200)
    {
        
    }
}
