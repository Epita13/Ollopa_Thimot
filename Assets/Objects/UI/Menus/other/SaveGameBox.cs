using Godot;
using System;

public class SaveGameBox : Panel
{
    [Signal]
    delegate void buttonClick(string save);
    
    private TextureRect tr;
    private Label title;
    private Label time;

    public string saveName;

    public override void _Ready()
    {
        tr = GetNode<TextureRect>("screen");
        title = GetNode<Label>("title");
        time = GetNode<Label>("time");
        title.Text = saveName;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("mouse1") && onButton)
        {
            EmitSignal("buttonClick", saveName);
        }
    }

    private bool onButton;
    public void _on_Area_mouse_entered()
    {
        onButton = true;
    }

    public void _on_Area_mouse_exited()
    {
        onButton = false;
    }

    public void SetOutline()
    {
        Modulate = Color.Color8(100,100,100);
    }
    public void ResetOutline()
    {
        Modulate = Color.Color8(255,255,255);
    }
}
