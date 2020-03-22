using Godot;
using System;

public class Loot : Node2D
{
    
    /*
     Object static:  World

     /!\ Initialisation static : STRICTEMENT NECESSAIRE.
         - Utiliser la fonction Init()
         - Verification d'initialisation : le getter IsInit

      /!\ Classe Initialisées necessaire : None
      
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

    private float time;
    public bool dead = false;
    
    public float speed = SPEEDMAX;
    public float dephase;

    private bool hasLoot = false;
    private Item.Type type;
    private int amount;
    public void setLoot(Item.Type type, int amount)
    {
        this.type = type;
        this.amount = amount;
        this.hasLoot = true;
    }

    public Item.Type GetLootType() => type;
    public int GetLootAmount() => amount;

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
        var bodies = area.GetOverlappingBodies();
        Node2D Player = null;
        foreach (var body in bodies)
        {
            if (((Node2D)body).GetGroups().Contains("Player"))
            {
                Player = (Node2D) body;
            }
        }

        if (Player != null)
        {
            Vector2 vec = Player.Position - Position;
            Position = Position + (vec.Normalized() * speed);
        }
        else
        {
            Position = new Vector2(Position.x, Position.y + Mathf.Sin((float)OS.GetTicksMsec()*0.004f+dephase)*0.2f);
        }

    }

    public void Explosion()
    {
        GetNode<Particles2D>("Explosion").Emitting = true;
    }
}
