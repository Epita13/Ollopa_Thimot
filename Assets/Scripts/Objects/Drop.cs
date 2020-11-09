using Godot;
using System;
using System.Collections.Generic;

public class Drop
{
    
    public struct Loot
    {
        public Loot(Item.Type type, int amount)
        {
            this.amount = amount;
            this.type = type;
        }
        public Item.Type type { get; }
        public int amount { get; }
    }
    
    public List<Loot> loots = new List<Loot>();
    
    public Drop(params Loot[] loots)
    {
        foreach (var l in loots)
        {
            this.loots.Add(l);
        }
    }



    public static bool PlayerCanCraft(Drop d)
    {
        bool can = true;
        foreach (var loot in d.loots)
        {
            if (Player.inventoryItems.GetItemCount(loot.type) < loot.amount)
            {
                can = false;
            }
        }

        return can;
    }
}
