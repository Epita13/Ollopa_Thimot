using Godot;
using System;
using System.Collections.Generic;

public class SolarPanelInterface : BuildingInterface
{
    private SolarPanel sp;
    

    public override void _Ready()
    {
        sp = (SolarPanel)building;
    }

    public override void _Process(float delta)
    {
    }



    public void _on_LinkBtn_button_down()
    {
        PlayerState.SetState(PlayerState.State.Link);
        Link.Init(sp);
        BuildingInterface.CloseInterface();
    }

  
    

}
