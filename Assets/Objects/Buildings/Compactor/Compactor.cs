using Godot;
using System;

public class Compactor : Building
{
    public static int nbCompactor;

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
    
    public Compactor() : base (200, 100.0f)
    {
    }

    public override void _EnterTree()
    {
        id = nbCompactor;
        nbCompactor += 1;
    }
    
}
