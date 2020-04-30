using Godot;
using System;

public class MenuPlay : Node2D
{

    private SpinBox spb;
    private CheckBox ckb;

    private ItemList saves;
    public override void _Ready()
    {
        spb = GetNode<SpinBox>("CanvasLayer/SpinBox");
        ckb = GetNode<CheckBox>("CanvasLayer/CheckBox");
        saves = GetNode<ItemList>("CanvasLayer/saves");

        foreach (var s in Save.GetSaves())
        {
            saves.AddItem(s);
        }
    }

    public void Play()
    {
        if (ckb.Pressed)
        {
            World.SetSeed((int)spb.Value);
        }
        GetTree().ChangeScene("res://Assets/Scenes/Jeux/Game.tscn");
    }


    public void _on_saves_item_activated(int id)
    {
        string saveName = saves.GetItemText(id);
        Game.load = true;
        Game.saveName = saveName;
        GetTree().ChangeScene("res://Assets/Scenes/Jeux/Game.tscn");
    }
}