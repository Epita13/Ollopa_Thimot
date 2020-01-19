using Godot;
using System;

public class Building
{
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
        // A FINIR //
        return;
    }

    /// Enleve le batiment de la map
    public void Remove()
    {
        if (!isPlaced)
            return;
        this.location = new Vector2(-1,-1);
        isPlaced = false;
        // A FINIR //
        return;
    }

    // Détruit le batmiment
    public void Destroy()
    {
        Remove();
        Destroy();
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
