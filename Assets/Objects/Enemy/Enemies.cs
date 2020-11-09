using Godot;
using System;
using System.Collections;

public class Enemies : Node2D
{
    private static PackedScene enemy_fly = GD.Load<PackedScene>("res://Assets/Objects/Enemy/Enemy_Fly.tscn");
    
    Timer spawn_time = new Timer();

    public static Enemies instance;
    public static Vector2 positionPlayer = new Vector2();

    public static int enemiesCount = 0;
    public static int MaxEnemies = 5;
    
    private bool watchdie = false;
    private static Random rand =new Random();
    private static int r;
    
    public override void _Ready()
    {
        spawn_time = GetNode<Timer>("SpawnTime");
        spawn_time.Start();
        instance = this;
    }

    void TimerTimeout()
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        if (enemiesCount >= MaxEnemies)
            return;
        if(Environement.cycle==Environement.TimeState.NIGHT)
            Spawn_Enemy_Randomly();
    }
    
    public static void Spawn_Enemy_Randomly()
    {
        r = rand.Next(0, 2);
        positionPlayer = new Vector2(PlayerMouvements.GetX(), PlayerMouvements.GetY());
        positionPlayer = Convertion.World2Location(positionPlayer);
        if(r==1) 
            Spawn_Enemy(PlayerMouvements.GetX()+10, PlayerMouvements.GetY()+50);
        else
            Spawn_Enemy(PlayerMouvements.GetX()-10, PlayerMouvements.GetY()+50);
    }
    
    public static void Spawn_Enemy(float x, float y)
    {
        Ennemy_Fly enemy1 = (Ennemy_Fly)enemy_fly.Instance();
        instance.AddChild(enemy1);
        enemy1.Position = Convertion.World2Location(new Vector2(x,y));
    }
}
