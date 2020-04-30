using Godot;
using System;
using System.Collections.Generic;

public class Printer3D : Building
{
    /* Signal pour les voyants */
    
    public static float power = 0.7f; // e/s
    //ID de chaque batiment
    public static int nbPrinter3D = 0;

    private Sprite bar;
    private Sprite printImage;
    private AnimationPlayer barAnimationP;

    /*Printing*/
    public bool isPrinting = false;
    public bool isInPause = false;
    public bool isFinish = false;
    public float printingLevel = 0.0f;
    public Type printingType;
    
    
    public Printer3D() : base (150, 100.0f)
    {
    }
    
    
    public override void _EnterTree()
    {
        this.id = nbPrinter3D;
        nbPrinter3D += 1;

        bar = GetNode<Sprite>("Image/bar");
        printImage = GetNode<Sprite>("Image/printImage");
        barAnimationP = GetNode<AnimationPlayer>("Image/bar/AnimationPlayer");
        barAnimationP.CurrentAnimation = "ENDPRINT";
        
        SetBar(0.0f);
        SetPrintImage(0);
    }
    
    public void _on_Timer_timeout()
    {
        if (isPrinting)
        {
            if (!isFinish && !isInPause && power * timer.WaitTime > energy)
            {
                PauseOrResume();
            }else if (!isFinish && !isInPause)
            {
                printingLevel += timer.WaitTime;
                RemoveEnergy(power * timer.WaitTime);
                SetBar(printingLevel / times2Create[printingType]);
                SetPrintImage(printingLevel / times2Create[printingType]);
                if (printingLevel >= times2Create[printingType])
                {
                    Finish();
                }
            }
        }
        
    }

    private void SetBar(float p)
    {
        float a = ((1-p) * 25.5f) - 22.0f;
        bar.Position = new Vector2(bar.Position.x, a);
    }
    private void SetPrintImage(float p)
    {
        float a = 1 - p;
        printImage.Material.Set("shader_param/y", a);
    }

    public void Print(Type type)
    {
        if (!isPrinting)
        {
            printingType = type;
            printingLevel = 0.0f;
            isPrinting = true;
            isFinish = false;
            isInPause = false;
            barAnimationP.CurrentAnimation = "PRINT";
            SetPrintImage(0);
            printImage.Texture = Building.textures[type];
        }
    }

    public void PauseOrResume()
    {
        if (isPrinting)
        {
            isInPause = !isInPause;
            if (isInPause)
            {
                barAnimationP.CurrentAnimation = "ENDPRINT";
            }
            else
            {
                barAnimationP.CurrentAnimation = "PRINT";
            }
        }
    }
    public void Claim()
    {
        isPrinting = false;
        isFinish = false;
        isInPause = false;
        printingLevel = 0.0f;
        SetPrintImage(0);
        barAnimationP.CurrentAnimation = "ENDPRINT";
    }
    private void Finish()
    {
        barAnimationP.CurrentAnimation = "ENDPRINT";
        isFinish = true;
        isInPause = false;
    }

    public void Cancel()
    {
        Claim();
    }
    
}
