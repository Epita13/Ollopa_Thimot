using Godot;
using System;
using System.Collections.Generic;

public class UI_PlayerInventory : Control
{
    /*Le script n'a besoin de rien pour exister mais il le script n'appelle jamais de lui-même Open()*/
    /*Le script change lui-même l'état du joueur en inventaire ou en normal à l'appel de Open et Close*/
    
    public static UI_PlayerInventory instance;
    public static UI_PlayerInventory GetInstance() => instance;

    private ItemList itemList;
    private HBoxContainer inventories;
    private Button btItems;
    private Button btBuildings;
    private Button btUsable;
    private Button btClose;
    private Vector2 size = new Vector2(300, 10);                //Taille des la barre des boutons, l'itemListe est * fois plus haut que les boutons
    private Vector2 pos = new Vector2(0,0);                    // Position coin supérieur gauche par rapport au noeud parent, par défaut à (0,0)


    public override void _Ready()
    {
        instance = this;
        itemList = GetNode<ItemList>("List");
        inventories = GetNode<HBoxContainer>("Inventories");
        btItems = GetNode("Inventories").GetNode<Button>("Items");
        btBuildings = GetNode("Inventories").GetNode<Button>("Buildings");
        btUsable = GetNode("Inventories").GetNode<Button>("Usable");
        btClose = GetNode("Inventories").GetNode<Button>("Close");

        inventories.RectSize = size;                                                                          //Définit la taille de la barre des boutons et de l'itemListe
        itemList.RectSize = new Vector2(size.x, size.y * 9);
        inventories.RectPosition = pos;
        itemList.RectPosition = new Vector2(pos.x, pos.y + inventories.RectSize.y);
        SizeTextButtons();                                                                                    //Définit le texte des boutons mais la taille ne fonctionne pas (détail dans la fonction)
    }

    public override void _Process(float delta)
    {
        if (inventories.Visible)                                    //Quand l'inventaire est affiché, vérifie si un des bouton est pressé
        {
            if(btItems.Pressed)
                Open("item");
            else if (btBuildings.Pressed)
                Open("building");
            else if (btUsable.Pressed)
                Open("usable");
            else if (btClose.Pressed)
                Close();
        }
    }

    public static void Open(string str)                           //Affiche l'inventaire correspondant a str, lance une exception si pas de correspondance
    {
        GetInstance().itemList.Clear();

        if (string.Equals(str, "item"))
        {
           for (int i = 0; i < Item.nbItems; i++)
               GetInstance().itemList.AddItem(Convert.ToString(Player.inventoryItems.GetItemCount((Item.Type)i)), Item.textures[i] , true); 
        }
        else if (string.Equals(str, "usable"))
        {
            for (int i = 0; i < Usable.nbUsables; i++)
            {
                if ((Usable.Type)i == Usable.Type.Laser)
                    GetInstance().itemList.AddItem("/", Usable.textures[i] , true);
                else
                    GetInstance().itemList.AddItem(Convert.ToString(Player.inventoryUsables.GetItemCount((Usable.Type)i)), Usable.textures[i] , true);
            }
        }
        else if (string.Equals(str, "building"))
        {
            foreach (Building.Type type in Enum.GetValues(typeof(Building.Type)))
            {
                GetInstance().itemList.AddItem(Player.inventoryBuildings.GetItemCount(type).ToString(), Building.textures[type]);
            }
        }
        else
            throw new Exception("Invalid argument in Open() from UI_PlayerInvventory.cs");

        GetInstance().Visible = true;
        
        
        PlayerState.SetState(PlayerState.State.Inventory);
    }
    
    public static void Close()                                        //Ferme l'inventaire
    {
        GetInstance().Visible = false;
        PlayerState.SetState(PlayerState.State.Normal);
    }

    private void SizeTextButtons()                             //Doit adapter la taille mais ne fonctionne pas parce que les boutons sont dans un hbox container
    {
        btItems.Text = "Items";
        btBuildings.Text = "Buildings";
        btUsable.Text = "Usable";
        btClose.Text = "X";
        
       /* btClose.RectSize = new Vector2(12 ,btClose.RectSize.y);
        btItems.RectSize = new Vector2((size.x - btClose.RectSize.x) / 3 , btItems.RectSize.y);
        btBuildings.RectSize = new Vector2((size.x - btClose.RectSize.x) / 3 , btBuildings.RectSize.y);
        btUsable.RectSize = new Vector2((size.x - btClose.RectSize.x) / 3 , btUsable.RectSize.y);*/
    }
    
    
    // On double click 
    public void _on_List_item_activated(int index)
    {
        
    }
    
    
}
