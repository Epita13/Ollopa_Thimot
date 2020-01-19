using Godot;
using System;

public class PlayerMouvements : KinematicBody2D
{
    

    public static float GRAVITY = 10; 
    public static float SPEED = 120;
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
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      Player.RemoveEnergy(2*delta);
      Player.PrintEnergy();
      if(Player.energy==0)
      {
          canMove = false;
      }
  }
}
