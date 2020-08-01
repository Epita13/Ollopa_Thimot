using Godot;
using System;

public class EndMenu : Control
{
    private LineEdit le;
    private Button publish;
    private bool published = false;
    public override void _EnterTree()
    {
        GetNode<Label>("Time").Text = "Time : " + Game.GetTimePlayedString();
        le = GetNode<LineEdit>("Name");
        publish = GetNode<Button>("BtnSendScore");
    }

    public override void _Process(float delta)
    {
        publish.Disabled = !NameIsGood() || published;
    }

    private bool NameIsGood()
    {
        bool res = le.Text.Length > 0 && le.Text.Length <= 40;
        return res;
    }
    private void _on_Button_button_down()
    {
        GetTree().Quit();
    }

    private void _on_BtnSendScore_button_down()
    {
        if (NameIsGood())
        {
            DBConnection.SendScore(this, le.Text);
            published = true;
        }
    }
}
