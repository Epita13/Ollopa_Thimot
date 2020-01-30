using Godot;
using System;
using System.Collections.Generic;

public class StorageUsables : Node
{
    public int max;

    public Dictionary<Usable.Type, int> inventory = new Dictionary<Usable.Type, int>{
        {Usable.Type.laser, 1},
        {Usable.Type.dirt, 0}
    }; 

    public StorageUsables(int max)
    {
        this.max = max;
    }

    /// Ajoute des blocks dans l'inventaire
    public void Add(Usable.Type type, int amount)
    {
        if (Usable.category[(int)type]!=Usable.Category.Tool)
            inventory[type] += amount;
    }
    /// Enleve des blocks de l'inventaire 
    public void Remove(Usable.Type type, int amount)
    {
        if (Usable.category[(int)type]!=Usable.Category.Tool)
            inventory[type] -= amount;
    }

    /// Verifie si il y a assez de blocks du type type
    public bool CanRemove(Usable.Type type, int amount)
    {
        if (Usable.category[(int)type]==Usable.Category.Tool)
            return false;
        return (inventory[type]-amount >= 0);
    }

    /// Verifie si il n'y a pas trop de blocks
    public bool CanAdd(Usable.Type type, int amount)
    {
        if (Usable.category[(int)type]==Usable.Category.Tool)
            return false;
        return (GetCount()+amount <= max);
    }

    /// Recupere le nombre d'item du type type dans l'inventaire
    public float GetItemCount(Usable.Type type)
    {
        return inventory[type];
    }

    /// Donne le nombre de blocks au totale (compte pas les outils)
    public float GetCount()
    {
        int sum = 0;
        foreach (var c in inventory)
        {
            if (Usable.category[(int)c.Key]!=Usable.Category.Tool)
                sum += c.Value;
        }
        return sum;
    }
}
