using Godot;
using System;
using System.Linq;
using Godot.Collections;

public class Ennemy_Fly : Godot.KinematicBody2D
{

    public static float Gravity = 0;
    public static float Speed = 100;
    public static float JUMP_power = -200;
    
    public float Health = 150;
    private float attack = 10;


    public float lifeTime = 90;
    public float t = 0;
    
    private float delta;
    public bool dead=false;
    private bool verifattack = false;
    private bool veriftime = false;
    private bool veriftime2 = false;
    private bool verifground = false;
    private bool verifdie = false;
    
    Vector2 up = new Vector2(0,-1);
    Vector2 vel;
    
    private int EnemyDirection = 1;
    private int OppositeDirection = -1;

    Timer time=new Timer();
    Timer time2=new Timer();
    Sprite Enemy_fly=new Sprite();
    Sprite Enemy_Die = new Sprite();
    Area2D proximity = new Area2D();
    AnimationPlayer anim = new AnimationPlayer();
    
    float move_distance =new float();
    
    private Vector2 Position_Player;

    private float prev_x_viewport;
    private bool mirrored = false;
    
    private Random rand = new Random();
    private int r = 0;
    

    void TimerTimeout()
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        
        r = rand.Next(0, 2);
        if(r==1)
            EnemyDirection = EnemyDirection * OppositeDirection;
        if (EnemyDirection == 1)
        {
            Enemy_fly.FlipH = true;
        }
        else
        {
            Enemy_fly.FlipH = false;
        }
    }

    public override void _Ready()
    {
        time = GetNode<Timer>("cooldown");
        time2 = GetNode<Timer>("afterattack");
        Enemy_fly = GetNode<Sprite>("Enemy_Fly");
        Enemy_Die = GetNode<Sprite>("Enemy_Die");
        proximity = GetNode<Area2D>("Detection");
        anim = GetNode<AnimationPlayer>("AnimationEnemy");
        anim.Play("Flying");

        Enemies.enemiesCount++;
        
        Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
        Vector2 vecMin = Convertion.Location2World(p) * -1;
        prev_x_viewport = vecMin.x;
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
            Health = 0;
        }
    }

    public void Die()
    {
        verifground = false;
        verifdie = true;
        anim.Stop();
        Enemy_fly.Visible = false;
        Enemy_Die.Visible = true;
        if (EnemyDirection == 1)
        {
            Enemy_Die.FlipH = true;
        }
        else
        {
            Enemy_Die.FlipH = false;
        }
        dead = true;
        GetNode<CollisionShape2D>("Collision").Disabled = true;
        anim.Play("Die");
    }

    public void _on_Attaque_body_entered(Node body)
    {
        if (body.GetGroups().Contains("Player"))
        {
            if (!dead)
            {
                //Dégâts par l'ennemi volant
                Player.RemoveHealth(attack);
                verifattack = true;
                if (!veriftime2)
                {
                    time2.Start();
                    veriftime2 = true;
                }
            }
        }
    }

    public void Onafterattack()
    {
        verifattack = false;
        veriftime2 = false;
    }
    
    private void Random_moves()
    {
        if (EnemyDirection == 1)
        {
            Enemy_fly.FlipH = true;
        }
        else
        {
            Enemy_fly.FlipH = false;
        }
        
        if (IsOnWall() || IsOnFloor())
        {
            vel.y += JUMP_power;
            vel.x = 0;
        }
        else
        {
            vel.y = 0;
        }

        if (!IsOnWall())
        {
            vel.x = EnemyDirection * rand.Next((int) (Speed/5),(int)(Speed));
        }
        MoveAndSlide(vel, up);
    }

    private bool Player_Near()    ////Bug lorsque j'instance la scène à Enemies
    {
        var bodies = proximity.GetOverlappingBodies();
        bool verif = false;
        int i = 0;
        int n = bodies.Count;
        while (!verif && i < n)
        {
            if (((Node2D)bodies[i]).GetGroups().Contains("Player"))
            {
                verif = true;
            }
            i++;
        }
        return verif;
    }

    private void Get_To_Player()
    {
        Position_Player = new Vector2(PlayerMouvements.GetX(), PlayerMouvements.GetY());
        Position_Player = Convertion.World2Location(Position_Player);

        Vector2 direction = (-this.GlobalPosition +Position_Player).Normalized();
        vel = direction * Speed;
        if (vel.x >= 0)
        {
            Enemy_fly.FlipH = true;
        }
        else
        {
            Enemy_fly.FlipH = false;
        }
        MoveAndSlide(vel, up);
    }

    public override void _PhysicsProcess(float delta)
    {
        
        if (PlayerState.Is(PlayerState.State.Pause))
            return;

        if (dead)
            return;
        
        if (t >= lifeTime)
        {
            QueueFree();
            Enemies.enemiesCount--;
        }
        
        if(!verifground)
        {
            if (IsOnFloor())
            {
                verifground = true;
            }
            vel.x = 0;
            if (!verifdie)
                vel.y = 300;
            else
                vel.y = 10;
            MoveAndSlide(vel, up);
        }
        if (verifground)
        {
            this.delta = delta;
            if (!verifattack)
            {
                if (!Player_Near())
                {
                    if (!veriftime)
                    {
                        time.Start();
                        veriftime = true;
                    }
                    t += delta;
                    Random_moves();
                }
                else
                {
                    t = 0;
                    time.Stop();
                    veriftime = false;
                    Get_To_Player();
                }
            }
            else
            {
                Position_Player = new Vector2(PlayerMouvements.GetX(), PlayerMouvements.GetY());
                Position_Player = Convertion.World2Location(Position_Player);

                Vector2 direction = (-this.GlobalPosition +Position_Player).Normalized();
                vel = direction * (-Speed/5);
                MoveAndSlide(vel, up);
            }
        }
        
        
        
        Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
        int viewportSizeX = Mathf.FloorToInt(GetViewport().Size.x * CurrentCamera.GetXZoom());
        Vector2 vecMin = Convertion.Location2World(p) * -1;
        Vector2 vecMax = Convertion.Location2World(new Vector2(p.x*-1+viewportSizeX, p.y));
        if (vecMin.x < 0)
        {
            if (!mirrored)
            {
                int i = (int) Mathf.Abs(vecMin.x / Chunk.size) + 1;
                if (Convertion.Location2World(Position).x >= (World.size - i) * Chunk.size)
                {
                    Position = Position - new Vector2(World.size * Chunk.size * World.BlockTilemap.CellSize.x, 0);
                    mirrored = true;
                }
            }else if (-vecMin.x+prev_x_viewport >= 0.90f * World.size * Chunk.size)
            {
                int i = (int) Mathf.Abs(vecMin.x / Chunk.size) + 1;
                if (Convertion.Location2World(Position).x >= (World.size - i) * Chunk.size)
                {
                    Position = Position - new Vector2(World.size * Chunk.size * World.BlockTilemap.CellSize.x, 0);
                    mirrored = false;
                }
            }
        }
        else if (vecMax.x >= World.size*Chunk.size)
        {
            if (!mirrored)
            {
                int i = (int) Mathf.Abs((vecMax.x - World.size * Chunk.size) / Chunk.size) + 1;
                if (Convertion.Location2World(Position).x <= i * Chunk.size)
                {
                    Position = Position + new Vector2(World.size * Chunk.size * World.BlockTilemap.CellSize.x, 0);
                    mirrored = true;
                }
            } else if (vecMin.x-prev_x_viewport >= 0.90f * World.size * Chunk.size)
            {
                int i = (int) Mathf.Abs((vecMax.x - World.size * Chunk.size) / Chunk.size) + 1;
                if (Convertion.Location2World(Position).x <= i * Chunk.size)
                {
                    Position = Position + new Vector2(World.size * Chunk.size * World.BlockTilemap.CellSize.x, 0);
                    mirrored = false;
                }
            }
        }
        else if (vecMax.x < World.size*Chunk.size && vecMin.x >= 0)
        {
            if (mirrored)
            {
                if (Convertion.Location2World(Position).x < 0)
                {
                    Position = Position + new Vector2(World.size * Chunk.size * World.BlockTilemap.CellSize.x, 0);
                }
                else
                {
                    Position = Position - new Vector2(World.size * Chunk.size * World.BlockTilemap.CellSize.x, 0);
                }

                mirrored = false;
            }
        }
        prev_x_viewport = vecMin.x;
    }


    public void _on_AnimationEnemy_animation_finished(string name)
    {
        if (name == "Die")
        {
            QueueFree();
            Enemies.enemiesCount--;
        }
    }
}
