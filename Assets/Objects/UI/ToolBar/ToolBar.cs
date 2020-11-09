using Godot;
using System;

public class ToolBar : Control
{
    public int[] displayTools = new int[5];


    [Signal] public delegate void Refresh(int[] values);

    private static ToolBar instance;
    public static ToolBar GetInstance() => instance;
    
    public override void _Ready()
    {
        instance = this;
        SendRefresh();
    }

    public override void _Process(float delta)
    {
        if (PlayerState.GetState() != PlayerState.State.Normal)
        {
            Visible = false;
        }
        else
        {
            Visible = true;
            if (Input.IsActionJustPressed("ui_toolbar_right"))
            {
                Player.UsableSelected = (Usable.Type) Modulo((int)Player.UsableSelected + 1, Usable.nbUsables);
            }
            else if (Input.IsActionJustPressed("ui_toolbar_left"))
            {
                Player.UsableSelected = (Usable.Type) Modulo((int)Player.UsableSelected - 1, Usable.nbUsables);
            }
            SendRefresh();
        }
    }

    private int Modulo(int value, int mod)
    {
        if (value < 0)
        {
            return mod + (value % mod);
        }
        else
        {
            return value % mod;
        }
    }

    public static void SendRefresh()
    {
        int index = (int)Player.UsableSelected;
        if (GetInstance()==null)
            return;
        GetInstance().displayTools[2] = index;
        for (int i = 1; i < 3; i++)
        {
            GetInstance().displayTools[2+i] = (index+i)%Usable.nbUsables;
            GetInstance().displayTools[2-i] = (Usable.nbUsables+(index-i))%Usable.nbUsables;
        }
        GetInstance().EmitSignal("Refresh", GetInstance().displayTools);
        Player.UsableSelected = (Usable.Type)index;
    }
    
    public override void _UnhandledInput(InputEvent @event){
        if (@event is InputEventMouseButton){
            InputEventMouseButton emb = (InputEventMouseButton)@event;
            if (emb.IsPressed()){
                if (emb.ButtonIndex == (int)ButtonList.WheelUp){
                    Player.UsableSelected = (Usable.Type) Modulo((int)Player.UsableSelected - 1, Usable.nbUsables);
                }
                if (emb.ButtonIndex == (int)ButtonList.WheelDown){
                    Player.UsableSelected = (Usable.Type) Modulo((int)Player.UsableSelected + 1, Usable.nbUsables);
                }
            }
        }
    }

}
