using Godot;
using System;
using System.Collections.Generic;

public class MenuNewGame : Node2D
{

    public static int defaultWorldSize = 25;
    
    
    private LineEdit name;
    private LineEdit seed;

    private Label nameState;
    private Label seedState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        name = GetNode<LineEdit>("CanvasLayer/GameName");
        seed = GetNode<LineEdit>("CanvasLayer/GameSeed");
        
        nameState = GetNode<Label>("CanvasLayer/GameName/state");
        seedState = GetNode<Label>("CanvasLayer/GameSeed/state");

        nameState.Visible = false;
        seedState.Visible = false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }


    public void _on_BtnNewGame_mouse_click()
    {
        bool v1 = VerificationOfName();
        bool v2 = VerificationOfSeed();
        if (v1 && v2)
        {
            if (seed.Text != "")
            {
                World.SetSeed(Base36ToInt(seed.Text));
            }
            World.SetSize(defaultWorldSize);
            World.SetSaveName(name.Text);
            GetTree().ChangeScene("res://Assets/Scenes/Jeux/Game.tscn");
        }

    }

    private bool VerificationOfName()
    {
        List<string> strings = Save.GetSaves();
        if (name.Text == "")
        {
            nameState.Visible = true;
            nameState.Text = "The save name can't be empty.";
            return false;
        }
        if (strings.Contains(name.Text))
        {
            nameState.Visible = true;
            nameState.Text = "The save name already exsit.";
            return false;
        }
        return true;
    }
    private bool VerificationOfSeed()
    {
        int length = seed.Text.Length;
        string s = seed.Text;
        bool res = true;
        s = s.ToUpper();
        int i = 0; 
        while (i < length && res)
        {
            if ((s[i] < '0' || s[i] > '9') && (s[i] < 'A' || s[i] > 'Z'))
                res = false;
            i++;
        }

        if (!res)
        {
            seedState.Visible = true;
        }
        return res;
    }


    public void _on_GameName_text_changed(string s)
    {
        nameState.Visible = false;
    }

    public void _on_GameSeed_text_changed(string s)
    {
        seedState.Visible = false;
    }


    private int Base36ToInt(string s)
    {
        s = s.ToUpper();
        int i = s.Length() - 1;
        int res = 0;
        int power = 1;
        while (i >= 0)
        {
            res += power * GetCharBase36(s[i]);
            power *= 36;
            i--;
        }
        return res;
    }

    private int GetCharBase36(char c)
    {
        if (c >= '0' && c <= '9')
        {
            return (int)(c - '0');
        }
        return (int)(c - 'A' + 10);
    }
}
