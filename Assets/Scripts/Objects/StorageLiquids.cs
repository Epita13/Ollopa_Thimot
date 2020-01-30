using Godot;
using System;
using System.Collections.Generic;

public class StorageLiquids
{
    public float max;

    public Dictionary<Liquid.Type, float> inventory = new Dictionary<Liquid.Type, float>{
        {Liquid.Type.Water, 0.0f},
        {Liquid.Type.Oil, 0.0f},
        {Liquid.Type.Fuel, 0.0f}
    };

    public StorageLiquids(float max)
    {
        this.max = max;
    }

    /// Ajoute des items dans l'inventaire
    public void Add(Liquid.Type type, float amount)
    {
        inventory[type] += amount;
    }
    /// Enleve des items de l'inventaire 
    public void Remove(Liquid.Type type, float amount)
    {
        inventory[type] -= amount;
    }
    /// Verifie si il y a assez d'item du type type
    public bool CanRemove(Liquid.Type type, float amount)
    {
        return (inventory[type]-amount >= 0);
    }
    /// Verifie si il n'y a pas trop d'items
    public bool CanAdd(Liquid.Type type, float amount)
    {
        return (GetCount()+amount <= max);
    }
    /// Recupere le nombre d'item du type type dans l'inventaire
    public float GetItemCount(Liquid.Type type)
    {
        return inventory[type];
    }
    /// Donne le nombre d'items au totale
    public float GetCount()
    {
        float sum = 0.0f;
        foreach (var c in inventory)
        {
            sum += c.Value;
        }
        return sum;
    }
}
