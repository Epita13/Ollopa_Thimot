using Godot;
using System;

public class DrillInterface : BuildingInterface
{
    private Drill drill;
    private ItemList stock;
    private Button giveSelected;
    private Button giveAll;
    private PackedScene itemBox = GD.Load<PackedScene>("res://Assets/Objects/UI/Building/Other/ItemBox.tscn");

    public override void _EnterTree()
    {
        drill = (Drill) building;
        stock = GetNode<ItemList>("back/stock");
        giveSelected = GetNode<Button>("back/GiveSelected");
        giveAll = GetNode<Button>("back/GiveAll");
        stock.MaxColumns = 1;
        _on_Timer_timeout();
    }

    public void _on_Timer_timeout()
    {
        if (!stock.IsAnythingSelected())
        {
            stock.Clear();
            foreach (var item in drill.stock)
            {
                stock.AddItem(drill.stock[item.Key].ToString(), Item.textures[(int) item.Key], false);
            }
        }
        
        RefreshBtns();
    }

    public void _on_GiveSelected_button_down()
    {
        for (int i = 1; i < stock.Items.Count; i++)
        {
            if (stock.IsSelected(i))
            {
                drill.togive.Add(GetType(stock.Items[i]));
            }
        }
    }

    public void _on_GiveAll_button_down()
    {
        foreach (var item in drill.stock)
        {
            drill.togive.Add(item.Key);
        }
    }

    private void RefreshBtns()
    {
        giveSelected.Disabled = false;
    }

    private Item.Type GetType(object item)
    {
        bool find = false;
        int i = 0;
        Item.Type type = Item.Type.Composite;
        while (!find && i < Item.drillable.Count)
        {
            if (item.Equals(Item.textures[(int)Item.drillable[i]]))
            {
                type = Item.drillable[i];
                find = true;
                i++;
            }
        }

        return type;
    }
}
