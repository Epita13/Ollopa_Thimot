using Godot;
using System;

public class Compactor : Building
{
    public static int nbCompactor;

    
    
    public Compactor() : base (200, 200.0f)
    {
    }

    public override void _EnterTree()
    {
        id = nbCompactor;
        nbCompactor += 1;
    }
    
}
