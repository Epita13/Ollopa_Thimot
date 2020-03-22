using Godot;
using System;

public class LootTest : Node2D
{

    public override void _Ready()
    {
        Loot.Init(this);
        PlayerInputs.playerInputActive = false;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("mouse1"))
        {
            Loot.SpawnLoot(Convertion.Location2World(GetGlobalMousePosition()), Item.Type.Composite, 8);
        }
    }
}
