using Godot;
using System;

public class MenuPlay : Node2D
{
    private Label labelBtn;

    public override void _Ready()
    {
        labelBtn = GetNode<Label>("CanvasLayer/labelBtn");
        labelBtn.Text = "";

        GetNode("CanvasLayer/BtnNewGame").Connect("mouse_click", this, "_on_BtnNewGame_mouse_click");
        GetNode("CanvasLayer/BtnLoadGame").Connect("mouse_click", this, "_on_BtnLoadGame_mouse_click");
    }
    
    

    public void _on_BtnSettings_mouse_entered()
    {
        labelBtn.Text = "Settings (unavailable)";
    }

    public void _on_BtnLoadGame_mouse_entered()
    {
        labelBtn.Text = "Continue";
    }

    public void _on_BtnNewGame_mouse_entered()
    {
        labelBtn.Text = "New game";
    }

    public void _on_Btn_mouse_exited()
    {
        labelBtn.Text = "";
    }

    public void _on_BtnNewGame_mouse_click()
    {
        GetTree().ChangeScene("res://Assets/Scenes/Menus/MenuNewGame.tscn");
    }

    public void _on_BtnLoadGame_mouse_click()
    {
        GetTree().ChangeScene("res://Assets/Scenes/Menus/MenuLoadGame.tscn");
    }
}