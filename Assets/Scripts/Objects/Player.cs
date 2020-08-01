using Godot;
using System;
using System.Collections.Generic;

public static class Player
{
    

    
    public static float healthMax = 100.0f;
    public static float health = 100.0f;

    public static float oxygeneMax = 100.0f;
    public static float oxygene = 100f;
    public static float oxygeneLoss = 0.4f;
    public static float oxygeneDamage = 3.0f;

    public static float energyMax = 100.0f;
    public static float energy = 100.0f;
    public static float energyDamage = 0.1f;

    // Ex : laser, blocks..
    public static int inventoryUsablesSize = 100;
    public static  StorageUsables inventoryUsables = new StorageUsables(inventoryUsablesSize);
    // Ex : bois, composite (matieres premieres)
    public static int inventoryItemsSize = 20000;
    public static StorageItems inventoryItems = new StorageItems(inventoryItemsSize);
    // Ex : eau, petrole (Liquide)
    public static float inventoryLiquidsSize = 100.0f;
    public static StorageLiquids inventoryLiquids = new StorageLiquids(inventoryLiquidsSize);
    public static int inventoryBuildingsSize = 10;
    public static StorageBuildings inventoryBuildings = new StorageBuildings(inventoryBuildingsSize);


    public static Usable.Type UsableSelected = Usable.Type.Laser;
    public static Building.Type BuildingSelected = Building.Type.SolarPanel;

    /// Ajoute de la vie au joueur. 
    public static void AddHealth(float amount)
    {
        health += amount;
        if (health>healthMax)
            health = healthMax;
    }
    /// Enleve de la vie au joueur.
    public static void RemoveHealth(float amount)
    {
        PlayerMouvements.PlaySound(Sounds.Type.PlayerHurt);
        health -= amount;
        if (health<0)
            health = 0;
    }
    /// Full vie au joueur.
    public static void FillHealth()
    {
        health = healthMax;
    }

    /// Ajoute de l'oxygene au joueur. 
    public static void AddOxygene(float amount)
    {
        oxygene += amount;
        if (oxygene>oxygeneMax)
            oxygene = oxygeneMax;
    }
    /// Enleve de l'oxygene au joueur.
    public static void RemoveOxygene(float amount)
    {
        oxygene -= amount;
        if (oxygene<0)
            oxygene = 0;
    }
    /// Full oxygene au joueur.
    public static void FillOxygene()
    {
        oxygene = oxygeneMax;
    }

    /// Ajoute de l'energie au joueur. 
    public static void AddEnergy(float amount)
    {
        energy += amount;
        if (energy>energyMax)
            energy = energyMax;
    }
    /// Enleve de l'energie au joueur.
    public static void RemoveEnergy(float amount)
    {
        energy -= amount;
        if (energy<0)
            energy = 0;
    }
    /// Full energie au joueur.
    public static void FillEnergy()
    {
        energy = energyMax;
    }

    public static void Die()
    {
        BuildingInterface.CloseInterface();
        UI_PlayerInventory2.Close();
        PlayerState.SetState(PlayerState.State.Dead);
    }
    public static void Revive()
    {
        if (health <= 0)
        {
            FillHealth();
            FillOxygene();
            FillEnergy();
            inventoryUsables = new StorageUsables(inventoryUsablesSize);
            inventoryItems = new StorageItems(inventoryItemsSize);
            inventoryLiquids = new StorageLiquids(inventoryLiquidsSize);
            inventoryBuildings = new StorageBuildings(inventoryBuildingsSize);
            UsableSelected = Usable.Type.Laser;
            BuildingSelected = Building.Type.SolarPanel;
            PlayerMouvements.Teleport(World.spawn.x, World.spawn.y);
            PlayerMouvements.canMove = true;
        }
    }
}
