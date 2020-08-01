using Godot;
using System;

public class Grinder : Building
{
    public static int nbGrinder;
    public static float power = 2.5f;
    private static float giveSpeed = 2f;

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
    
    
    public Grinder() : base (200, 100.0f)
    {
    }

    public override void _EnterTree()
    {
        id = nbGrinder;
        nbGrinder += 1;
    }

    public void Grind(Item.Type type)
    {
        Player.inventoryItems.Remove(type, Item.ToComposite[type]);
        Player.inventoryItems.Add(Item.Type.Composite, 1);
    }
    
    
}
