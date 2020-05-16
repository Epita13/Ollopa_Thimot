using Godot;
using System;
using System.Reflection;

public class O2GeneratorInterface : BuildingInterface
{
    private O2Generator o2Generator;

    /*Description*/
    private Button btnGiveO2;
    
    [Signal]
    delegate void ChangeOxygeneBar(float oxygene, float oxygeneMax);
    
    public override void _EnterTree()
    {
        o2Generator = (O2Generator)building;
        btnGiveO2 = GetNode<Button>("back/BtnO2");
        
        RefreshBtnCompact();
    }
    
    
    
    public override void _Process(float delta)
    {
        RefreshBtnCompact();
    }

    private void RefreshBtnCompact()
    {
        if (o2Generator.o2 > 0)
        {
            btnGiveO2.Disabled = false;
        }
        else
        {
            btnGiveO2.Disabled = true;
        }
    }

    public void _on_BtnO2_button_down()
    {
        if (o2Generator.o2 > 0)
        {
            o2Generator.togive += Math.Min(o2Generator.o2, Player.oxygeneMax - Player.oxygene);
        }
    }

    public void _on_TimerOxygene_timeout()
    {
        EmitSignal("ChangeOxygeneBar", o2Generator.o2, o2Generator.o2MAX);
    }
}
