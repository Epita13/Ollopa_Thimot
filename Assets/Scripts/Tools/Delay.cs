using Godot;
using System;

public class Delay : Node
{
    private float time;
    private Action end;
    private Action<float, float> step;

    private bool start = false;
    private float s;


    public static void StartDelay(Node n, float time, Action end, Action<float, float> step = null)
    {
        Delay d = new Delay(time, end, step);
        n.AddChild(d);
        d.Start();
    }
    
    public Delay(float time, Action end, Action<float, float> step = null)
    {
        this.end = end;
        this.step = step;
        this.time = time;
    }

    public void Start()
    {
        start = true;
        s = 0.0f;
    }


    public override void _Process(float delta) 
    {
        if (start)
        {
            if (s >= time)
            {
                end();
                QueueFree();
            }
            else
            {
                if (step != null)
                    step(s, time);
                s += delta;
            }
        }
    }
}
