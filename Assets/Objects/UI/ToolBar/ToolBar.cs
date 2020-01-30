using Godot;
using System;

public class ToolBar : Node
{
    private int index = 0;
    public int[] displayTools = new int[5];


    [Signal] public delegate void Refresh(int[] values);

    public override void _Ready()
    {
        SendRefresh();
    }

    public override void _Process(float delta)
    {
            if (Input.IsActionJustPressed("ui_toolbar_right"))
            {
                index = (index+1)%Usable.nbUsable;
                SendRefresh();
            }else if (Input.IsActionJustPressed("ui_toolbar_left"))
            {
                index = (Usable.nbUsable + (index-1))%Usable.nbUsable;
                SendRefresh();
            }
    }

    public void SendRefresh()
    {
        displayTools[2] = index;
        for (int i = 1; i < 3; i++)
        {
            displayTools[2+i] = (index+i)%Usable.nbUsable;
            displayTools[2-i] = (Usable.nbUsable+(index-i))%Usable.nbUsable;
        }
        EmitSignal("Refresh", displayTools);
    }

}
