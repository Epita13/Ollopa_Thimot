using Godot;
using System;

public class Printer3DInterface : BuildingInterface
{

    private Printer3D printer3D;

    private Building.Type buildingSelected;

    private ItemList buildingSelector;
    
    /*Description*/
    private TextureRect imageDesc;
    private Label titleDesc;
    private Label timeDesc;
    private Label energyDesc;
    private Label descriptionDesc;
    private Control itemListDesc;
    private PackedScene itemBox = GD.Load<PackedScene>("res://Assets/Objects/UI/Building/Other/ItemBox.tscn");

    /*Controls*/
    private Button btnPrint;
    private Button btnPause;
    private Button btnCancel;
    private Button btnClaim;
    private ProgressBar progressBar;
    private Label stateLabel;

    public override void _Ready()
    {
        printer3D = (Printer3D)building;

        buildingSelector = GetNode<ItemList>("back/BuildingList");
        
        imageDesc = GetNode<TextureRect>("back/BuildingDescription/ImageBuilding/TextureRect");
        titleDesc = GetNode<Label>("back/BuildingDescription/Description/HBox/Title");
        timeDesc = GetNode<Label>("back/BuildingDescription/Description/HBox/VBox/Time");
        energyDesc = GetNode<Label>("back/BuildingDescription/Description/HBox/VBox/Energy");
        descriptionDesc = GetNode<Label>("back/BuildingDescription/Description/Description");
        itemListDesc = GetNode<Control>("back/BuildingDescription/Description/Items");

        btnPrint = GetNode<Button>("back/BuildingDescription/BtnPrint");
        
        btnPause = GetNode<Button>("back/VBox/HBox/BtnPause");
        btnCancel = GetNode<Button>("back/VBox/HBox/BtnCancel");
        btnClaim = GetNode<Button>("back/VBox/HBox/BtnClaim");
        progressBar = GetNode<ProgressBar>("back/VBox/Progress");
        stateLabel = GetNode<Label>("back/VBox/HBox/State");
        
        
        _on_BuildingList_item_selected(0);
        InitBuildingSelector();
        RefreshBtns();

    }

    public override void _Process(float delta)
    {
        RefreshBtns();
    }

    public void _on_BuildingList_item_selected(int index)
    {
        if (index < Building.nbBuildings)
        {
            Building.Type type = (Building.Type) index;
            SetDescription(type);
            buildingSelected = type;
            RefreshBtnPrint();
        }
    }

    private void SetDescription(Building.Type type)
    {
        imageDesc.Texture = Building.textures[type];
        titleDesc.Text = type.ToString();
        timeDesc.Text = Building.times2Create[type].ToString() + "s";
        energyDesc.Text = "-> " + (Building.times2Create[type]*Printer3D.power) + "e";
        descriptionDesc.Text = Building.descriptions[type];
        ClearItemsList();
        foreach (var loot in Building.crafts[type].loots)
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

    private void InitBuildingSelector()
    {
        buildingSelector.Clear();
        for (int i = 0; i < Building.nbBuildings; i++)
        {
            Building.Type type = (Building.Type) i;
            buildingSelector.AddItem(type.ToString(), Building.textures[type]);
        }
    }

    private void RefreshBtnPrint()
    {
        if (Drop.PlayerCanCraft(Building.crafts[buildingSelected]) && !printer3D.isPrinting)
        {
            btnPrint.Disabled = false;
        }
        else
        {
            btnPrint.Disabled = true;
        }
    }
    private void RefreshBtnPause()
    {
        if (printer3D.isPrinting && !printer3D.isFinish)
        {
            btnPause.Disabled = false;
        }
        else
        {
            btnPause.Disabled = true;
        }
        if (printer3D.isInPause)
        {
            btnPause.Text = "RE";
        }
        else
        {
            btnPause.Text = "PAUSE";
        }
    }
    
    private void RefreshBtnCancel()
    {
        if (printer3D.isPrinting && !printer3D.isFinish)
        {
            btnCancel.Disabled = false;
        }
        else
        {
            btnCancel.Disabled = true;
        }
    }
    
    private void RefreshBtnClaim()
    {
        if (printer3D.isPrinting && printer3D.isFinish)
        {
            btnClaim.Disabled = false;
        }
        else
        {
            btnClaim.Disabled = true;
        }
    }
    
    private void RefreshProgress()
    {
        if (printer3D.isPrinting)
        {
            float p = printer3D.printingLevel / Building.times2Create[printer3D.printingType] * 100;
            progressBar.Value = p;
        }
        else
        {
            progressBar.Value = 0;
        }
    }
    
    private void RefreshLabelState()
    {
        if (printer3D.isPrinting)
        {
            string txt = "(" + printer3D.printingType.ToString() + ") ";
            if (printer3D.isFinish)
                txt += "finished.";
            else if (printer3D.isInPause)
                txt += "is in pause.";
            else
                txt += "is printing";
            stateLabel.Text = txt;
        }
        else
        {
            stateLabel.Text = "Not Printing.";
        }
    }
    
    private void RefreshBtns()
    {
        RefreshBtnPrint();
        RefreshBtnPause();
        RefreshBtnCancel();
        RefreshBtnClaim();
        RefreshProgress();
        RefreshLabelState();
    }


    public void _on_BtnPrint_button_down()
    {
        if (!printer3D.isPrinting && Drop.PlayerCanCraft(Building.crafts[buildingSelected]))
        {
            foreach (var loot in Building.crafts[buildingSelected].loots)
            {
                Player.inventoryItems.Remove(loot.type, loot.amount);
            }
            printer3D.Print(buildingSelected);
        }

        SetDescription(buildingSelected);
    }

    public void _on_BtnPause_button_down()
    {
        if (printer3D.isPrinting && !printer3D.isFinish)
        {
            printer3D.PauseOrResume();
        }
    }

    public void _on_BtnCancel_button_down()
    {
        if (printer3D.isPrinting && !printer3D.isFinish)
        {
            foreach (var loot in Building.crafts[printer3D.printingType].loots)
            {
                Player.inventoryItems.Add(loot.type, loot.amount);
            }
            printer3D.Cancel();
        }
        SetDescription(buildingSelected);
    }

    public void _on_BtnClaim_button_down()
    {
        if (printer3D.isPrinting && printer3D.isFinish)
        {
            Player.inventoryBuildings.Add(printer3D.printingType, 1);
            printer3D.Claim();
        }
    }
}
