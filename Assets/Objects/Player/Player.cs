using Godot;
using System;

public class Player : KinematicBody2D
{
    
    const float GRAVITY = 10; 
    const float SPEED = 70;
    const float JUMP_POWER = -250;
    Vector2 UP = new Vector2(0,-1);

    Vector2 vel;
    bool on_ground;

    public override void _Ready()
    {
        
    }


public override void _PhysicsProcess(float delta)
{
    /*Horizontale deplacement*/
    if (Input.IsActionPressed("ui_right"))
        vel.x = SPEED;
    else if (Input.IsActionPressed("ui_left"))
        vel.x = -SPEED;
    else
        vel.x = 0;

    /* JUMP */
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
    

    vel.y += GRAVITY;
    Console.WriteLine(vel.y);
    MoveAndSlide(vel,UP);
}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
