using Godot;
using System;

public class EnergyBar : Control
{

    private Control bar;

    private Label energyMaxLabel;
    private Label energyLabel;
    
    private float yMax;
    
    public override void _Ready()
    {
        bar = GetNode<Control>("slide");

        energyLabel = GetNode<Label>("labelCUR");
        energyMaxLabel = GetNode<Label>("labelMAX");
        
        yMax = bar.RectSize.y;
        bar.RectSize = new Vector2(bar.RectSize.x, 0);
    }
    

    public void Change(float energy, float energyMax)
    {
        float y = energy * yMax / energyMax;
        bar.RectSize = new Vector2(bar.RectSize.x, y);
        energyMaxLabel.Text = energyMax.ToString() + "e";
        energyLabel.Text = energy.ToString(GetFormat(energy)) + "e";

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
