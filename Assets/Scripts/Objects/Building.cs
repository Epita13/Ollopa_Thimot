using Godot;
using System;
using System.Collections.Generic;

public abstract class Building : Node2D
{
    
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
        Object abstract:  Building

        /!\ Initialisation static : STRICTEMENT NECESSAIRE.
            - Utiliser la fonction Init()
            - Verification d'initialisation : le getter IsInit

        /!\ Classe Initialisées necessaire : None

        Description de l'object :
            Un batiment (Building) est un object placable dans une scene.
            Il possede de la vie et peut ainsi en perdre. 
            La classe Building est une definition generale d'un batiment et a donc d'autre classes qui herite de celui-ci.
            Les coordonnées manipulées dans cette classe sont strictement celle de Godot.

        Description des parametres:
            (static) Node parent : est la node dans laquelle les buildings vont etre instancier en tant qu'enfant de cette node.
            (static) int zIndex : est la profondeur 2D a laquelle les buildings vont etre instancier.
            (static) bool isInit : True si Building est initialisé, false sinon.
            Vector2 location : est la position du batiment dans la scene, si le batiment n'est pas placer alors : (-1,-1). 
            bool isPlaced : true si le batiment est placer dans la scene; false sinon.
            int health, maxGHealth : represente la vie du joueur et son maximum de vie.
    */


    public static Node parent;
    public static int zIndex = -1;

    public int size = 4;
    public Vector2[] corners = new Vector2[4];

    private static bool isInit = false;
    public static bool IsInit => isInit;
    public static void IsInitBuildingTest(string funcName)
    {
        if (!isInit)
            throw new UninitializedException(funcName, "Building");
    } 

    /// Initialise les variables pour le fonctionnement des batiments (OBLIGATOIRE)
    public static void Init(Node parent, int zIndex = -1)
    {
        isInit = true;
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
        IsInitBuildingTest("Place");
        if (isPlaced)
            return;
        this.location = Convertion.World2Location(location);
        corners = SetCorners(location);
        isPlaced = true;
        ZIndex = zIndex;
        Position = this.location;
        parent.AddChild(this);
        return;
    }

    /// Enleve le batiment de la map
    public void Remove()
    {
        IsInitBuildingTest("Remove");
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
    
    private Vector2[] SetCorners(Vector2 location)
    {
        Vector2[] l = new Vector2[4]
        {
            new Vector2(location.x - size / 2, location.y - size / 2),
            new Vector2(location.x - size / 2, location.y + size / 2),
            new Vector2(location.x + size / 2, location.y + size / 2),
            new Vector2(location.x + size / 2, location.y - size / 2)
        };
        return l;
    }

}
