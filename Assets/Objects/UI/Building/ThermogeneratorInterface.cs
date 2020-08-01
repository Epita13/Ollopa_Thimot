using Godot;
using System;
using System.Collections.Generic;

public class ThermogeneratorInterface : BuildingInterface
{
	private Thermogenerator tg;
	private bool woodon = false;
	private bool oilon = false;
	private Button buttonWood;
	private Button buttonOil;
	
	public override void _EnterTree()
	{
		tg = (Thermogenerator)building;
		buttonWood = GetNode<Button>("back/BtnWood");
		buttonOil = GetNode<Button>("back/BtnOil");
		GetNode<TextureRect>("back/ItemBox/img").Texture = Item.textures[(int)Item.Type.Wood];
	}

	public override void _Process(float delta)
	{
		GetNode<PetrolBar>("back/PetrolBar").Change(tg.oil,Thermogenerator.oilMax);
		GetNode<Label>("back/ItemBox/texte").Text = tg.wood + "/" + Thermogenerator.woodMax;
		
		
		if (tg.wood >= Thermogenerator.woodMax || Player.inventoryItems.GetItemCount(Item.Type.Wood) == 0)
		{
			buttonWood.Disabled = true;
			buttonWood.Text = "OFF";
			woodon = false;
		}
		else
		{
			buttonWood.Disabled = false;
		}
		if (tg.oil >= Thermogenerator.oilMax || Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil) == 0)
		{
			buttonOil.Disabled = true;
			buttonOil.Text = "OFF";
			oilon = false;
		}
		else
		{
			buttonOil.Disabled = false;
		}
		
		
		if (oilon)
		{
			if (Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil) > 0)
			{
				float oil = 5F * delta;
				oil = Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil) < oil
					? Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil)
					: oil;
				oil = tg.oil + oil > Thermogenerator.oilMax ? Thermogenerator.oilMax - tg.oil : oil;
				tg.AddOil(oil);
				Player.inventoryLiquids.Remove(Liquid.Type.Oil, oil);
			}
			else
			{
				oilon = false;
			}
		}
	}



	public void _on_LinkBtn_button_down()
	{
		PlayerState.SetState(PlayerState.State.Link);
		Link.Init(tg);
		BuildingInterface.CloseInterface();
	}
	
	private void _on_BtnOil_button_down()
	{
		if (oilon)
		{
			oilon = false;
			buttonOil.Text = "OFF";
		}
		else
		{
			oilon = true;
			buttonOil.Text = "ON";
		}
	}
	
	
	private void _on_BtnWood_button_down()
	{
		if (woodon)
		{
			woodon = false;
			buttonWood.Text = "OFF";
		}
		else
		{
			woodon = true;
			buttonWood.Text = "ON";
		}
	}

	private void _on_TimerWood_timeout()
	{
		if (woodon)
		{
			if (Player.inventoryItems.GetItemCount(Item.Type.Wood) >= 1)
			{
				Player.inventoryItems.Remove(Item.Type.Wood, 1);
				tg.AddWood(1);
			}
			else
			{
				woodon = false;
			}
		}
	}												 
 
 
 
 

}







											
