using Godot;
using System;

public class StorageInterface : BuildingInterface
{
    private Storage st;
    
    
    public override void _Ready()
    {
        st = (Storage)building;
    }

    public override void _Process(float delta)
    {
    }
    
    public void _on_LinkBtn_button_down()
    {
        PlayerState.SetState(PlayerState.State.Link);
        Link.Init(st);
        BuildingInterface.CloseInterface();
    }
}
