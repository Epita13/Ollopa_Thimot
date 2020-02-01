using Godot;
using System;
using System.Collections.Generic;

public class StorageLiquids
{
    public float max;

    public Dictionary<Liquid.Type, float> stokage;

    public StorageLiquids(float max)
    {
        this.max = max;
    }

    /// Ajoute des items dans le stokage
    public bool Add(Liquid.Type type, float amount)
    {
        if (CanAdd(type, amount))
        {
            stokage[type] += amount;
            return true;
        }
        return false;
    }
    /// Enleve des items de le stokage 
    public bool Remove(Liquid.Type type, float amount)
    {
        if (CanRemove(type, amount))
        {
            stokage[type] -= amount;
            return true;
        }
        return false;
    }
    /// Verifie si il y a assez d'item du type type
    public bool CanRemove(Liquid.Type type, float amount)
    {
        return (stokage[type]-amount >= 0);
    }
    /// Verifie si il n'y a pas trop d'items
    public bool CanAdd(Liquid.Type type, float amount)
    {
        return (GetCount()+amount <= max);
    }
    /// Recupere le nombre d'item du type type dans le stokage
    public float GetItemCount(Liquid.Type type)
    {
        return stokage[type];
    }
    /// Donne le nombre d'items au totale
    public float GetCount()
    {
        float sum = 0.0f;
        foreach (var c in stokage)
        {
            sum += c.Value;
        }
        return sum;
    }

    /// Initialise le stockage a 0
    private void Init()
    {
        stokage = new Dictionary<Liquid.Type, float>();
        for (int i = 0; i < Liquid.nbLiquids; i++)
        {
            stokage.Add((Liquid.Type)i,0);
        }
    }
}
