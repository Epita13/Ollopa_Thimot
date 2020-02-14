using Godot;
using System;

public class ItemBox : NinePatchRect
{
    [Export] private int id;
    private int amount;

    private TextureRect img;
    private Label label;

    public override void _Ready()
    {
            img = GetNode<TextureRect>("Vbox/TRect/Image");
            label = GetNode<Label>("Vbox/Count");
    }

    public void Change(int[] values)
    {
        int UsableId = values[id];
        img.Texture = Usable.textures[UsableId];
        string txt = "";
        if (Usable.category[UsableId]==Usable.Category.Tool)
            txt = "-/-";
        else
            txt = Player.inventoryUsables.GetItemCount((Usable.Type)UsableId).ToString();
        label.Text = txt;
    }
}
