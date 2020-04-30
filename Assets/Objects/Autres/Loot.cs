using Godot;
using System;
using System.Net.Sockets;

public class Loot : Node2D
{

    /*
     Object static:  World

     /!\ Initialisation static : STRICTEMENT NECESSAIRE.
         - Utiliser la fonction Init()
         - Verification d'initialisation : le getter IsInit

      /!\ Classe Initialisées necessaire : World
      
     Description de l'object :
        A faire

     Description des parametres:
         A faire
             
 */
    private static Node parent;
    
    private static bool isInit = false;
    public static bool IsInit => isInit;
    public static void IsInitLootTest(string funcName)
    {
        if (!isInit)
            throw new UninitializedException(funcName, "Loot");
    }

    public static void Init(Node parent)
    {
        Loot.parent = parent;
        isInit = true;
    }

    public static void SpawnLoot(Vector2 loc, Item.Type type, int amount)
    {
        IsInitLootTest("SpawnLoot");
        Vector2 nloc = Convertion.World2Location(loc);
        Loot l = (Loot)GD.Load<PackedScene>("res://Assets/Objects/Autres/Loot.tscn").Instance();
        l.setLoot(type, amount);
        l.Position = nloc;
        parent.AddChild(l);
    }
    
    
    public const float SPEEDMIN = 1.4f;
    public const float SPEEDMAX = 2.0f;
    public const float LIFETIME = 30.0f;
    public const int STACKSIZE = 48;

    private float time;
    public bool dead = false;
    
    public float speed = SPEEDMAX;
    public float dephase;

    
    /*Teleportation variables*/
    private bool mirrored = false;
    private float prev_x_viewport;

    private bool hasLoot = false;
    private Item.Type type;
    private int amount;
    public void setLoot(Item.Type type, int amount)
    {
        this.type = type;
        this.amount = amount;
        this.hasLoot = true;
    }

    public void GiveLoot(Loot l)
    {
        int r = STACKSIZE - l.GetLootAmount();
        int amount = 0;
        if (GetLootAmount() <= r)
        {
            amount = GetLootAmount();
        }
        else
        {
            amount = r;
        }
        this.amount -= amount;
        l.AddLoot(amount);
        l.ResetTime();
        
    }

    public void AddLoot(int amount)
    {
        this.amount += amount;
    }

    public Item.Type GetLootType() => type;
    public int GetLootAmount() => amount;

    public void ResetTime() => time = 0;

    private bool HasLoot() => hasLoot;
    
    
    private Area2D area;
    public override void _Ready()
    {
        Random r = new Random();
        speed = ((float)r.NextDouble() * (SPEEDMAX-SPEEDMIN)) + SPEEDMIN;
        dephase = (float)r.NextDouble()*2.0f;
        
        if (!hasLoot)
            QueueFree();
        area = GetNode<Area2D>("ZONE");
        time = 0;
        
        Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
        Vector2 vecMin = Convertion.Location2World(p) * -1;
        prev_x_viewport = vecMin.x;
        
        World.IsInitWorldTest("Loot constructor");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (dead)
            return;
        time += delta;
        if (time >= LIFETIME)
        {
            Explosion();
            dead = true;
            Delay.StartDelay(this, 0.3f, () => QueueFree());
        }
        
        
        /*Teleportation loots*/
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
                    GD.Print("--");
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
        /*----------------------*/
        
        var bodies = area.GetOverlappingBodies();
        Node2D Player = null;
        foreach (var body in bodies)
        {
            if (((Node2D)body).GetGroups().Contains("Player"))
            {
                Player = (Node2D) body;
            }
        }
        Node2D NearestLoot = null;
        var areas = area.GetOverlappingAreas();
        foreach (var a in areas)
        {
            Node2D n = (Node2D) a;
            if (n.GetGroups().Contains("loot") && n.GetNode<Loot>("..")!=this)
            {
                Loot l = n.GetNode<Loot>("..");
                if (l.GetLootType()==GetLootType() && GetLootAmount()<STACKSIZE && l.GetLootAmount() < STACKSIZE)
                {
                    NearestLoot = n.GetNode<Loot>("..");
                    break;
                }
            }
        }

        if (Player != null)
        {
            Vector2 vec = Player.Position - Position;
            Position = Position + (vec.Normalized() * speed);
        }
        else if (NearestLoot != null)
        {
            Vector2 vec = NearestLoot.Position - Position;
            Position = Position + (vec.Normalized() * 4.0f);
        }
        else
        {
            Position = new Vector2(Position.x, Position.y + Mathf.Sin((float)OS.GetTicksMsec()*0.004f+dephase)*0.2f);
        }

    }

    public void _on_BALL_area_shape_entered(int are_id, Area2D area, int a, int b)
    {
        if (area.GetGroups().Contains("loot"))
        {
            Loot l = area.GetNode<Loot>("..");
            if (l.dead==false && l.GetLootType() == GetLootType() && l.GetLootAmount()<STACKSIZE && GetLootAmount()<STACKSIZE)
            {
                dead = true;
                GiveLoot(l);
                if (GetLootAmount() <= 0)
                {
                    QueueFree();
                }
            }
        }
    }

    public void Explosion()
    {
        GetNode<Particles2D>("Explosion").Emitting = true;
    }
}
