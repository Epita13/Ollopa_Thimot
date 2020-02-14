using Godot;
using System;

public class PlayerMouvements : KinematicBody2D
{
    

    public static float GRAVITY = 10; 
    public static float SPEED = 250;
    public static float JUMP_POWER = -250;
    public static bool canMove = true;

    Vector2 UP = new Vector2(0,-1);
    Vector2 vel;
    bool on_ground;

    public override void _Ready()
    {
        
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
        if (canMove){
            HorizontalMouvement();
            JUMP();
        }

        vel.y += GRAVITY;

        MoveAndSlide(vel,UP);
    }


  public override void _Process(float delta)
  {
      Player.RemoveEnergy(0.1f*delta);
      if(Player.energy==0)
        {
          canMove = false;
        }
    if (Input.IsActionJustPressed("mouse1")){
        //Storage sp = (Storage)Building.prefabs[Building.Type.Storage].Instance();
        //sp.Place(pos);
        Vector2 pos = GetGlobalMousePosition();
        Vector2 pos1 = Convertions.Location2World(pos);
        Vector2 pos2 = Convertions.Location2WorldFloor(pos);
        GD.Print("\nValuers brutes : ", pos);
        GD.Print("Valuers Converties (decimal) : ", pos1);
        GD.Print("Valuers Converties (entier) : ", pos2);
        Chunk c = World.GetChunk((int)pos2.x);
        //c.AddBlock(Chunk.GetLocaleX((int)pos2.x), (int)pos2.y, Block.Type.Stone);
        Liquid water =  (Liquid)GetTree().GetRoot().GetNode("SceneGeneration").GetNode("Liquid").GetNode("WaterMap");
        water.PlaceWater((int) pos2.x, (int) pos2.y);
        
    }
  }
}
