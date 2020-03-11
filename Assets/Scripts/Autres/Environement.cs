using Godot;
using System;

public class Environement : Node2D
{
    private static float length_day = 60 * 15;  // seconde
    private float time = length_day / 2 + 25; // seconde
    private int nb_day = 0;

    private static int hourNight = 19;
    private static int minNight = 30;
    
    private static int hourDay = 7;
    private static int minDay = 0;

    private static float transition = 4.5f; // %
    
    public enum TimeState
    {
        DAY,
        NIGHT
    }
    public TimeState cycle = TimeState.DAY;

    private int GetHour(float seconds) => Mathf.FloorToInt(seconds * 24 / length_day);
    private int GetMin(float seconds) => Mathf.FloorToInt(((seconds%(length_day/24))*60)/(length_day/24));

    public override void _Process(float delta)
    {
        time += delta;
        int hour = GetHour(time);
        int minute = GetMin(time);
        if (hour >= 24)
            time = time % length_day;
        GD.Print(hour + ":" + minute);
        
        if (hour>hourNight || hour==hourNight && minute>=minNight || hour<hourDay || hour==hourDay && minute<minDay)
        {
            if (cycle == TimeState.DAY)
            {
                cycle = TimeState.NIGHT;
                // passe a la nuit
                Night();
            }
        }
        else
        {
            if (cycle == TimeState.NIGHT)
            {
                cycle = TimeState.DAY;
                nb_day++;
                // passe au jour
                Day();
            }
        }
    }


    private void Night()
    {
        Tween twe = new Tween();
        AddChild(twe);
        if (HasNode("Canvas_DayNight"))
        {
            CanvasModulate CM = GetNode<CanvasModulate>("Canvas_DayNight");
            twe.InterpolateProperty(CM,"color",
                Color.Color8(255,255,255),Color.Color8(50,50,50), transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
            twe.Start();
        }
    }
    
    private void Day()
    {
        Tween twe = new Tween();
        AddChild(twe);
        if (HasNode("Canvas_DayNight"))
        {
            CanvasModulate CM = GetNode<CanvasModulate>("Canvas_DayNight");
            twe.InterpolateProperty(CM,"color", 
                Color.Color8(50,50,50),Color.Color8(255,255,255), transition*length_day/100,Tween.TransitionType.Sine,Tween.EaseType.In);
            twe.Start();
        }
    }
}
