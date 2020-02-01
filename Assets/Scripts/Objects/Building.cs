using Godot;
using System;
using System.Collections.Generic;

public class Building : Node2D
{

    /*
        La classe Building : Represente un batiment qui peut etre placer sur une scene.
        Le batiment possede de la vie.
        
        IMPORTANT : Pour le fonctionnement de la classe (pour le placement des batiments) il faut tout d'abord initialiser les variable static avec la fonction Init() sinon ERREUR.
    */


    // Enumeration : Type de batiment disponible
    public enum Type
    {
        SolarPanel,
		Storage
    }
    // Dictionaire : Stock les scnenes batiment en fonction du type de batiment
    public static Dictionary<Type, PackedScene> prefabs = new Dictionary<Type, PackedScene>
    {
        {Type.SolarPanel, GD.Load<PackedScene>("res://Assets/Objects/Buildings/SolarPanel/SolarPanel.tscn")},
		{Type.Storage, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Storage/Storage.tscn")}
    };




    /*
        Node parent : la Node a l'interieur de laquelle les batiments vont être placés.
        zIndex : La profondeur z des batiments qui vont être placés.
    */
    public static Node parent;
    public static int zIndex = -1;
    /// Initialise les variables pour le fonctionnement des batiments (OBLIGATOIRE)
    public static void Init(Node parent, int zIndex = -1)
    {
        Building.parent = parent;
        Building.zIndex = zIndex;
    }


    public Vector2 location;
    public bool isPlaced = false;

    public int healthMax = 100;
    public int health;

    public Building(int healthMax)
    {
        this.health = healthMax;
        this.healthMax = healthMax;
    }

    /// Place le batiment sur la map
    public void Place(Vector2 location)
    {
        if (isPlaced)
            return;
        this.location = location;
        isPlaced = true;
        SetZIndex(zIndex);
        SetPosition(location);
        parent.AddChild(this);
        return;
    }

    /// Enleve le batiment de la map
    public void Remove()
    {
        if (!isPlaced)
            return;
        this.location = new Vector2(-1,-1);
        isPlaced = false;
        parent.RemoveChild(this);
        return;
    }

    // Détruit le batmiment
    public void Destroy()
    {
        Remove();
        Free();
    }

    /// Donne des dégats à la structure
    public void Damage(int value)
    {
        health -= value;
        if (health<0)
            health = 0;
    }

    /// Donne de la vie a la structure
    public void Healing(int value)
    {
        health += value;
        if (health>healthMax)
            health = healthMax;
    }

}
