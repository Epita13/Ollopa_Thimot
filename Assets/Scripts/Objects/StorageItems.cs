using Godot;
using System;
using System.Collections.Generic;

public class StorageItems
{
    public int max;

    public Dictionary<Item.Type, int> stokage;

    public StorageItems(int max)
    {
        Init();
        this.max = max;
    }

    /// Ajoute des items dans le stokage
    public bool Add(Item.Type type, int amount)
    {
        if (CanAdd(type, amount))
        {
            stokage[type] += amount;
            return true;
        }
        return false;
    }
    /// Enleve des items de le stokage 
    public bool Remove(Item.Type type, int amount)
    {
        if (CanRemove(type, amount))
        {
            stokage[type] -= amount;
            return true;
        }
        return false;
    }
    /// Verifie si il y a assez d'item du type type
    public bool CanRemove(Item.Type type, int amount)
    {
        return (stokage[type]-amount >= 0);
    }
    /// Verifie si il n'y a pas trop d'items
    public bool CanAdd(Item.Type type, int amount)
    {
        return (GetCount()+amount <= max);
    }
    /// Recupere le nombre d'item du type type dans le stokage
    public int GetItemCount(Item.Type type)
    {
        return stokage[type];
    }
    /// Donne le nombre d'items au totale
    public float GetCount()
    {
        int sum = 0;
        foreach (var c in stokage)
        {
            sum += c.Value;
        }
        return sum;
    }

    /// Initialise le stockage a 0
    private void Init()
    {
        stokage = new Dictionary<Item.Type, int>();
        for (int i = 0; i < Item.nbItems; i++)
        {
            stokage.Add((Item.Type)i,0);
        }
    }

}
