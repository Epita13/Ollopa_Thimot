using Godot;
using System;
using System.Collections.Generic;

public class PlayerBuildingActions : Node2D
{
    private static float radius = 8;

    private Line2D line2d = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        
        Vector2 playerPosition = new Vector2(PlayerMouvements.GetX(), PlayerMouvements.GetY());
        
        List<Building> buildingsNear = new List<Building>();
        int x = (int) (playerPosition.x - radius);
        x = Chunk.size * Mathf.FloorToInt(x / (float) Chunk.size);
        for (; x <= (int) (playerPosition.x + radius); x += Chunk.size)
        {
            Chunk c = World.GetChunk(x);
            List <Building> l = World.placedBuildingByChunk[c];
            foreach (var b in l)
            {
                buildingsNear.Add(b);
            }
        }
        RemoveLine();
        foreach (var b in buildingsNear)
        {
            if (isInRange(b, playerPosition))
            {
                if (Building.buildingGiveEnergy2Player.Contains(b.type))
                {
                    TransfereEnergy(b, delta);
                }
            }
        }

    }


    private bool isInRange(Building b, Vector2 playerPos)
    {
        Vector2 p = b.locationNow;
        float distance = Mathf.Sqrt(Mathf.Pow(p.x - playerPos.x, 2) + Mathf.Pow(p.y - playerPos.y, 2));
        return distance <= radius;
    }

    private void TransfereEnergy(Building b, float delta)
    {
        float energy = Building.powerEnergy2Player * delta;
        if (b.energy >= energy && Player.energy+energy <= Player.energyMax)
        {
            b.RemoveEnergy(energy);
            Player.AddEnergy(energy);
            DrawLine(b, Color.Color8(0,125,255));
        }
    }

    private void DrawLine(Building b, Color color)
    {
        line2d = new Line2D();
        line2d.DefaultColor = color;
        Game.root.AddChild(line2d);
        line2d.Width = 2;
        line2d.AddPoint(PlayerMouvements.instance.Position);
        line2d.AddPoint(b.Position);
    }
    private void RemoveLine()
    {
        if (line2d != null)
        {
            line2d.QueueFree();
            line2d = null;
        }
    }
    
}
