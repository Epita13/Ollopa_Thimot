using Godot;
using System;

public class Spaceship : Node
{
    public const float ENERGYWIN = 1000.0f; 
    public const float FUELWIN = 500.0f;
    public const int COMPOSITEWIN = 2500;
    
    
    public static Spaceship instance;
    public static Spaceship GetInstance() => instance;

    public static StorageItems itemStokage = new StorageItems(500);
    public static StorageUsables usableStorage = new StorageUsables(200);
    public static int composite = 0;
    public static float fuel = 0;
    public static float energy = 0;
    
    public override void _Ready()
    {
        instance = this;
    }

    public static void AddFuel(float amount)
    {
        fuel += amount;
    }

    public static void AddEnergy(float amount)
    {
        energy += amount;
    }

    public static void AddComposite(int amount)
    {
        composite += amount;
    }

    public static bool HasEnougthComposite(int n)
    {
        int nb = GetNbCompositeStorage();
        return nb >= n;
    }

    public static int GetNbCompositeStorage()
    {
        return  itemStokage.GetItemCount(Item.Type.Composite);
    }

}
