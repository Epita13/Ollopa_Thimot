using Godot;
using System;

public class End : Control
{
    private void _on_Button_button_down()
    {
        GetTree().Quit();
    }
}
