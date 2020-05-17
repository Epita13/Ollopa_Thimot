using Godot;
using System;
using System.Collections.Generic;

public class Animation : Control
{
    private List<Texture> alltextures = new List<Texture>();
    private Random rnd = new Random();

    private TextureRect tr;
    private List<(TextureRect,Vector2)> alltextureRect = new List<(TextureRect,Vector2)>();

    private int textureSize = 20;
    private float speed = 40.0f;

    private Vector2 screenSize;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        screenSize = GetViewport().Size;
        LoadTextures();
        SpawnAllRect();
    }

    public override void _Process(float delta)
    {
        Vector2 v = GetViewport().Size;
        if (v != screenSize)
        {
            RemoveAll();
            SpawnAllRect();
            screenSize = v;
        }
        foreach (var tr in alltextureRect)
        {
            tr.Item1.SetPosition(new Vector2(tr.Item1.RectPosition.x+speed*delta, tr.Item1.RectPosition.y+speed*delta));
            if (tr.Item1.RectPosition.x >= GetViewport().Size.x + textureSize)
            {
                tr.Item1.SetPosition(new Vector2(-textureSize, tr.Item1.RectPosition.y));
            }
            if (tr.Item1.RectPosition.y >= GetViewport().Size.y + textureSize)
            {
                tr.Item1.SetPosition(new Vector2(tr.Item1.RectPosition.x, -textureSize));
            }
            
        }
    }

    private void RemoveAll()
    {
        foreach (var c in GetChildren())
        {
            RemoveChild((Node)c);
            ((Node)c).QueueFree();
        }
        alltextureRect.Clear();
    }

    private TextureRect GetRandomTextureRect()
    {
        TextureRect tr = new TextureRect();
        int index = rnd.Next(0, alltextures.Count);
        tr.Texture = alltextures[index];
        tr.Expand = true;
        tr.SetSize(new Vector2(textureSize,textureSize));
        return tr;
    }

    private void SpawnAllRect()
    {
        int yy = 0;
        for (int y = -textureSize; y < GetViewport().Size.y + textureSize; y += textureSize)
        {
            int xx = 0;
            for (int x = -textureSize; x < GetViewport().Size.x+textureSize; x+=textureSize)
            {
                if ((xx+yy)%2==0)
                    AddRect(x,y);
                xx++;
            }
            yy++;
        }
    }

    private void AddRect(int x, int y)
    {
        TextureRect tr = GetRandomTextureRect();
        AddChild(tr);
        tr.SetPosition(new Vector2(x,y));
        alltextureRect.Add((tr, new Vector2(x,y)));
    }
    private void LoadTextures()
    {
        /*Items*/
        for (int i = 0; i < Item.nbItems; i++)
        {
            alltextures.Add(Item.textures[i]);
        }
        /*Buildings*/
        for (int i = 0; i < Building.nbBuildings; i++)
        {
            alltextures.Add(Building.textures[(Building.Type)i]);
        }
    }
    
}
