using Godot;
using System;
using System.Reflection;

public class InfirmaryInterface : BuildingInterface
{
    private Infirmary infirmary;
    private Item.Type blockSelected;
    private ItemList blocksSelector;

    /*Description*/
    private TextureRect imageDesc;
    private Label titleDesc;
    private Label energyDesc;
    private Label descriptionDesc;
    private Control itemListDesc;
    private PackedScene itemBox = GD.Load<PackedScene>("res://Assets/Objects/UI/Building/Other/ItemBox.tscn");
    
    private Button btnHeal;
    
    public override void _EnterTree()
    {
        infirmary = (Infirmary)building;
        
        blocksSelector = GetNode<ItemList>("back/BlocksList");
        imageDesc = GetNode<TextureRect>("back/BlockDescription/ImageBlock/TextureRect");
        titleDesc = GetNode<Label>("back/BlockDescription/Description/HBox/Title");
        energyDesc = GetNode<Label>("back/BlockDescription/Description/HBox/Energy");
        descriptionDesc = GetNode<Label>("back/BlockDescription/Description/Description");
        itemListDesc = GetNode<Control>("back/BlockDescription/Description/Items");

        btnHeal = GetNode<Button>("back/BlockDescription/BtnHeal");
        
        InitBlocksSelector();
        _on_BlocksList_item_selected(0);
        RefreshBtnCompact();

    }

    public void _on_BlocksList_item_selected(int index)
    {
        Item.Type type = (Item.Type)Enum.Parse(typeof(Item.Type), blocksSelector.GetItemText(index));
        if (Item.item2heal.Contains(type))
        {
            SetDescription(type);
            blockSelected = type;
            RefreshBtnCompact();
        }
    }
    
    public override void _Process(float delta)
    {
        RefreshBtnCompact();
    }
    
    private void SetDescription(Item.Type type)
    {
        imageDesc.Texture = Item.textures[(int)type];
        titleDesc.Text = type.ToString();
        energyDesc.Text = "-> " + infirmary.energy2heal + "e";
        descriptionDesc.Text = "Health : +" + Item.healingPower[type];
        ClearItemsList();
        Control it = (Control) itemBox.Instance();
        it.GetNode<TextureRect>("img").Texture = Item.textures[(int)type];
        it.GetNode<Label>("texte").Text = Player.inventoryItems.GetItemCount(type) +"/ 1";
        itemListDesc.AddChild(it);
    }
    
    private void ClearItemsList()
    {
        foreach (var child in itemListDesc.GetChildren())
        {
            ((Node)child).QueueFree();
        }
    }
    
    private void InitBlocksSelector()
    {
        blocksSelector.Clear();
        foreach (Item.Type item in Enum.GetValues(typeof(Item.Type)))
        {
            if (Item.item2heal.Contains(item))
            {
                blocksSelector.AddItem(item.ToString(), Item.textures[(int)item]);
            }
        }
    }
    
    private void RefreshBtnCompact()
    {
        if (Player.inventoryItems.GetItemCount(blockSelected) > 0 && infirmary.energy >= infirmary.energy2heal)
        {
            btnHeal.Disabled = false;
        }
        else
        {
            btnHeal.Disabled = true;
        }
    }

    public void _on_BtnHeal_button_down()
    {
        if (Player.inventoryItems.GetItemCount(blockSelected) > 0 && infirmary.energy >= infirmary.energy2heal)
        {
            Player.inventoryItems.Remove(blockSelected, 1);
            infirmary.RemoveEnergy(infirmary.energy2heal);
            infirmary.togive += Item.healingPower[blockSelected];
        }

        SetDescription(blockSelected);
    }
}
