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
                index = (index+1)%Usable.nbUsables;
                SendRefresh();
            }else if (Input.IsActionJustPressed("ui_toolbar_left"))
            {
                index = (Usable.nbUsables + (index-1))%Usable.nbUsables;
                SendRefresh();
            }
    }

    public void SendRefresh()
    {
        displayTools[2] = index;
        for (int i = 1; i < 3; i++)
        {
            displayTools[2+i] = (index+i)%Usable.nbUsables;
            displayTools[2-i] = (Usable.nbUsables+(index-i))%Usable.nbUsables;
        }
        EmitSignal("Refresh", displayTools);
    }

}
