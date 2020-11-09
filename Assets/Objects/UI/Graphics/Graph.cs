using Godot;
using System;
using System.Collections.Generic;

public class Graph : Panel
{
    private Vector2 size;
    private int yZero;
    private int xBegin;
    private int xDataBegin;
    private int yDataMax;
    private int barLength;
    private int nbDataPossible;
    
    private float coefficientDirecteur;
    private float ordonneeALOrigine;

    private DynamicFont font;
    private Color color = Colors.Red;

    private float dataMax = 200.0f;
    private List<float> datas = new List<float>();

    private float xZoom = 0.5f;
    private float yZoom = 1.0f;

    private string yUnit = "";
    private string xUnit = "";

    public void SetValue(List<float> datas, float dataMax)
    {
        this.datas = datas;
        this.dataMax = dataMax;
        coefficientDirecteur = (yDataMax - yZero) / dataMax;
        ordonneeALOrigine = yZero;
    }
    public void SetParams(float zero, Color color, string yUnit, string xUnit)
    {
        yZero = Mathf.FloorToInt(zero * size.y);
        this.color = color;
        coefficientDirecteur = (yDataMax - yZero) / dataMax;
        ordonneeALOrigine = yZero;
        this.yUnit = yUnit;
        this.xUnit = xUnit;
    }
    
    public override void _Ready()
    {
        size = RectSize;
        yZero = Mathf.FloorToInt(0.8f * size.y);
        xBegin = Mathf.FloorToInt(0.25f * size.x);
        xDataBegin = Mathf.FloorToInt(0.95f * size.x);
        yDataMax = Mathf.FloorToInt(0.2f * size.y);
        barLength = Mathf.FloorToInt(0.025f * size.x);

        nbDataPossible = xDataBegin - xBegin + 1;
        
        
        font = new DynamicFont();
        font.FontData = GD.Load<DynamicFontData>("res://Assets/Ressources/Fonts/F25_Bank_Printer.otf");
        font.Size = 12;
    }

    public void _on_Timer_timeout()
    {
        Update();
    }

    public override void _Draw()
    {
        
        
        DrawLine(new Vector2(xBegin-1, 0), new Vector2(xBegin-1, size.y-1), Colors.White);
        DrawLine(new Vector2(xBegin, yZero), new Vector2(size.x-1, yZero),  Colors.White);

        DrawLine(new Vector2(xBegin-barLength/2, yDataMax), new Vector2(xBegin+barLength/2, yDataMax), Colors.White);
        string number = Math.Round(dataMax*yZoom, 2).ToString() + yUnit;
        Vector2 stringSize = font.GetStringSize(number);
        DrawString(font, new Vector2(xBegin-barLength/2-stringSize.x-(0.01f*size.x),yDataMax+(stringSize.y/2)), number);
        
        DrawLine(new Vector2(xBegin-barLength/2, yZero), new Vector2(xBegin+barLength/2, yZero), Colors.White);
        number = "0" + yUnit;
        stringSize = font.GetStringSize(number);
        DrawString(font, new Vector2(xBegin-barLength/2-stringSize.x-(0.01f*size.x),yZero+(stringSize.y/2)), number);
        
        
        int x = xDataBegin;
        float step = History<float>.valuesCount * xZoom / nbDataPossible;
        float a = datas.Count - 1;
        for (float i = History<float>.valuesCount-1; i >= History<float>.valuesCount * (1-xZoom); i-=step)
        {
            if (a >= 0)
            {
                float y = datas[Mathf.FloorToInt(a)] * coefficientDirecteur + ordonneeALOrigine;
                if (y >= 0 && y < size.y)
                    DrawCircle(new Vector2(x, y), 1.5f, color);
            }
            x--;
            a -= step;
        }
        
    }

    public void _on_HSlider_value_changed(float value)
    {
        xZoom = value;
    }

    public void _on_yZOOM_value_changed(float value)
    {
        yZoom = value;
        coefficientDirecteur = (yDataMax - yZero) / (dataMax*value);
        ordonneeALOrigine = yZero;
    }
}
