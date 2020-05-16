using Godot;
using System;

public class Infirmary : Building
{
    public static int nbInfirmary;
    public float energy2heal = 20f;

    public float togive = 0;

    public override void _EnterTree()
    {
        id = nbInfirmary;
        nbInfirmary++;
    }

    public override void _Process(float delta)
    {
        if (togive >= 0.1f)
        {
            Player.AddHealth(0.1f);
            togive -= 0.1f;
        }
        else if(togive > 0 && togive < 0.1f)
        {
            Player.AddHealth(togive);
            togive = 0;
        }
    }

    public Infirmary() : base(100, 200)
    {
        
    }
}
