using Godot;
using System;
using System.Reflection;

public class Raygun : Node2D
{


    public const float POWER = 80f;
    public const float RADIUS = 250;

    private float delta;
    
    [Signal]
    public delegate void hit(Vector2 xy,Vector2 az);

    [Signal]
    public delegate void shooting(Vector2 xy, Vector2 az);

    private RayCast2D raycast;
    
    private Sprite anSprite;
    private Line2D ray;
	

    public override void _Ready()
    {
        anSprite = GetNode<Sprite>("Sprite_Raygun");
        raycast = anSprite.GetNode<RayCast2D>("shoot_ray");
        ray = GetNode<Line2D>("Ray");
        raycast.CastTo = new Vector2(RADIUS,raycast.CastTo.y);
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
            }else if (hit_collider is StaticBody2D)
            {
                // Collide with a tree
                StaticBody2D s = (StaticBody2D) hit_collider;
                if (s.GetGroups().Contains("tree"))
                {
                    TreeCollosion();
                }

            }
            
            
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
        if (vec.x >= 0 && vec.y >= 0)
        {
            if (block_pos.y == Mathf.Floor(block_pos.y))
                block_posF += new Vector2(0,-1.0f);
        } else if (vec.x < 0 && vec.y >= 0)
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
        if (block_hit.GetType != Block.Type.Air)
            block_hit.Damage(POWER*delta);
    }

    private void TreeCollosion()
    {
        Tree t = (Tree)raycast.GetCollider();
        t.Damage(POWER*delta);
    }


    public override void _Process(float delta)
    {
        this.delta = delta;
        if (PlayerState.GetState() != PlayerState.State.Normal || Player.UsableSelected != Usable.Type.Laser)
        {
            ray.Visible = false;
            anSprite.Visible = false;
            return;
        }
        ray.Visible = true;
        anSprite.Visible = true;
        LookAt(GetGlobalMousePosition());
        if (GlobalRotation < -Mathf.Pi / 2 || GlobalRotation > Mathf.Pi / 2)
        {
            anSprite.FlipV = true;
        }
        else
        {
            anSprite.FlipV = false;
        }
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
