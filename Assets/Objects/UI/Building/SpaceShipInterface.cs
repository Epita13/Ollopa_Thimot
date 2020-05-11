using Godot;
using System;

public class SpaceShipInterface : Control
{
    private static Control inventory;
    private static Label ECurrent;
    private static Label EBetween;
    private static Label EGoal;
    private static Label EValue;
    private static Label FCurrent;
    private static Label FBetween;
    private static Label FGoal;
    private static Label FValue;
    private static Label CCurrent;
    private static Label CBetween;
    private static Label CGoal;
    private static Label CValue;
    private static Button Launch;
    
    
    public static void open_interface()
    {
        inventory = (Control) GD.Load<PackedScene>("res://Assets/Objects/UI/Building/SpaceShipInterface.tscn").Instance(); 
        SpaceShip.canvas.AddChild(inventory);
        SpaceShip.inventoryOpen = true;
        PlayerState.SetState(PlayerState.State.BuildingInterface);
        ECurrent = inventory.GetNode("NinePatchRect").GetNode("Energie").GetNode<Label>("EC");
        EBetween = inventory.GetNode("NinePatchRect").GetNode("Energie").GetNode<Label>("EB");
        EGoal = inventory.GetNode("NinePatchRect").GetNode("Energie").GetNode<Label>("EG");
        EValue = inventory.GetNode("NinePatchRect").GetNode("Energie").GetNode<Label>("EV");
        FCurrent = inventory.GetNode("NinePatchRect").GetNode("Fuel").GetNode<Label>("FC");
        FBetween = inventory.GetNode("NinePatchRect").GetNode("Fuel").GetNode<Label>("FB");
        FGoal = inventory.GetNode("NinePatchRect").GetNode("Fuel").GetNode<Label>("FG");
        FValue = inventory.GetNode("NinePatchRect").GetNode("Fuel").GetNode<Label>("FV");
        CCurrent = inventory.GetNode("NinePatchRect").GetNode("Composite").GetNode<Label>("CC");
        CBetween = inventory.GetNode("NinePatchRect").GetNode("Composite").GetNode<Label>("CB");
        CGoal = inventory.GetNode("NinePatchRect").GetNode("Composite").GetNode<Label>("CG");
        CValue = inventory.GetNode("NinePatchRect").GetNode("Composite").GetNode<Label>("CV");
        Launch = inventory.GetNode<Button>("Launch");

        ECurrent.Text = SpaceShip.energy.ToString();
        EBetween.Text = " / ";
        EGoal.Text = SpaceShip.ENERGYWIN.ToString();
        EValue.Text = " Energy";
        FCurrent.Text = SpaceShip.fuel.ToString();
        FBetween.Text = " / ";
        FGoal.Text = SpaceShip.FUELWIN.ToString();
        FValue.Text = " Fuel";
        CCurrent.Text = SpaceShip.composite.ToString();
        CBetween.Text = " / ";
        CGoal.Text = SpaceShip.COMPOSITEWIN.ToString();
        CValue.Text = " Composite";

        if (SpaceShip.composite >= SpaceShip.COMPOSITEWIN && SpaceShip.fuel >= SpaceShip.FUELWIN &&
            SpaceShip.energy >= SpaceShip.ENERGYWIN)
            Launch.Disabled = false;
        else
        {
            Launch.Disabled = true;
        }
    }
    
    public static void close_interface()
    {
        inventory.QueueFree();
        SpaceShip.inventoryOpen = false;
        PlayerState.SetState(PlayerState.State.Normal);
    }


    public void _on_Launch_button_down()
    {
        SpaceShip.close_interface();
        Control End = (Control)GD.Load<PackedScene>("res://Assets/Objects/UI/Menus/End.tscn").Instance();
        SpaceShip.canvas.AddChild(End);
        PlayerState.state = PlayerState.State.Finish;
    }
}
