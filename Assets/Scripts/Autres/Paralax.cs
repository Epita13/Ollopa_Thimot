using Godot;
using System;
using System.Collections.Generic;

public class Paralax : Node2D
{
    
    [Export] public float speed = 0.5f;
    [Export] public Texture texture;
    [Export] public int zIndex;
    [Export] public float y;
    
    List<Sprite> childs = new List<Sprite>();

    private Vector2 viewportPos;
    private int nbImage = 2;
    
    public override void _Ready()
    {
        if (!PlayerMouvements.HasPlayer)
            throw new UninitializedException("Paralax", "PlayerMouvement");
        viewportPos = GetViewportTransform().origin * CurrentCamera.GetXZoom();

        Sprite s1 = new Sprite();
        Sprite s2 = new Sprite();
        s1.Texture = texture;
        s2.Texture = texture;
        s1.ZIndex = zIndex;
        s2.ZIndex = zIndex;
        s1.Centered = true;
        s2.Centered = true;
        s1.Position = new Vector2(PlayerMouvements.instance.Position.x, y);
        s2.Position = new Vector2(PlayerMouvements.instance.Position.x+texture.GetSize().x, y);
        AddChild(s1);
        AddChild(s2);
        childs.Add(s1);
        childs.Add(s2);
    }

    public override void _Process(float delta)
    {
        int viewportSizeX = Mathf.FloorToInt(GetViewport().Size.x * CurrentCamera.GetXZoom());
        Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
        float xdiff = viewportPos.x - p.x;
        float ydiff = viewportPos.y - p.y;
        
        /*Detection de teleportation a un seuil de 95%*/
        if (Mathf.Abs(xdiff) >= 0.95f * World.size * Chunk.size * World.BlockTilemap.CellSize.x)
        {
            foreach (var s in childs)
            {
                s.Position = new Vector2(s.Position.x+xdiff, s.Position.y + ydiff*speed);
            }
        }
        else
        {
            foreach (var s in childs)
            {
                s.Position = new Vector2(s.Position.x + xdiff*speed, s.Position.y+ ydiff*speed);
                if (s.Position.x + texture.GetSize().x / 2 < -p.x)
                {
                    s.Position = new Vector2(s.Position.x + texture.GetSize().x*2, s.Position.y);
                }
                if (s.Position.x - texture.GetSize().x / 2 > (-p.x)+viewportSizeX)
                {
                    s.Position = new Vector2(s.Position.x - texture.GetSize().x*2, s.Position.y);
                }
            }
        }
        viewportPos = GetViewportTransform().origin * CurrentCamera.GetXZoom();
    }
}
