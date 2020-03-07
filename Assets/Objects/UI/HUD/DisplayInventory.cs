using Godot;
using System;

public class DisplayInventory : ItemList
{
    public override void _Ready()
    {
        ImageTexture test = new ImageTexture();
        test.ResourcePath = "D:/thimo/Documents/EPITA/Ollopa/Ollopa_Thimot/Assets/Ressources/Imgs/Items/dirt.png";
        AddItem("test", new ImageTexture(), true);
    }
    
    public override void _Process(float delta)
    {
        //Label test = GetNode("Item1").GetNode<Label>("Quantity");
        //test.Text = "je fais un test";
    }
}
