using Godot;
using System;
using System.Collections.Generic;



public class ValuesPanel : Panel
{

    List<Label> txts = new List<Label>();

    public override void _Ready()
    {
        foreach(var node in GetNode("VBox").GetChildren())
        {
            txts.Add((Label)node);
        }
    }

    public override void _Process(float delta)
    {
      txts[0].Text = "Composite : "+ Player.inventoryItems.GetItemCount(Item.Type.Composite);
      txts[1].Text = "Wood : "+ Player.inventoryItems.GetItemCount(Item.Type.Wood);
      txts[2].Text = "Stone : "+ Player.inventoryItems.GetItemCount(Item.Type.Stone);
    }
}
