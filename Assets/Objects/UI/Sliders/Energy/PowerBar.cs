using Godot;
using System;

public class PowerBar : Control
{

    private TextureRect bar;
    
    public override void _Ready()
    {
        bar = GetNode<TextureRect>("slide");
        
        SetSlide(0);
    }
    

    public void Change(float power, float powerMax)
    {
        float p = power / powerMax;
        SetSlide(p);
    }
    
    private void SetSlide(float p)
    {
        bar.Material.Set("shader_param/y", 1-p);
    }
    
    
}
