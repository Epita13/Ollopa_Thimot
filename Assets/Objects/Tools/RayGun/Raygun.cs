using Godot;
using System;
using System.Reflection;

public class Raygun : Node2D
{
	public const float POWER = 80f;
	public const float RADIUS = 350f;
	public const float POWERENERGY = 0.55f;

	public const bool can_shoot = true;
	private bool isalreadyshooting = false;
	
	[Signal]
	public delegate void hit(Vector2 xy,Vector2 az);

	[Signal]
	public delegate void shooting(Vector2 xy, Vector2 az);

	private RayCast2D raycast;
	private Sprite anSprite;

	private Sprite begin;
	private Sprite beam;
	private Position2D end;
	private Particles2D particule;

	public override void _Ready()
	{
		anSprite = GetNode<Sprite>("Sprite_Raygun");
		raycast = anSprite.GetNode<RayCast2D>("shoot_ray");
		beam = GetNode<Sprite>("beam");
		begin = GetNode<Sprite>("begin");
		end = GetNode<Position2D>("end");
		particule = end.GetNode<Particles2D>("explosion");
		raycast.CastTo = new Vector2(RADIUS,raycast.CastTo.y);
		begin.Visible = false;
		beam.Visible = false;
	}
	
	public void shoot(float delta)
	{
		begin.Visible = true;
		beam.Visible = true;
		if (raycast.IsColliding())
		{
			end.GlobalPosition = raycast.GetCollisionPoint();
			
			float collision = (raycast.GetCollisionPoint()-raycast.GlobalPosition).Length()*500/120;
			
			beam.Rotation = raycast.CastTo.Angle();
			
			var beamRegionRect = beam.RegionRect;
			var vector2 = beamRegionRect.End;
			vector2.x = collision;
			beamRegionRect.End = vector2;
			beam.RegionRect = beamRegionRect;
			
			var hit_collider = raycast.GetCollider();
			// Collide with a block
			if (hit_collider is TileMap)
			{
				BlockCollision(delta);
			}else if (hit_collider is StaticBody2D)
			{
				// Collide with a tree
				StaticBody2D s = (StaticBody2D) hit_collider;
				if (s.GetGroups().Contains("tree"))
				{
					TreeCollosion(delta);
				}
			}else if (hit_collider is Ennemy_Fly)
			{
				EnemyCollision(delta);
			}
		}
		else
		{
			end.GlobalPosition = raycast.CastTo;
			
			beam.Rotation = raycast.CastTo.Angle();
			var beamRegionRect = beam.RegionRect;
			var vector2 = beamRegionRect.End;
			vector2.x = raycast.CastTo.Length();
			beamRegionRect.End = vector2;
			beam.RegionRect = beamRegionRect;
		}
		particule.Emitting = true;
	}

	private void EnemyCollision(float delta)
	{
		Ennemy_Fly t = (Ennemy_Fly)raycast.GetCollider();
		t.Damage(POWER * delta);
	}

	private void BlockCollision(float delta)
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
		{
			block_hit.Damage(POWER * delta);
		}
			
	}

	private void TreeCollosion(float delta)
	{
		Tree t = (Tree)raycast.GetCollider();
		t.Damage(POWER * delta);
	}



	public override void _Process(float delta)
	{
		if (PlayerState.GetState() != PlayerState.State.Normal || Player.UsableSelected != Usable.Type.Laser)
		{
			Visible = false;
			return;
		}

		Visible = true;

			LookAt(GetGlobalMousePosition());
		if (GlobalRotation < -Mathf.Pi / 2 || GlobalRotation > Mathf.Pi / 2)
		{
			anSprite.FlipV = true;
		}
		else
		{
			anSprite.FlipV = false;
		}
		Vector2 mouse_position = GetLocalMousePosition();
		Vector2 max_cast_to = mouse_position.Normalized() * RADIUS;
		raycast.CastTo = max_cast_to;

		if (Input.IsActionPressed("mouse1"))
		{
			if (Player.energy > 0)
			{
				shoot(delta);
				particule.Emitting = true;
				Player.RemoveEnergy(POWERENERGY*delta);
				if (!isalreadyshooting)
				{
					PlayerMouvements.PlaySound(Sounds.Type.PlayerLaser);
					isalreadyshooting = true;
				}
			}
			else
			{
				begin.Visible = false;
				beam.Visible = false;
				particule.Emitting = false;
			}
		}
		if (Input.IsActionJustReleased("mouse1"))
		{
			begin.Visible = false;
			beam.Visible = false;
			particule.Emitting = false;
			isalreadyshooting = false;
		}
	}
}
