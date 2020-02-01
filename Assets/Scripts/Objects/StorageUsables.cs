using Godot;
using System;
using System.Collections.Generic;

public class StorageUsables : Node
{
    public int max;

    public Dictionary<Usable.Type, int> stokage;

    public StorageUsables(int max)
    {
        this.max = max;
    }

    /// Ajoute des blocks dans le stokage
    public bool Add(Usable.Type type, int amount)
    {
        if (CanAdd(type, amount))
        {
            stokage[type] += amount;
            return true;
        }
        return false;
    }
    /// Enleve des blocks de le stokage
    public bool Remove(Usable.Type type, int amount)
    {
        if (CanRemove(type, amount))
        {
            stokage[type] -= amount;
            return true;
        }
        return false;
    }

    /// Verifie si il y a assez de blocks du type type
    public bool CanRemove(Usable.Type type, int amount)
    {
        if (Usable.category[(int)type]==Usable.Category.Tool)
            return false;
        return (stokage[type]-amount >= 0);
    }

    /// Verifie si il n'y a pas trop de blocks
    public bool CanAdd(Usable.Type type, int amount)
    {
        if (Usable.category[(int)type]==Usable.Category.Tool)
            return false;
        return (GetCount()+amount <= max);
    }

    /// Recupere le nombre d'item du type type dans le stokage
    public float GetItemCount(Usable.Type type)
    {
        return stokage[type];
    }

    /// Donne le nombre de blocks au totale (compte pas les outils)
    public float GetCount()
    {
        int sum = 0;
        foreach (var c in stokage)
        {
            if (Usable.category[(int)c.Key]!=Usable.Category.Tool)
                sum += c.Value;
        }
        return sum;
    }

    /// Initialise le stockage
    private void Init()
    {
        stokage = new Dictionary<Usable.Type, int>();
        for (int i = 0; i < Usable.nbUsables; i++)
        {
            if (Usable.category[i]==Usable.Category.Tool)
            {
                stokage.Add((Usable.Type)i,1);
            }else
            {
                stokage.Add((Usable.Type)i,0);
            }
        }
    }
    
}
