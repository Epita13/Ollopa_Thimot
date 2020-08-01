using Godot;
using System;
using System.Collections.Generic;

public class MenuLoadGame : Node2D
{
    private static PackedScene SaveBox = GD.Load<PackedScene>("res://Assets/Objects/UI/Menus/other/SaveGameBox.tscn");

    private int page = 0;
    private int pageNumber;
    private int saveNumber;
    private List<string> saves;

    private Button btnBack;
    private Label pageLabel;
    private Button btnNext;

    private string saveSelected = null;
    
    private Control listSaves;
    
    public override void _Ready()
    {
        btnBack = GetNode<Button>("CanvasLayer/GestionPages/BtnBack");
        btnNext = GetNode<Button>("CanvasLayer/GestionPages/BtnNext");
        pageLabel = GetNode<Label>("CanvasLayer/GestionPages/pages");
        
        saves = Save.GetSaves();
        saveNumber = saves.Count;
        pageNumber = saveNumber / 6 + 1;
        listSaves = GetNode<Control>("CanvasLayer/saves");
        ClearSave();
        DrawPage(page);
        RefreshLabelPage();
    }

    public void DrawPage(int page)
    {
        if (page < pageNumber)
        {
            ClearSave();
            int i = page * 6;
            while (i < saveNumber && i < page*6+6)
            {
                SaveGameBox saveBox = (SaveGameBox)SaveBox.Instance();
                saveBox.saveName = saves[i];
                listSaves.AddChild(saveBox);
                saveBox.Connect("buttonClick", this, "_on_save_clicked");
                saveBox.RectMinSize = new Vector2(saveBox.RectSize.x, listSaves.RectSize.y/3);
                i++;
            }
        }
    }

    private void ClearSave()
    {
        foreach (var c in listSaves.GetChildren())
        {
            listSaves.RemoveChild((Node)c);
            ((Node)c).QueueFree();
        }
    }

    private void RefreshLabelPage()
    {
        pageLabel.Text = (page+1) + "/" + pageNumber;
    }
    public override void _Process(float delta)
    {
        btnBack.Disabled = page==0;
        btnNext.Disabled = page == pageNumber - 1;
    }

    public void _on_BtnBack_button_down()
    {
        page--;
        DrawPage(page);
        RefreshLabelPage();
        RefreshSelected(saveSelected);
    }

    public void _on_BtnNext_button_down()
    {
        page++;
        DrawPage(page);
        RefreshLabelPage();
        RefreshSelected(saveSelected);
    }

    public void _on_save_clicked(string saveName)
    {
        RefreshSelected(saveName);
    }

    private void RefreshSelected(string saveName)
    {
        int i = page * 6;
        while (i < saveNumber && i < page * 6 + 6)
        {
            if (saves[i] == saveName)
            {
                ((SaveGameBox)listSaves.GetChild(i-page*6)).SetOutline();
                saveSelected = saveName;
            }
            else
            {
                ((SaveGameBox)listSaves.GetChild(i-page*6)).ResetOutline();
            }
            i++;
        }
    }

    public void _on_BtnLoad_mouse_click()
    {
        if (saveSelected != "")
        {
            Game.load = true;
            Game.saveName = saveSelected;
            Game.saveName = saveSelected;
            GetTree().ChangeScene("res://Assets/Scenes/Jeux/Game.tscn");
        }
    }
}
