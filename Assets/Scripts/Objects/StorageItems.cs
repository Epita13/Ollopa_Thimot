using Godot;
using System;
using System.Collections.Generic;

public class StorageItems : Node
{
    public int max;

    public Dictionary<Item.Type, int> inventory = new Dictionary<Item.Type, int>{
        {Item.Type.Composite, 0},
        {Item.Type.Wood, 0},
        {Item.Type.Dirt, 0}
    };

    public StorageItems(int max)
    {
        this.max = max;
    }

    /// Ajoute des items dans l'inventaire
    public void Add(Item.Type type, int amount)
    {
        inventory[type] += amount;
    }
    /// Enleve des items de l'inventaire 
    public void Remove(Item.Type type, int amount)
    {
        inventory[type] -= amount;
    }
    /// Verifie si il y a assez d'item du type type
    public bool CanRemove(Item.Type type, int amount)
    {
        return (inventory[type]-amount >= 0);
    }
    /// Verifie si il n'y a pas trop d'items
    public bool CanAdd(Item.Type type, int amount)
    {
        return (GetCount()+amount <= max);
    }
    /// Recupere le nombre d'item du type type dans l'inventaire
    public float GetItemCount(Item.Type type)
    {
        return inventory[type];
    }
    /// Donne le nombre d'items au totale
    public float GetCount()
    {
        int sum = 0;
        foreach (var c in inventory)
        {
            sum += c.Value;
        }
        return sum;
    }
}
