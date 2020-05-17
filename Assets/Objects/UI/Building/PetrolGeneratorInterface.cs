using Godot;
using System;
using System.Reflection;

public class PetrolGeneratorInterface : BuildingInterface
{
    private PetrolGenerator petrolGenerator;

    /*Description*/
    private Button btnGivePetrol;
    
    [Signal]
    delegate void ChangePetrolBar(float oil, float oilMax);
    
    public override void _EnterTree()
    {
        petrolGenerator = (PetrolGenerator)building;
        btnGivePetrol = GetNode<Button>("back/BtnPetrol");
        
        RefreshBtnCompact();
    }
    
    
    
    public override void _Process(float delta)
    {
        RefreshBtnCompact();
    }

    private void RefreshBtnCompact()
    {
        if (petrolGenerator.oil > 0)
        {
            btnGivePetrol.Disabled = false;
        }
        else
        {
            btnGivePetrol.Disabled = true;
        }
    }

    public void _on_BtnO2_button_down()
    {
        if (petrolGenerator.oil > 0)
        {
            petrolGenerator.togive += Math.Min(petrolGenerator.oil, Player.oxygeneMax - Player.oxygene);
        }
    }

    public void _on_Timer_timeout()
    {
        EmitSignal("ChangePetrolBar", petrolGenerator.oil, petrolGenerator.oilMAX);
    }
}
