using Godot;
using System;

public class HealthBar : Node
{
    private Control bar;

    private Label healthMaxLabel;
    private Label healthLabel;
    
    private float yMax;
    
    public override void _Ready()
    {
        bar = GetNode<Control>("slide");

        healthLabel = GetNode<Label>("labelCUR");
        healthMaxLabel = GetNode<Label>("labelMAX");
        
        yMax = bar.RectSize.y;
        bar.RectSize = new Vector2(bar.RectSize.x, 0);
    }
    

    public void Change(float health, float healthMax)
    {
        float y = health * yMax / healthMax;
        bar.RectSize = new Vector2(bar.RectSize.x, y);
        healthMaxLabel.Text = healthMax + " life";
        healthLabel.Text = health.ToString(GetFormat(health)) + " life";

    }

    private string GetFormat(float health)
    {
        string d = ((int)health).ToString();
        string format = "";
        for (int i = 0; i < d.Length(); i++)
        {
            format += "0";
        }
        format += ".0";
        return format;
    }
}
