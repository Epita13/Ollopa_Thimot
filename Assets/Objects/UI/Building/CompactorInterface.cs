using Godot;
using System;

public class CompactorInterface : BuildingInterface
{
    private Compactor compactor;

    private Usable.Type blockSelected;

    private ItemList blocksSelector;
    
    /*Description*/
    private TextureRect imageDesc;
    private Label titleDesc;
    private Label energyDesc;
    private Label descriptionDesc;
    private Control itemListDesc;
    private PackedScene itemBox = GD.Load<PackedScene>("res://Assets/Objects/UI/Building/Other/ItemBox.tscn");
    
    private Button btnCompact;
    
    public override void _EnterTree()
    {
        compactor = (Compactor)building;

        blocksSelector = GetNode<ItemList>("back/BlocksList");
        
        imageDesc = GetNode<TextureRect>("back/BlockDescription/ImageBlock/TextureRect");
        titleDesc = GetNode<Label>("back/BlockDescription/Description/HBox/Title");
        energyDesc = GetNode<Label>("back/BlockDescription/Description/HBox/Energy");
        descriptionDesc = GetNode<Label>("back/BlockDescription/Description/Description");
        itemListDesc = GetNode<Control>("back/BlockDescription/Description/Items");

        btnCompact = GetNode<Button>("back/BlockDescription/BtnCompact");
        
        InitBlocksSelector();
        _on_BlocksList_item_selected(0);
        RefreshBtnCompact();

    }
    
    public void _on_BlocksList_item_selected(int index)
    {
        Usable.Type type = (Usable.Type)Enum.Parse(typeof(Usable.Type), blocksSelector.GetItemText(index));
        if (Usable.category[(int)type] == Usable.Category.Block)
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
    
    private void SetDescription(Usable.Type type)
    {
        imageDesc.Texture = Usable.textures[(int)type];
        titleDesc.Text = type.ToString();
        energyDesc.Text = "-> " + Usable.energyToCreat[type] + "e";
        descriptionDesc.Text = "Health : " + Block.durabilities[Usable.blocks[type]];
        ClearItemsList();
        foreach (var loot in Usable.crafts[type].loots)
        {
            Control it = (Control) itemBox.Instance();
            it.GetNode<TextureRect>("img").Texture = Item.textures[(int) loot.type];
            it.GetNode<Label>("texte").Text = Player.inventoryItems.GetItemCount(loot.type)+"/"+loot.amount;
            itemListDesc.AddChild(it);
        }
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
        for (int i = 0; i < Usable.nbUsables; i++)
        {
            Usable.Type type = (Usable.Type) i;
            if (Usable.category[(int) type] == Usable.Category.Block)
            {
                blocksSelector.AddItem(type.ToString(), Usable.textures[(int)type]);
            }
        }
    }
    
    private void RefreshBtnCompact()
    {
        if (Drop.PlayerCanCraft(Usable.crafts[blockSelected]) && compactor.energy >= Usable.energyToCreat[blockSelected])
        {
            btnCompact.Disabled = false;
        }
        else
        {
            btnCompact.Disabled = true;
        }
    }

    public void _on_BtnCompact_button_down()
    {
        if (Drop.PlayerCanCraft(Usable.crafts[blockSelected]) &&
            compactor.energy >= Usable.energyToCreat[blockSelected])
        {
            foreach (var loot in Usable.crafts[blockSelected].loots)
            {
                Player.inventoryItems.Remove(loot.type, loot.amount);
            }
            compactor.RemoveEnergy(Usable.energyToCreat[blockSelected]);
            Player.inventoryUsables.Add(blockSelected, 1);
        }

        SetDescription(blockSelected);
    }
}
