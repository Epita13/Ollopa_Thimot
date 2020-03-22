using Godot;
using System;
using System.Reflection;

public class Raygun : Node2D
{

    [Signal]
    public delegate void hit(Vector2 xy,Vector2 az);

    [Signal]
    public delegate void shooting(Vector2 xy, Vector2 az);

    private RayCast2D raycast;
    private AnimatedSprite anSprite;
	

    public override void _Ready()
    {
        anSprite = GetNode<AnimatedSprite>("Sprite_Raygun");
        raycast = anSprite.GetNode<RayCast2D>("shoot_ray");
    }
	
    public void shoot()
    {
        if (raycast.IsColliding())
        {
            var hit_collider = raycast.GetCollider();
            // Collide with a block
            if (hit_collider is TileMap)
            {
                BlockCollision();
            }
        }
        else
        {
            //GD.Print("shoot");
        }
    }

    private void BlockCollision()
    {
        TileMap tilemap = (TileMap)raycast.GetCollider();
        var hit_pos = raycast.GetCollisionPoint();
        var raygun_pos = Position + GlobalPosition;
        var vec = hit_pos - raygun_pos;
        var block_pos = Convertion.Location2World(hit_pos);
        var block_posF = Convertion.Location2WorldFloor(hit_pos);
        if (vec.x > 0 && vec.y > 0)
        {
            if (block_pos.y == Mathf.Floor(block_pos.y))
                block_posF += new Vector2(0,-1.0f);
        } else if (vec.x < 0 && vec.y > 0)
        {
            if (block_pos.y == Mathf.Floor(block_pos.y))
                block_posF += new Vector2(0,-1.0f);
            if (block_pos.x == Mathf.Floor(block_pos.x))
                block_posF += new Vector2(-1.0f,0);
        } else if (vec.x < 0 && vec.y < 0)
        {
            if (block_pos.x == Mathf.Floor(block_pos.x))
                block_posF += new Vector2(-1.0f,0);
        }

        Block block_hit = World.GetBlock((int) block_posF.x, (int) block_posF.y);
        block_hit.Damage(3.0f);
    }


    public override void _Process(float delta)
    {
        if (PlayerState.GetState() != PlayerState.State.Normal || Player.UsableSelected != Usable.Type.Laser)
            return;
        
        LookAt(GetGlobalMousePosition());

        if (Input.IsActionPressed("mouse1"))
        {
            shoot();
        }
    }


    public void Ray()
    {
        Vector2 globalRaycastPos = (new Vector2(Mathf.Cos(GlobalRotation)*raycast.Position.x, Mathf.Sin(GlobalRotation)*raycast.Position.y)) + GlobalPosition;
        var d = raycast.GetCollisionPoint() - globalRaycastPos;
        float length = d.Length();
        Line2D s = GetNode<Line2D>("Ray");
        s.Visible = true;
        s.SetPointPosition(1, new Vector2(length/Scale.x, 0));

    }
}
