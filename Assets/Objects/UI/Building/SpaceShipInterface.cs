using Godot;
using System;

public class SpaceShipInterface : Control
{
    private static Control inventory;
    private static Button Launch;
    private static Button BtnCompoT;
    private static Button BtnFuelT;
    private static Button BtnEnerT;


    private static bool CompositeOn = false;
    private static bool FuelOn = false;
    private static bool EnergyeOn = false;
    public static void open_interface()
    {
        inventory = (Control) GD.Load<PackedScene>("res://Assets/Objects/UI/Building/SpaceShipInterface.tscn").Instance(); 
        SpaceShip.canvas.AddChild(inventory);
        SpaceShip.inventoryOpen = true;
        PlayerState.SetState(PlayerState.State.BuildingInterface);
        Launch = inventory.GetNode("back").GetNode<Button>("Launch");
        BtnCompoT = inventory.GetNode<Button>("back/ItemBox/BtnTran");
        BtnFuelT = inventory.GetNode<Button>("back/FuelBar/BtnTran");
        BtnEnerT = inventory.GetNode<Button>("back/EnergyBar/BtnTran");

        RefreshLaunchBTN();
    }
    
    public static void close_interface()
    {
        CompositeOn = false;
        FuelOn = false;
        EnergyeOn = false;
        inventory.QueueFree();
        SpaceShip.inventoryOpen = false;
        PlayerState.SetState(PlayerState.State.Normal);
    }


    
    public void _on_Launch_button_down()
    {
        SpaceShip.close_interface();
        PlayerState.state = PlayerState.State.Finish;
        Save.DeleteSave(Game.saveName);
        GetTree().ChangeScene("res://Assets/Objects/UI/Menus/EndMenu.tscn");
    }

    public override void _Process(float delta)
    {
        GetNode<FuelBar>("back2/FuelBar").Change(Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel), Player.inventoryLiquidsSize);
        GetNode<EnergyBar>("back2/EnergyBar").Change(Player.energy, Player.energyMax);
        GetNode<Label>("back2/ItemBox/texte").Text = Player.inventoryItems.GetItemCount(Item.Type.Composite).ToString();

        GetNode<FuelBar>("back/FuelBar").Change(SpaceShip.fuel, SpaceShip.FUELWIN);
        GetNode<EnergyBar>("back/EnergyBar").Change(SpaceShip.energy, SpaceShip.ENERGYWIN);
        GetNode<Label>("back/ItemBox/texte").Text = SpaceShip.composite + "/" + SpaceShip.COMPOSITEWIN;

        if (SpaceShip.energy >= SpaceShip.ENERGYWIN)
        {
            BtnEnerT.Disabled = true;
            BtnEnerT.Text = "OFF";
            EnergyeOn = false;
        }
        if (SpaceShip.fuel >= SpaceShip.FUELWIN)
        {
            BtnFuelT.Disabled = true;
            BtnFuelT.Text = "OFF";
            FuelOn = false;
        }
        if (SpaceShip.composite >= SpaceShip.COMPOSITEWIN)
        {
            BtnCompoT.Disabled = true;
            BtnCompoT.Text = "OFF";
            CompositeOn = false;
        }
        
        if (FuelOn)
        {
            if (Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel) > 0)
            {
                float fuel = 5 * delta;
                fuel = Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel) < fuel
                    ? Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel)
                    : fuel;
                fuel = SpaceShip.fuel + fuel > SpaceShip.FUELWIN ? SpaceShip.FUELWIN - SpaceShip.fuel : fuel;
                SpaceShip.AddFuel(fuel);
                Player.inventoryLiquids.Remove(Liquid.Type.Fuel, fuel);
            }
            else
            {
                FuelOn = false;
                BtnFuelT.Text = "OFF";
            }
        }
        
        if (EnergyeOn)
        {
            if (Player.energy > 0)
            {
                float energy = 5 * delta;
                energy = Player.energy < energy
                    ? Player.energy
                    : energy;
                energy = SpaceShip.energy + energy > SpaceShip.ENERGYWIN ? SpaceShip.ENERGYWIN - SpaceShip.energy : energy;
                SpaceShip.AddEnergy(energy);
                Player.RemoveEnergy(energy);
            }
            else
            {
                EnergyeOn = false;
                BtnEnerT.Text = "OFF";
            }
        }
        
        RefreshLaunchBTN();
    }

    public void _on_TimerComposite_timeout()
    {
        if (CompositeOn)
        {
            if (Player.inventoryItems.GetItemCount(Item.Type.Composite) > 0)
            {
                if (SpaceShip.composite < SpaceShip.COMPOSITEWIN)
                {
                    SpaceShip.AddComposite(1);
                    Player.inventoryItems.Remove(Item.Type.Composite, 1);
                }
            }
            else
            {
                CompositeOn = false;
                BtnCompoT.Text = "OFF";
            }
        }
    }

    private static void RefreshLaunchBTN()
    {
        if (SpaceShip.composite >= SpaceShip.COMPOSITEWIN && SpaceShip.fuel >= SpaceShip.FUELWIN &&
            SpaceShip.energy >= SpaceShip.ENERGYWIN)
            Launch.Disabled = false;
        else
            Launch.Disabled = true;
    }

    public void _on_BtnTranEner_button_down()
    {
        if (EnergyeOn)
        {
            EnergyeOn = false;
            BtnEnerT.Text = "OFF";
        }
        else
        {
            EnergyeOn = true;
            BtnEnerT.Text = "ON";
        }
    }

    public void _on_BtnTranFuel_button_down()
    {
        if (FuelOn)
        {
            FuelOn = false;
            BtnFuelT.Text = "OFF";
        }
        else
        {
            FuelOn = true;
            BtnFuelT.Text = "ON";
        }
    }

    public void _on_BtnTranCompo_button_down()
    {
        if (CompositeOn)
        {
            CompositeOn = false;
            BtnCompoT.Text = "OFF";
        }
        else
        {
            CompositeOn = true;
            BtnCompoT.Text = "ON";
        }
    }
}
