using Godot;
using System;

public class SpaceShipInterface : Control
{
    private static Control inventory;
    private static Label Energy;
    private static Label Fuel;
    private static Label Composite;
    private static LineEdit EnergyTrans;
    private static LineEdit FuelTrans;
    private static LineEdit CompositeTrans;
    private static Button Launch;
    private static Button Transfer;


    public static void open_interface()
    {
        inventory = (Control) GD.Load<PackedScene>("res://Assets/Objects/UI/Building/SpaceShipInterface.tscn").Instance(); 
        SpaceShip.canvas.AddChild(inventory);
        SpaceShip.inventoryOpen = true;
        PlayerState.SetState(PlayerState.State.BuildingInterface);
        Launch = inventory.GetNode("back").GetNode<Button>("Launch");
        Transfer = inventory.GetNode("back").GetNode("Transfer").GetNode<Button>("Transfer");
        Energy = inventory.GetNode("back").GetNode("Ressource").GetNode<Label>("Energy");
        Fuel = inventory.GetNode("back").GetNode("Ressource").GetNode<Label>("Fuel");
        Composite = inventory.GetNode("back").GetNode("Ressource").GetNode<Label>("Composite");
        EnergyTrans = inventory.GetNode("back").GetNode("Transfer").GetNode("EnergyTrans").GetNode<LineEdit>("ETrans");
        FuelTrans = inventory.GetNode("back").GetNode("Transfer").GetNode("FuelTrans").GetNode<LineEdit>("FTrans");
        CompositeTrans = inventory.GetNode("back").GetNode("Transfer").GetNode("CompositeTrans").GetNode<LineEdit>("CTrans");
        
        EnergyTrans.Text = "0";
        FuelTrans.Text = "0";
        CompositeTrans.Text = "0";
        
        Energy.Text = SpaceShip.energy + "  /  " + SpaceShip.ENERGYWIN + "  Energy";
        Fuel.Text = SpaceShip.fuel + "  /  " + SpaceShip.FUELWIN + "  Fuel";
        Composite.Text = SpaceShip.composite + "  /  " + SpaceShip.COMPOSITEWIN + "  Composite";
        
        if (SpaceShip.composite >= SpaceShip.COMPOSITEWIN && SpaceShip.fuel >= SpaceShip.FUELWIN &&
            SpaceShip.energy >= SpaceShip.ENERGYWIN)
            Launch.Disabled = false;
        else
            Launch.Disabled = true;
    }
    
    public static void close_interface()
    {
        inventory.QueueFree();
        SpaceShip.inventoryOpen = false;
        PlayerState.SetState(PlayerState.State.Normal);
    }

    public void _on_Transfer_button_down()
    {
        float e = 0 ;
        float f = 0;
        int c = 0;

        bool corect = float.TryParse(EnergyTrans.Text, out e);
        corect = corect && float.TryParse(FuelTrans.Text, out f);
        corect = corect && int.TryParse(CompositeTrans.Text, out c);
        
        if (corect && c <= Player.inventoryItems.GetItemCount(Item.Type.Composite) /*&& f <= Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel)*/)
        {
            SpaceShip.AddComposite(c);
            SpaceShip.AddEnergy(e);
            SpaceShip.AddFuel(f);
            Player.inventoryItems.Remove(Item.Type.Composite, c);
            //Player.inventoryLiquids.Remove(Liquid.Type.Fuel, f);
                
            Energy.Text = SpaceShip.energy + "  /  " + SpaceShip.ENERGYWIN + "  Energy";
            Fuel.Text = SpaceShip.fuel + "  /  " + SpaceShip.FUELWIN + "  Fuel";
            Composite.Text = SpaceShip.composite + "  /  " + SpaceShip.COMPOSITEWIN + "  Composite";
        
            if (SpaceShip.composite >= SpaceShip.COMPOSITEWIN && SpaceShip.fuel >= SpaceShip.FUELWIN &&
                SpaceShip.energy >= SpaceShip.ENERGYWIN)
                Launch.Disabled = false;
            else
                Launch.Disabled = true;
        }
    }
    
    public void _on_Launch_button_down()
    {
        SpaceShip.close_interface();
        Control endMenu = (Control)GD.Load<PackedScene>("res://Assets/Objects/UI/Menus/EndMenu.tscn").Instance();
        SpaceShip.canvas.AddChild(endMenu);
        PlayerState.state = PlayerState.State.Finish;
        Save.DeleteSave(World.saveName);
    }


    
    public void _on_TimerEnergy_timeout()
    {
        
    }
}
