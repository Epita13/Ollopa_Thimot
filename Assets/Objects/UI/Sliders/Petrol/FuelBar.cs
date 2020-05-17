using Godot;
using System;

public class FuelBar : Control
{

    private Control bar;

    private Label fuelMaxLabel;
    private Label fuelLabel;
    
    private float yMax;
    
    public override void _Ready()
    {
        bar = GetNode<Control>("slide");

        fuelLabel = GetNode<Label>("labelCUR");
        fuelMaxLabel = GetNode<Label>("labelMAX");
        
        yMax = bar.RectSize.y;
        bar.RectSize = new Vector2(bar.RectSize.x, 0);
    }
    

    public void Change(float fuel, float fuelMax)
    {
        float y = fuel * yMax / fuelMax;
        bar.RectSize = new Vector2(bar.RectSize.x, y);
        fuelMaxLabel.Text = fuelMax + " fuel";
        fuelLabel.Text = fuel.ToString(GetFormat(fuel)) + " fuel";

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
