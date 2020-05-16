using Godot;
using System;
using System.Collections.Generic;

public class PlayerMouvements : KinematicBody2D
{


    public static bool HasPlayer = false;
    
    public static PlayerMouvements instance;
    public static Vector2 size = new Vector2(1.625f,3);

    public static float GRAVITY = 10; 
    public static float SPEED = 125*2.5f*0.6f;
    public static float JUMP_POWER = -250*1.4f;
    public static bool canMove = true;

    public static Vector2 initialPosition = new Vector2(10,46f);
    
    Vector2 UP = new Vector2(0,-1);
    Vector2 vel;
    bool on_ground;

    Vector2 depopos;
    
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
    }

    public static float GetX() => Convertion.Location2World(instance.Position).x;
    public static float GetY() => Convertion.Location2World(instance.Position).y;

    public static void Teleport(float x, float y)
    {
        instance.Position = Convertion.World2Location(new Vector2(x,y));
    }

    private void HorizontalMouvement()
    {
        if (Input.IsActionPressed("ui_right"))
            vel.x = SPEED;
        else if (Input.IsActionPressed("ui_left"))
            vel.x = -SPEED;
    }

    private void JUMP()
    {
        if (IsOnFloor()){
            on_ground = true;
            vel.y = 0;
        }
        else
            on_ground = false;
        if (on_ground && Input.IsActionPressed("ui_up")){
            vel.y = JUMP_POWER;
            on_ground = false;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        vel.x = 0;
        if (canMove && (PlayerState.GetState()==PlayerState.State.Normal || PlayerState.GetState()==PlayerState.State.Build || PlayerState.GetState()==PlayerState.State.Link)){
            HorizontalMouvement();
            JUMP();
        }

        vel.y += GRAVITY;

        MoveAndSlide(vel,UP);
    }


  public override void _Process(float delta)
  {
        if (World.IsInit)
        {
            WorldEndTeleportation();
        }

        Player.RemoveEnergy(0.1f*delta);
        
  }



  private void WorldEndTeleportation()
  {
      Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
      int viewportSizeX = Mathf.FloorToInt(GetViewport().Size.x * CurrentCamera.GetXZoom());
      Vector2 vecMin = Convertion.Location2World(p) * -1;
      Vector2 vecMax = Convertion.Location2World(new Vector2(p.x*-1+viewportSizeX, p.y));
      
      Chunk NchunkLeft = World.GetChunk(Mathf.FloorToInt(vecMin.x));
      Chunk NchunkRight = World.GetChunk(Mathf.FloorToInt(vecMin.x));

      if (chunkLeft != NchunkLeft || chunkRight != NchunkRight)
      {
          chunkLeft = NchunkLeft;
          chunkRight = NchunkRight;
          int x = Mathf.FloorToInt(vecMin.x);
          x = Chunk.size * Mathf.FloorToInt(x / (float)Chunk.size);
          List<Chunk> nextVisbleChunks = new List<Chunk>();
          List<int> nextVisbleChunksPos = new List<int>();
          while (x <= Mathf.CeilToInt(vecMax.x) + Chunk.size)
          {
              nextVisbleChunks.Add(World.GetChunk(x));
              nextVisbleChunksPos.Add(x);
              x += Chunk.size;
          }

          int a = 0;
          while (a < World.visibleChunks.Count)
          {
              Chunk c = World.GetChunkWithID(World.visibleChunks[a].Item1);
              if (!nextVisbleChunks.Contains(c))
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
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  

  
}
