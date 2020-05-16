using Godot;
using System;
using System.Collections.Generic;

public class PlayerMouvements : KinematicBody2D
{

	/*Sound*/
	private AudioStreamPlayer2D audioStream;

    public static bool HasPlayer = false;
    
    public static PlayerMouvements instance;
    public static Vector2 size = new Vector2(1.5f,2.5f);

    public static float GRAVITY = 900;
    public static float YLIMITESPEED = 400.0f;
    public static float SPEED = 125;
    public static float JUMP_POWER = -16000;
    public static bool canMove = true;

    public static float LiquidCoefMove = 1.0f; 

    public static Vector2 initialPosition = new Vector2(10,46f);
    
    Vector2 UP = new Vector2(0,-1);
    Vector2 vel;
    bool on_ground;

    Vector2 depopos;
    
    /*annimation*/
    private bool move_right = false;
	private bool move_left = false;

	/*Chunk extremité du joueur*/
    private Chunk chunkLeft = null;
    private Chunk chunkRight = null;
    
    public override void _EnterTree()
    {
        HasPlayer = true;
        depopos = Position;
        instance = this;
        Teleport(initialPosition.x, initialPosition.y);
        if (!World.IsInit)
        {
            GD.Print("Player : Warning, le monde n'est pas initialisé");
        }

        audioStream = GetNode<AudioStreamPlayer2D>("Stream");
    }

    public static float GetX() => Convertion.Location2World(instance.Position).x;
    public static float GetY() => Convertion.Location2World(instance.Position).y;

    public static void Teleport(float x, float y)
    {
        instance.Position = Convertion.World2Location(new Vector2(x,y));
    }

    public static bool IsInWater()
    {
	    Vector2 playerPos = new Vector2(GetX(), GetY());
	    int xmin = Mathf.FloorToInt(playerPos.x - size.x / 2);
	    int ymin = Mathf.FloorToInt(playerPos.y-size.y/2);
	    int xmax = Mathf.FloorToInt(playerPos.x+size.x/2);
	    int ymax = Mathf.FloorToInt(playerPos.y+size.y/2);
	    for (int x = xmin; x <= xmax; x++)
	    {
		    for (int y = ymin; y <= ymax; y++)
		    {
			    if (y < 0 || y >= Chunk.chunkMax)
				    continue;
			    int nx = x < 0 ? World.size * Chunk.size + x : x;
			    nx = nx % (World.size * Chunk.size);
			    if (Liquid.list[Liquid.Type.Water].map[nx, y] > 0)
			    {
				    return true;
			    }
		    }
	    }
	    return false;
    }
    private void HorizontalMouvement(float delta)
    {
        AnimationPlayer bond = GetNode<AnimationPlayer>("AnimationPlayer");
		Sprite image = GetNode<Sprite>("Image");
		
		/////////Move Right
		
		if (Input.IsActionJustPressed("ui_right")&& move_left==false)
		{
			image.FlipH = false;
			if(IsOnFloor())
				bond.Play("Turn");
		}                                                        ///Cannot input Turn because it wont be seen and will be replace by run
		if (Input.IsActionPressed("ui_right") && move_left==false )
		{
			move_right = true;
			image.FlipH = false;
			vel.x = SPEED * LiquidCoefMove;
			if(IsOnFloor())
				bond.Play("Run");
		}
		else if (Input.IsActionJustReleased("ui_right") && move_left==false)
		{
			move_right = false;
			if(IsOnFloor())
				bond.Play("Turn_Back");
		}
		
		/////////Move Left
		if (Input.IsActionJustPressed("ui_left")&& move_right==false)
		{
			image.FlipH=true;
			if(IsOnFloor())
				bond.Play("Turn");
		}
		if (Input.IsActionPressed("ui_left") && move_right==false)
		{
			move_left = true;
			image.FlipH=true;
			vel.x = -SPEED * LiquidCoefMove;
			if(IsOnFloor())
				bond.Play("Run");
		}
		else if (Input.IsActionJustReleased("ui_left") && move_right==false)
		{
			move_left = false;
			if(IsOnFloor())
				bond.Play("Turn_Back");
		}
    }

    private void JUMP(float delta)
    {
		AnimationPlayer bond = GetNode<AnimationPlayer>("AnimationPlayer");
		Sprite image = GetNode<Sprite>("Image");
		
		if (IsOnFloor())
		{
			on_ground = true;
			vel.y = 0;
		}
		else
		{
			on_ground = false;
		}

		if (LiquidCoefMove != 1.0f && Input.IsActionPressed("ui_up"))
		{
			vel.y += JUMP_POWER * 0.2f * LiquidCoefMove * delta;
		}
		else if (on_ground && Input.IsActionPressed("ui_up") && LiquidCoefMove==1.0f)
		{
			bond.Play("Start_Jump");
			vel.y += JUMP_POWER * LiquidCoefMove * delta;
			on_ground = false;
		}
		else if (vel.y != 0 && (!bond.IsPlaying() || bond.CurrentAnimation=="Run"))
		{
			bond.Play("Jump");
		}
    }

