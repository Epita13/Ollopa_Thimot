using Godot;
using System;

public class DeathMenu : Control
{
    public static bool menuOn = false;
    public static void ShowMenu()
    {
        if (!menuOn)
        {
            DeathMenu deathMenu = (DeathMenu)GD.Load<PackedScene>("res://Assets/Objects/UI/Menus/DeathMenu.tscn").Instance();
            deathMenu.Modulate = Color.Color8(255,255,255,150);
            Game.root.GetNode("CanvasLayer").AddChild(deathMenu);
            menuOn = true;
        }
    }

    private float fadeTime = 1.5f;
    private float time;
    private bool finish = false;

    private Button btnRevive;
    private Button btnQuit;
    
    public override void _EnterTree()
    {
        btnRevive = GetNode<Button>("c/BtnRevive");
        btnQuit = GetNode<Button>("c/BtnQuit");
        btnQuit.Disabled = true;
        btnRevive.Disabled = true;
        time = 0;
    }

    public override void _Process(float delta)
    {
       if (finish)
            return;
        
        time += delta;
        float a = time * 255.0f / fadeTime;
        a = a > 255 ? 255 : a;
        Modulate = Color.Color8(255,255,255,(byte)a);
        if (time  >= fadeTime)
        {
            time = 0;
            finish = true;
            btnQuit.Disabled = false;
            btnRevive.Disabled = false;
        }
    }

    public void _on_BtnRevive_button_down()
    {
        menuOn = false;
        Player.Revive();
        PlayerState.SetState(PlayerState.State.Normal);
        QueueFree();
    }

    public void _on_BtnQuit_button_down()
    {
        Save._Save(World.saveName);
        GetTree().Quit();
    }
}
