using Godot;
using System;

public class PlayerVariablesShow : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private TextureRect health;
    private TextureRect healthB;
    
    private TextureRect oxygene;
    private TextureRect oxygeneB;
    
    private TextureRect energy;
    private TextureRect energyB;

    public override void _EnterTree()
    {
        health = GetNode<TextureRect>("n/Health/Slide");
        healthB = GetNode<TextureRect>("n/Health/back");
        oxygene = GetNode<TextureRect>("n/Oxygene/Slide");
        oxygeneB = GetNode<TextureRect>("n/Oxygene/back");
        energy = GetNode<TextureRect>("n/Energy/Slide");
        energyB = GetNode<TextureRect>("n/Energy/back");
        SetHealth();
        SetOxygene();
        SetEnergy();
    }

    public override void _Process(float delta)
    {
        if (PlayerState.IsNot(PlayerState.State.Normal, PlayerState.State.Build, PlayerState.State.Link))
        {
            Visible = false;
        }
        else
        {
            Visible = true;
            SetHealth();
            SetOxygene();
            SetEnergy();
        }

    }

    private void SetHealth()
    {
        Set(health,healthB, Player.health/Player.healthMax);
    }
    private void SetOxygene()
    {
        Set(oxygene,oxygeneB, Player.oxygene/Player.oxygeneMax);
    }
    private void SetEnergy()
    {
        Set(energy,energyB, Player.energy/Player.energyMax);
    }



    private void Set(TextureRect tr, TextureRect btr, float p)
    {
        float max = btr.GetRect().Size.x;
        Vector2 newSize = new  Vector2(max * p, tr.GetRect().Size.y);
        tr.SetSize(newSize);
    }
    
}
