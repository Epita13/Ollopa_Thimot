using Godot;
using System;

public class GrinderInterface : BuildingInterface
{
    private Grinder grinder;

    private Item.Type blockSelected;

    private ItemList blocksSelector;
    
    /*Description*/
    private TextureRect imageDesc;
    private Label titleDesc;
    private Label energyDesc;
    private Label descriptionDesc;
    private ItemList inventory;
    private PackedScene itemBox = GD.Load<PackedScene>("res://Assets/Objects/UI/Building/Other/ItemBox.tscn");
    
    private Button btnGrind;
    
    public override void _EnterTree()
    {
        grinder = (Grinder)building;

        blocksSelector = GetNode<ItemList>("back/BlocksList");
        
        imageDesc = GetNode<TextureRect>("back/BlockDescription/ImageBlock/TextureRect");
        titleDesc = GetNode<Label>("back/BlockDescription/Description/HBox/Title");
        energyDesc = GetNode<Label>("back/BlockDescription/Description/HBox/Energy");
        descriptionDesc = GetNode<Label>("back/BlockDescription/Description/Description");
        inventory = GetNode<ItemList>("back3/Inventory");
        inventory.MaxColumns = 5;
        inventory.FixedIconSize = new Vector2(64,64);

        btnGrind = GetNode<Button>("back/BlockDescription/BtnGrind");
        
        InitBlocksSelector();
        _on_BlocksList_item_selected(0);
        Refresh();

    }
    
    public void _on_BlocksList_item_selected(int index)
    {
        Item.Type type = Item.grindable[index];
        blockSelected = type;
        SetDescription(type);
        Refresh();
        
    }
    
    public override void _Process(float delta)
    {
        Refresh();
    }
    
    private void SetDescription(Item.Type type)
    {
        imageDesc.Texture = Item.textures[(int)type];
        titleDesc.Text = type.ToString();
        energyDesc.Text = "-> " + Grinder.power + "e";
        descriptionDesc.Text = "With " + Item.ToComposite[blockSelected] + " of " + blockSelected + ", you make " + "1 Composite";
    }

    private void InitBlocksSelector()
    {
        blocksSelector.Clear();
        foreach (var type in Item.grindable)
        {
            blocksSelector.AddItem(type.ToString(), Item.textures[(int)type], false);
        }
    }
    
    private void Refresh()
    {
        if (grinder.energy >= Grinder.power && Player.inventoryItems.GetItemCount(blockSelected) >= Item.ToComposite[blockSelected])
        {
            btnGrind.Disabled = false;
        }
        else
        {
            btnGrind.Disabled = true;
        }
        
        inventory.Clear();
        foreach (Item.Type item in Enum.GetValues(typeof(Item.Type)))
        {
            inventory.AddItem(Player.inventoryItems.GetItemCount(item).ToString(), Item.textures[(int)item]);
        }
    }

    public void _on_BtnGrind_button_down()
    {
        if (grinder.energy >= Grinder.power && Player.inventoryItems.GetItemCount(blockSelected) >= Item.ToComposite[blockSelected])
        {
            grinder.RemoveEnergy(Grinder.power);
            grinder.Grind(blockSelected);
        }

        SetDescription(blockSelected);
    }
}
