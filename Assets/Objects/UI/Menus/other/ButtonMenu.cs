using Godot;
using System;

public class ButtonMenu : TextureRect
{
    [Export] public float coef;

    [Signal]
    delegate void mouse_click();
    
    public override void _EnterTree()
    {
        Connect("mouse_entered", this, "_on_mouse_entered");
        Connect("mouse_exited", this, "_on_mouse_exited");
        Material = (Material)Material.Duplicate();
    }
    
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("mouse1") && onButton)
        {
            EmitSignal("mouse_click");
        }
    }

    private bool onButton = false;

    public void _on_mouse_entered()
    {
        SetOutline(coef);
        onButton = true;
    }
    
    public void _on_mouse_exited()
    {
        ResetOutline();
        onButton = false;
    }
    
    public void ResetOutline()
    {
        Material.Set("shader_param/width", 0.0f);
    }

    public void SetOutline(float w)
    {
        Material.Set("shader_param/width", w);
    }
}
