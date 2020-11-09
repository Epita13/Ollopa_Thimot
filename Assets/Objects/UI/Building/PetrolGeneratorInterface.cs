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
        float nb = Math.Min(petrolGenerator.oil, Player.inventoryLiquids.max - Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil));
        if (petrolGenerator.oil > 0 && Player.inventoryLiquids.CanAdd(Liquid.Type.Oil, nb))
        {
            petrolGenerator.togive += nb;
        }
    }

    public void _on_Timer_timeout()
    {
        EmitSignal("ChangePetrolBar", petrolGenerator.oil, petrolGenerator.oilMAX);
    }
}
