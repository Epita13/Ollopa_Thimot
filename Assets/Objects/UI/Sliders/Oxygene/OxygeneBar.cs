using Godot;
using System;

public class OxygeneBar : Control
{

    private Control bar;

    private Label oxygeneMaxLabel;
    private Label oxygeneLabel;
    
    private float yMax;
    
    public override void _Ready()
    {
        bar = GetNode<Control>("slide");

        oxygeneLabel = GetNode<Label>("labelCUR");
        oxygeneMaxLabel = GetNode<Label>("labelMAX");
        
        yMax = bar.RectSize.y;
        bar.RectSize = new Vector2(bar.RectSize.x, 0);
    }
    

    public void Change(float oxygene, float oxygeneMax)
    {
        float y = oxygene * yMax / oxygeneMax;
        bar.RectSize = new Vector2(bar.RectSize.x, y);
        oxygeneMaxLabel.Text = oxygeneMax + " o2";
        oxygeneLabel.Text = oxygene.ToString(GetFormat(oxygene)) + " o2";

    }

    private string GetFormat(float energy)
    {
        string d = ((int)energy).ToString();
        string format = "";
        for (int i = 0; i < d.Length(); i++)
        {
            format += "0";
        }
        format += ".0";
        return format;
    }
}
