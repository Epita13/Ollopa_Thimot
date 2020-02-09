using Godot;
using System;

public static class Convertions
{

    /// Convertie une location de repere Godot vers le repere World (nombres entiers)
    public static Vector2 Location2WorldFloor(Vector2 location)
    {
        location = location / World.tilemp_blocks.CellSize.x;
        return new Vector2(Mathf.Floor(location.x), Mathf.Floor((1-location.y)+(Chunk.height)) );
    }

    /// Convertie une location de repere Godot vers le repere World (nombres decimals)
    public static Vector2 Location2World(Vector2 location)
    {
        location = location / World.tilemp_blocks.CellSize.x;
        return new Vector2(location.x, (1-location.y)+(Chunk.height) );
    }

    /// Convertie une location de repere World vers le repere Godot
    public static Vector2 World2Location(Vector2 location)
    {
        location.y = -((location.y - Chunk.height) - 1);
        location *= World.tilemp_blocks.CellSize.x;
        return location;
    }
}
