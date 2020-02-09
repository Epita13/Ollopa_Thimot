using Godot;
using System;

public class Block
{
    public enum Type 
    {
        Air = -1,
        Stone = 0,
        Grass = 1,
        Dirt = 2
    }
    public static int GetIDTile(Type type)
    {
        return (int) type;
    }

    public int x,y;
    public Type type;

    public Block(Type type, int x, int y){
        this.x = x;
        this.y = y;
        this.type = type;
    }

}
