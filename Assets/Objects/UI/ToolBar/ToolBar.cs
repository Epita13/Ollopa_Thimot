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
                Player.UsableSelected = (Usable.Type)(((int)Player.UsableSelected + 1) % Usable.nbUsables);
            }
            else if (Input.IsActionJustPressed("ui_toolbar_left"))
            {
                Player.UsableSelected = (Usable.Type)(((int)Player.UsableSelected + 1) % Usable.nbUsables);
            }
            SendRefresh();
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

}