    public override void _PhysicsProcess(float delta)
    {
        AnimationPlayer bond = GetNode<AnimationPlayer>("AnimationPlayer");
		Sprite image = GetNode<Sprite>("Image");

		if (IsInWater())
		{
			LiquidCoefMove = Liquid.density[Liquid.Type.Water];
		}
		else
		{
			LiquidCoefMove = 1.0f;
		}
		
		vel.x = 0;
		
		if (Player.health <= 0 && PlayerState.GetState()!=PlayerState.State.Dead)   ///////// Death
		{
			bond.Play("Death");
			canMove = false;
			PlaySound(Player.Sounds.PlayerDeath);
			PlayerState.SetState(PlayerState.State.Dead);
		}
		
		if(canMove && (PlayerState.GetState()==PlayerState.State.Normal || PlayerState.GetState()==PlayerState.State.Build || PlayerState.GetState()==PlayerState.State.Link))
		{
			HorizontalMouvement(delta);
			JUMP(delta);
			if (vel.x == 0)      ////////// Idle
			{
				if (!bond.IsPlaying())
				{
					image.FlipH = false;
					bond.Play("Idle");
				}
			}
		}
		
		vel.y += GRAVITY * LiquidCoefMove * delta;
		vel.y = vel.y < -YLIMITESPEED ? -YLIMITESPEED : vel.y; 
		vel.y = vel.y > YLIMITESPEED ? YLIMITESPEED : vel.y; 
		
		MoveAndSlide(vel,UP);
    }

    public override void _Process(float delta)
  {
        if (World.IsInit)
        {
            WorldEndTeleportation();
        }
        Player.RemoveOxygene(Player.oxygeneLoss*delta);
        Player.RemoveEnergy(Player.energyLoss*delta);
        if (Player.oxygene == 0)
        {
	        Player.RemoveHealth(Player.oxygeneDamage*delta);
        }
        if (Player.energy == 0)
        {
	        Player.RemoveHealth(Player.energyDamage*delta);
        }

  }



  private void WorldEndTeleportation()
  {


      float vecMinX = Game.GetScreenMinX();
      float vecMaxX = Game.GetScreenMaxX();

      Chunk NchunkLeft = World.GetChunk(Mathf.FloorToInt(vecMinX));
      Chunk NchunkRight = World.GetChunk(Mathf.FloorToInt(vecMinX));

      if (chunkLeft != NchunkLeft || chunkRight != NchunkRight)
      {
          chunkLeft = NchunkLeft;
          chunkRight = NchunkRight;
          int x = Mathf.FloorToInt(vecMinX);
          x = Chunk.size * Mathf.FloorToInt(x / (float)Chunk.size);
          List<Chunk> nextVisbleChunks = new List<Chunk>();
          List<int> nextVisbleChunksPos = new List<int>();
          while (x <= Mathf.CeilToInt(vecMaxX) + Chunk.size)
          {
              nextVisbleChunks.Add(World.GetChunk(x));
              nextVisbleChunksPos.Add(x);
              x += Chunk.size;
          }
          int a = 0;
          while (a < World.visibleChunks.Count)
          {
              Chunk c = World.GetChunkWithID(World.visibleChunks[a].Item1);
              int index = nextVisbleChunks.IndexOf(c);
              if (index == -1)
              {
	              World.HideChunkc(c);
                  a--;
              }else if (nextVisbleChunksPos[index] != World.visibleChunks[a].Item2)
              {
	              World.HideChunkc(c);
	              a--;
              }
              a++;
          }

          for (int i = 0; i < nextVisbleChunks.Count; i++)
          {
	          if (!World.visibleChunks.Contains((nextVisbleChunks[i].id, nextVisbleChunksPos[i])))
	          {
		          nextVisbleChunks[i].DrawClone(nextVisbleChunksPos[i]);
              }
          }
      }
      if (GetX() < 0)
      {
          Teleport(World.size*Chunk.size + GetX(),GetY());
          chunkLeft = null;
          chunkRight = null;
      }
      if (GetX() > World.size*Chunk.size)
      {
          Teleport(GetX()-World.size*Chunk.size,GetY()); 
          chunkLeft = null;
          chunkRight = null;
      }
  }





  public void _on_AnimationPlayer_animation_finished(string animationName)
  {
	  if (animationName == "Death")
	  {
		  DeathMenu.ShowMenu();
	  }
  }




  private void PlaySound(Player.Sounds sound)
  {
	  audioStream.Stream = Player.sounds[sound];
	  audioStream.Playing = true;
  }
  
  
  
  

  
}
