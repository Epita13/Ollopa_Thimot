using Godot;
using System;

public static class Player
{
    public static float healthMax = 100.0f;
    public static float health = 100.0f;

    public static float oxygeneMax = 100.0f;
    public static float oxygene = 100.0f;

    public static float energyMax = 100.0f;
    public static float energy = 100.0f;


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




    public static void PrintEnergy(){
        GD.Print("Le Joueur a " + energy + "/" + energyMax + " d'energie.");
    }
    public static void PrintOxygene(){
        GD.Print("Le Joueur a " + oxygene + "/" + oxygeneMax + " d'oxygene.");
    }
    public static void PrintHealth(){
        GD.Print("Le Joueur a " + health + "/" + healthMax + " de santé.");
    }
}
