using Godot;
using System;

public class StorageBuildings 
{
    //inventaire des batiments

    private int max;
	
    //ici déclaration des attributs
	
    public System.Collections.Generic.Dictionary<Building.Type, int> storage;
	
    //ici la classe
    public StorageBuildings(int max)
    {
        //initialisation du dictionnaire
        storage = new System.Collections.Generic.Dictionary<Building.Type, int>();
        foreach(Building.Type type in Enum.GetValues(typeof(Building.Type)))
        {
            storage[type] = 0;
        }

        this.max = max;
    }
	
    //ici les méthodes
	
    //Ajouter un batiment
    public bool Add(Building.Type type, int nb)
    {
        bool res = false;
        if (storage[type] + nb < max)
        {
            res = true;
            storage[type] += nb;
        }
        return res;
    }
	
    //Enlever un batiment
    public bool Remove(Building.Type type, int nb)
    {
        bool res = false;
        if (storage[type] - nb >= 0)
        {
            res = true;
            storage[type] -= nb;
        }
        return res;
    }
	
    /// Recupere le nombre de batiments du type demandé
    public int GetItemCount(Building.Type type)
    {
        return storage[type];
    }
}
