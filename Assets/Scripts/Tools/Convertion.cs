using Godot;
using System;

public static class Convertion
{

    /// Convertie une location de repere Godot vers le repere World (nombres entiers)
    public static Vector2 Location2WorldFloor(Vector2 location)
    {
        location = location / Block.cellSize;
        return new Vector2(Mathf.Floor(location.x), Mathf.Floor((1-location.y)+(Chunk.height)) );
    }

    /// Convertie une location de repere Godot vers le repere World (nombres decimals)
    public static Vector2 Location2World(Vector2 location)
    {
        location = location / Block.cellSize;
        return new Vector2(location.x, (1-location.y)+(Chunk.height) );
    }

    /// Convertie une location de repere World vers le repere Godot
    public static Vector2 World2Location(Vector2 location)
    {
        location.y = -((location.y - Chunk.height) - 1);
        location *= Block.cellSize;
        return location;
    }
    
    /// Convertie une coordonee World quelquonque dans le repere World dans les bornes (World need to be initialised)
    public static Vector2 World2WorldBorn(Vector2 location)
    {
        World.IsInitWorldTest("Convertion/World2WorldBorn");
        Vector2 res = new Vector2(0,0);
        if (location.x < 0)
        {
            res = new Vector2(location.x % (World.size*Chunk.size) + World.size*Chunk.size, location.y);
        }
        else
        {
            res = new Vector2(location.x % (World.size*Chunk.size), location.y);
        }

        return res;
    }
}
