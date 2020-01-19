using Godot;
using System;

public class Block
{

    public int x,y;
    public int tileId;

    public Block(int tileId, int x, int y){
        this.x = x;
        this.y = y;
        this.tileId = tileId;
    }
}
