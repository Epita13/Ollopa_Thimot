using Godot;
using System;

public class SlideBar : Sprite
{
    [Export] public float max;

    public void Change(float value, float valueMax)
    {
        float x = (value * max) / valueMax;
        Scale = new Vector2(x, Transform.Scale.y);
    }

}
