using Godot;
using System;
using System.Collections.Generic;

public class Tree : StaticBody2D
{

    /*
        Object :  Tree

        /!\ Initialisation static : STRICTEMENT NECESSAIRE.
         - Utiliser la fonction Init()
         - Verification d'initialisation : le getter IsInit
         
        /!\ Classe Initialisées necessaire : World

        Description de l'object :
            

        Description des parametres:
            
    */
    
    private static List<(Texture, Texture)> trees_textures = new List<(Texture, Texture)>
    {
        (GD.Load<Texture>("res://Assets/Ressources/Imgs/Environement/Tree/treebot.png"), 
            GD.Load<Texture>("res://Assets/Ressources/Imgs/Environement/Tree/treetop.png")),
        (GD.Load<Texture>("res://Assets/Ressources/Imgs/Environement/Tree/treebot2.png"), 
            GD.Load<Texture>("res://Assets/Ressources/Imgs/Environement/Tree/treetop2.png"))
    };
    

    /*public static bool HasTree(float x, float y)
    {
        foreach (var tree in trees)
        {
            tree.GetNode<CollisionPolygon2D>("CollisionPolygon2D").
        }   
    }*/
    
    

    
    
    
    private static Node parent;
    private static Random random;
    
    private static bool isInit = false;
    public static bool IsInit => isInit;
    public static void IsInitTreeTest(string funcName)
    {
        if (!isInit)
            throw new UninitializedException(funcName, "Tree");
    }

    public static void Init(Node parent)
    {
        Tree.parent = parent;
        isInit = true;
        random = new Random();
    }

    public static void SpawnTree(Vector2 loc)
    {
        Tree t = (Tree)GD.Load<PackedScene>("res://Assets/Objects/Autres/Tree/Tree.tscn").Instance();
        World.trees.Add(t);

        t.treeNumber = World.random.Next(trees_textures.Count);
        t.treeSize = (float) World.random.NextDouble() * 0.25f + 0.1f;
        
        t.Place((int)loc.x, (int)loc.y);
    }
    
    
    /*Structure de sauvegarde*/
    public struct SaveStruct
    {
        public Vector2 location;
        public int treeNumber;
        public float treeSize;
    }

    public SaveStruct GetSaveStruct()
    {
        SaveStruct s = new SaveStruct();
        s.location = location;
        s.treeNumber = treeNumber;
        s.treeSize = treeSize;
        return s;
    }
    /*************************/
    
    
    private float health;
    private Drop drop;
    public Vector2 location;

    public int treeNumber;
    public float treeSize;

    private AnimationPlayer ap;

    private bool dead = false;
    
    private bool mirrored;
    private float prev_x_viewport;

    public override void _EnterTree()
    {
        World.IsInitWorldTest("Tree constructor");
        IsInitTreeTest("Tree constructor");
        Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
        Vector2 vecMin = Convertion.Location2World(p) * -1;
        prev_x_viewport = vecMin.x;
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
        ap.CurrentAnimation = "";
        
        GetNode<Sprite>("bot").Texture = trees_textures[treeNumber].Item1;
        GetNode<Sprite>("top").Texture = trees_textures[treeNumber].Item2;

        Scale = new Vector2(treeSize, treeSize);
        health = treeSize * 750 / 0.3f;
        drop = new Drop(new Drop.Loot(Item.Type.Wood, Mathf.CeilToInt(treeSize*4/0.3f)));
    }


    public void Place(int x, int y)
    {
        IsInitTreeTest("Tree/Place");
        var vec = Convertion.World2Location(new Vector2(x+0.5f, y));
        Position = vec;
        location = new Vector2(x,y);
        parent.AddChild(this);
    }

    public void Damage(float damage)
    {
        health -= damage;
        ap.CurrentAnimation = "BREAKING";
        if (health <= 0)
        {
            dead = true;
            Destroy(true);
        }
    }

    private void Destroy(bool withLoot)
    {
        World.IsInitWorldTest("Tree/Destroy");
        GetNode<CollisionPolygon2D>("CollisionPolygon2D").Disabled = true;
        World.trees.Remove(this);
        if (withLoot)
        {
            var pos = Convertion.Location2World(Position);
            foreach (var l in drop.loots)
            {
                Vector2 npos = new Vector2(pos.x + ((float) World.random.NextDouble() * 0.8f - 0.4f),
                    pos.y + ((float) World.random.NextDouble() * 0.8f - 0.4f));
                GD.Print(npos);
                Loot.SpawnLoot(npos, l.type, l.amount);
            }
        }

        int a = random.Next(2);
        if(a==0)
            ap.CurrentAnimation = "DEATH";
        else
            ap.CurrentAnimation = "DEATH2";
    }

    public void _on_AnimationPlayer_animation_finished(string anim_name)
    {
        if (anim_name == "DEATH" || anim_name == "DEATH2")
        {
            QueueFree();
        }
    }
    
    public override void _Process(float delta)
    {
        /*Teleportation Tree*/
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
        /*----------------------*/
        
        if (dead)
            return;
        
        /*Verification du sol*/
        if (World.GetBlock((int) location.x, (int) location.y - 1).GetType == Block.Type.Air)
        {
            dead = true;
            Destroy(false);
        }
    }
}
