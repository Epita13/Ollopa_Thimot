using Godot;
using System;

public class RefineryInterface : BuildingInterface
{
    private Refinery refinery;
    private PackedScene itemBox = GD.Load<PackedScene>("res://Assets/Objects/UI/Building/Other/ItemBox.tscn");
    private Button btnTransfer;
    private Button btnGive;
    private LineEdit entry;
    
    [Signal]
    delegate void ChangePetrolBar(float oil, float oilMax);
    [Signal]
    delegate void ChangeFuelBar(float fuel, float fuelMax);
    
    public override void _EnterTree()
    {
        refinery = (Refinery)building;
        btnGive = GetNode<Button>("back/BtnGive");
        btnTransfer = GetNode<Button>("back/BtnTransfer");
        entry = GetNode<LineEdit>("back/LineEdit");
        entry.Text = "0";
        RefreshBtn();

    }

    public override void _Process(float delta)
    {
        RefreshBtn();
    }

    private void RefreshBtn()
    {
        btnGive.Disabled = !(refinery.fuel > 0);
    }

    public void _on_BtnGive_button_down()
    {
        float nb = Math.Min(refinery.fuel, Player.inventoryLiquids.max - Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel));
        if (refinery.fuel > 0 && Player.inventoryLiquids.CanAdd(Liquid.Type.Fuel, nb))
        {
            refinery.togive += nb;
        }
    }


    public void _on_BtnTransfer_button_down()
    {
        float transfer = 0;
        float.TryParse(entry.Text, out transfer);
        if(Math.Min(transfer, refinery.oilMAX - refinery.oil) <= Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil))
            refinery.toadd += Math.Min(transfer, refinery.oilMAX - refinery.oil);
    }
    
    public void _on_Timer_timeout()
    {
        EmitSignal("ChangePetrolBar", refinery.oil, refinery.oilMAX);
        EmitSignal("ChangeFuelBar", refinery.fuel, refinery.fuelMAX);
    }
}
