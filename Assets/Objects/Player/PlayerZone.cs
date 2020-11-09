using Godot;
using System;

public class PlayerZone : Area2D
{

    public override void _Ready()
    {
        
    }
    
    public void _on_Area2D_area_shape_entered(int id, Area2D area, int areaShape, int selfShape)
    {
        /* REcuperation des loots*/
        if (area.GetGroups().Contains("loot"))
        {
            /*On a un loot*/
            Loot loot = area.GetNode<Loot>("..");
            bool canAdd = Player.inventoryItems.CanAdd(loot.GetLootType(), loot.GetLootAmount());
            Player.inventoryItems.Add(loot.GetLootType(),
                Player.inventoryItems.GetAmountCanAdd(loot.GetLootType(), loot.GetLootAmount()));
            area.RemoveFromGroup("loot");
            if (canAdd)
            {
            	PlayerMouvements.PlaySound(Sounds.Type.PlayerGetloot);
                loot.QueueFree();
            }
            else
            {
                loot.Explosion();
                loot.dead = true;
                loot.GetNode<Sprite>("img").Visible = false;
                Delay.StartDelay(loot, 0.3f, () => loot.QueueFree());
            }
        }
    }
}
