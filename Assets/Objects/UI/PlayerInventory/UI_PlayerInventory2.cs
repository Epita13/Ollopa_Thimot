using Godot;
using System;

public class UI_PlayerInventory2 : Control
{
    
    
    
    public static UI_PlayerInventory2 instance;
    public static UI_PlayerInventory2 GetInstance() => instance;
    
    private ItemList itemsList;
    private ItemList usablesList;
    private ItemList buildingsList;
    
    private Button btClose;
    
    public override void _Ready()
    {
        instance = this;
        itemsList = GetNode<ItemList>("back/Items");
        usablesList = GetNode<ItemList>("back/Usables");
        buildingsList = GetNode<ItemList>("back/Buildings");
        btClose = GetNode<Button>("back/topbar/Close"); 
        
        GetNode<EnergyBar>("back2/EnergyBar").Change(Player.energy, Player.energyMax);
        GetNode<OxygeneBar>("back2/OxygeneBar").Change(Player.oxygene, Player.oxygeneMax);
        GetNode<HealthBar>("back2/HealthBar").Change(Player.health, Player.healthMax);
        GetNode<FuelBar>("back2/FuelBar").Change(Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel), Player.inventoryLiquidsSize);
        GetNode<PetrolBar>("back2/PetrolBar").Change(Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil), Player.inventoryLiquidsSize);
    }
    
    public override void _Process(float delta)
    {
        if (PlayerState.GetState() == PlayerState.State.Inventory)
        {
            if (btClose.Pressed)
                Close();
            Update();
        }
        GetNode<EnergyBar>("back2/EnergyBar").Change(Player.energy, Player.energyMax);
        GetNode<OxygeneBar>("back2/OxygeneBar").Change(Player.oxygene, Player.oxygeneMax);
        GetNode<HealthBar>("back2/HealthBar").Change(Player.health, Player.healthMax);
        GetNode<FuelBar>("back2/FuelBar").Change(Player.inventoryLiquids.GetItemCount(Liquid.Type.Fuel), Player.inventoryLiquidsSize);
        GetNode<PetrolBar>("back2/PetrolBar").Change(Player.inventoryLiquids.GetItemCount(Liquid.Type.Oil), Player.inventoryLiquidsSize);
    }
    
    
    public static void Open()
    {
        Refresh();
        GetInstance().Visible = true;
        PlayerState.SetState(PlayerState.State.Inventory);
    }
    
    public static void Close(PlayerState.State state = PlayerState.State.Inventory)
    {
        GetInstance().Visible = false;
        if (state == PlayerState.State.Inventory)
        {
            PlayerState.SetState(PlayerState.prec_state);
        }
        else
            PlayerState.SetState(state);
    }

    public static void Update()
    {
        for (int i = 0; i < Item.nbItems; i++)
            GetInstance().itemsList.SetItemText(i, Convert.ToString(Player.inventoryItems.GetItemCount((Item.Type)i)));
        for (int i = 0; i < Usable.nbUsables; i++)
        {
            if (Usable.category[i] == Usable.Category.Block)
                GetInstance().usablesList.SetItemText(i,Convert.ToString(Player.inventoryUsables.GetItemCount((Usable.Type)i)));
        }
        int a = 0;
        foreach (Building.Type type in Enum.GetValues(typeof(Building.Type)))
        {
            GetInstance().buildingsList.SetItemText(a, Player.inventoryBuildings.GetItemCount(type).ToString());
            a++;
        }
    }
    public static void Refresh()
    {
        GetInstance().itemsList.Clear();
        GetInstance().usablesList.Clear();
        GetInstance().buildingsList.Clear();
        for (int i = 0; i < Item.nbItems; i++)
            GetInstance().itemsList.AddItem(Convert.ToString(Player.inventoryItems.GetItemCount((Item.Type)i)), Item.textures[i] , true);
        for (int i = 0; i < Usable.nbUsables; i++)
        {
            if (Usable.category[i] == Usable.Category.Tool)
                GetInstance().usablesList.AddItem("/", Usable.textures[i] , true);
            else
                GetInstance().usablesList.AddItem(Convert.ToString(Player.inventoryUsables.GetItemCount((Usable.Type)i)), Usable.textures[i] , true);
        }
        foreach (Building.Type type in Enum.GetValues(typeof(Building.Type)))
        {
            GetInstance().buildingsList.AddItem(Player.inventoryBuildings.GetItemCount(type).ToString(), Building.textures[type]);
        }
    }


    public void _on_Usables_item_activated(int index)
    {
        Usable.Type type = (Usable.Type) index;
        Player.UsableSelected = type;
        ToolBar.SendRefresh();
        Delay.StartDelay(this,0.1f, () => Close(PlayerState.State.Normal));
    }

    public void _on_Buildings_item_activated(int index)
    {
        Building.Type type = (Building.Type) index;
        if (Player.inventoryBuildings.GetItemCount(type) >= 1)
        {
            Player.BuildingSelected = type;
            Delay.StartDelay(this,0.1f, () => Close(PlayerState.State.Build));
        }
    }
    
}
