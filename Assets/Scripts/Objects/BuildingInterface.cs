using Godot;
using System;
using System.Collections.Generic;

public class BuildingInterface : Control
{
    private static bool isInit = false;
    public static bool IsInit => isInit;
    public static void IsInitBuildingInterfaceTest(string funcName)
    {
        if (!isInit)
            throw new UninitializedException(funcName, "BuildingInterface");
    } 
    
    private static Node parent;
    public static void Init(Node parent)
    {
        BuildingInterface.parent = parent;
        isInit = true;
    }
    
    private static Dictionary<Building.Type, PackedScene> prefads = new Dictionary<Building.Type, PackedScene>
    {
        {Building.Type.SolarPanel, GD.Load<PackedScene>("res://Assets/Objects/UI/Building/SolarPanelInterface.tscn")},
        {Building.Type.Storage, GD.Load<PackedScene>("res://Assets/Objects/UI/Building/StorageInterface.tscn")},
        {Building.Type.Printer3D, GD.Load<PackedScene>("res://Assets/Objects/UI/Building/Printer3DInterface.tscn")}
    };

    public static bool interfaceOpen = false;
    public static BuildingInterface buildingInterface = null;

    public static void OpenInterface(Building b)
    {
        IsInitBuildingInterfaceTest("OpenInterface");
        if (interfaceOpen)
        {
            CloseInterface();
        }
        BuildingInterface bi = (BuildingInterface) prefads[b.type].Instance();
        bi.building = b;
        bi.Open();
        interfaceOpen = true;
        buildingInterface = bi;
        PlayerState.SetState(PlayerState.State.BuildingInterface);
    }
    public static void CloseInterface()
    {
        IsInitBuildingInterfaceTest("CloseInterface");
        if (interfaceOpen)
        {
            buildingInterface.Close();
            buildingInterface = null;
            interfaceOpen = false;
            PlayerState.SetState(PlayerState.prec_state);
        }
    }

    /*-----*/
    
    
    [Signal]
    delegate void ChangeEnergyBar(float energy, float energyMax);
    [Signal]
    delegate void ChangePowerInBar(float power, float powerMax);
    [Signal]
    delegate void ChangePowerOutBar(float power, float powerMax);
    
    
    public Building building;

    private Label idLabel;
    public void Open()
    {
        parent.AddChild(this);
        GetNode<Graph>("back2/Graph").SetValue(building.energyhistory, building.energyMax);
        GetNode<Graph>("back2/Graph").SetParams(0.8f, Colors.Yellow, "e","t");
        GetNode<Graph>("back2/Graph2").SetValue(building.powerInhistory, 1.2f);
        GetNode<Graph>("back2/Graph2").SetParams(0.8f, Colors.Green, "e/s", "t");
        GetNode<Graph>("back2/Graph3").SetValue(building.powerOuthistory, 1.2f);
        GetNode<Graph>("back2/Graph3").SetParams(0.8f, Colors.Red, "e/s", "t");
        idLabel = GetNode<Label>("back/id");
        Refresh();
    }
    public void Close()
    {
        QueueFree();
    }

    public void _on_TimerEnergy_timeout()
    {
        Refresh();
    }
    
    private void Refresh()
    {
        idLabel.Text = "ID : " + building.id;
        EmitSignal("ChangeEnergyBar", building.energy, building.energyMax);
        EmitSignal("ChangePowerInBar", building.powerIn, 1.5f);
        EmitSignal("ChangePowerOutBar", building.powerOut, 1.5f);
    }
}
