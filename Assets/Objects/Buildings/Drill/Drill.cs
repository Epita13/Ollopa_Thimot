using Godot;
using System;
using System.Collections.Generic;

public class Drill : Building
{
    public static int nbDrill;
    private static float power = 3f;
    private static readonly float Drillproduc = 2f;
    public int stockMAX = 500;
    public  List<Item.Type> togive = new List<Item.Type>();
    public Dictionary<Item.Type, int> stock = new Dictionary<Item.Type, int>();
    public bool on = false;
    private static float power2wake = 2 * power;
    private static float giveSpeed = 2.5f;
    private float timeBetweenProduct = 0;
    private Item.Type[] proba = new Item.Type[100];
    public bool deploy = false;

    
    
    /*Structure de sauvegarde*/
    public struct SaveStruct
    {
        public Building.SaveStruct buildingSave;
        public List<Item.Type> togive;
        public Dictionary<Item.Type, int> stock;
        public bool on;
        public bool deploy;
    }

    public SaveStruct GetSaveStruct()
    {
        SaveStruct s = new SaveStruct();
        s.buildingSave = GetBuildingSaveStruct();
        s.togive = togive;
        s.stock = stock;
        s.@on = @on;
        s.deploy = deploy;
        return s;
    }
    /*************************/

    public Drill() : base (150, 100.0f)
    {
    }
    
    
    public override void _EnterTree()
    {
        this.id = nbDrill;
        nbDrill++;
        int i = 0;
        
        foreach (Item.Type item in Item.drillable)
        {
            stock.Add(item, 0);
        }
        
        GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("TOON");

        for (i = i; i < 30; i++)
        {
            proba[i] = Item.Type.Dirt;
        }
        for (i = i; i < 80; i++)
        {
            proba[i] = Item.Type.Stone;
        }
        for (i = i; i < 95; i++)
        {
            proba[i] = Item.Type.Sonar;
        }
        for (i = i; i < 100; i++)
        {
            proba[i] = Item.Type.Ospirit;
        }
    }
    
    public void _on_Timer_timeout()
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        
        if (togive.Count > 0)
        {
            foreach (Item.Type item in togive)
            {
                Player.inventoryItems.Add(item, stock[item]);
                stock[item] = 0;
            }
            togive.Clear();
        }
        
        if (on)
        {
            if (!deploy)
            {
                GetNode<AnimationPlayer>("AnimationPlayer").Play("TOON");
                deploy = true;
            }
            if(!GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
                GetNode<AnimationPlayer>("AnimationPlayer").Play("ON");
            
            RemoveEnergy(power * timer.WaitTime);
            timeBetweenProduct += Drillproduc * timer.WaitTime;
            while (timeBetweenProduct >= Drillproduc)
            {
                DrillItem();
                timeBetweenProduct -= Drillproduc;
            }
        }
        else if (deploy)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("TOON");
            deploy = false;
        }
        
        
        on = (energy >= power2wake && !on || energy > 0 && on) &&  Count() < stockMAX;
    }

    private void DrillItem()
    {
        Random i = new Random();
        stock[proba[i.Next(99)]]++;
    }

    private int Count()
    {
        int res = 0;
        foreach (var item in stock)
        {
            res += item.Value;
        }

        return res;
    }
}
