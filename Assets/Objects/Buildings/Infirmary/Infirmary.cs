using Godot;
using System;

public class Infirmary : Building
{
    public static int nbInfirmary;
    public float energy2heal = 20f;

    public float togive = 0;

    
    /*Structure de sauvegarde*/
    public struct SaveStruct
    {
        public Building.SaveStruct buildingSave;
        public float togive;
    }

    public SaveStruct GetSaveStruct()
    {
        SaveStruct s = new SaveStruct();
        s.buildingSave = GetBuildingSaveStruct();
        s.togive = togive;
        return s;
    }
    /*************************/
    
    public override void _EnterTree()
    {
        id = nbInfirmary;
        nbInfirmary++;
    }

    public void _on_Timer_timeout(float delta)
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        
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

    public Infirmary() : base(100, 100)
    {
        
    }
}
