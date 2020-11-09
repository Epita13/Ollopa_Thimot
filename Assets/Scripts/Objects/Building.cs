using Godot;
using System;
using System.Collections.Generic;

public abstract class Building : Node2D
{
    
    // Enumeration : Type de batiment disponible
    public static int nbBuildings = 11;

    public enum Type
    {
        SolarPanel,
        O2Generator,
        Thermogenerator,
		Storage,
        Grinder,
        Printer3D,
        Compactor,
        Infirmary,
        OilPump,
        Refinery,
        Drill,
    }
    
    // Dictionaire : Stock les scnenes batiment en fonction du type de batiment
    public static Dictionary<Type, PackedScene> prefabs = new Dictionary<Type, PackedScene>
    {
        {Type.SolarPanel, GD.Load<PackedScene>("res://Assets/Objects/Buildings/SolarPanel/SolarPanel.tscn")},
		{Type.Storage, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Storage/Storage.tscn")},
		{Type.Printer3D, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Printer3D/Printer3D.tscn")},
		{Type.Compactor, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Compactor/Compactor.tscn")},
        {Type.Infirmary, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Infirmary/Infirmary.tscn")},
        {Type.O2Generator, GD.Load<PackedScene>("res://Assets/Objects/Buildings/O2Generator/O2Generator.tscn")},
        {Type.OilPump, GD.Load<PackedScene>("res://Assets/Objects/Buildings/PetrolGenerator/PetrolGenerator.tscn")},
        {Type.Refinery, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Refinery/Refinery.tscn")},
        {Type.Drill, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Drill/Drill.tscn")},
        {Type.Grinder, GD.Load<PackedScene>("res://Assets/Objects/Buildings/Grinder/Grinder.tscn")},
        {Type.Thermogenerator, GD.Load<PackedScene>("res://Assets/Objects/Buildings/thermogenerator/thermogenerator.tscn")},
    };
    public static Dictionary<Type, Texture> textures = new Dictionary<Type, Texture>
    {
        {Type.SolarPanel, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/SolarPanel/SolarPanel.png")},
        {Type.Storage, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Storage/Stockage.png")},
        {Type.Printer3D, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Printer3D/Printer3D.png")},
        {Type.Compactor, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Compactor/Compactor.png")},
        {Type.Infirmary, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Infirmary/Infirmary.png")},
        {Type.O2Generator, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/O2Generator/O2Generator.png")},
        {Type.OilPump, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Oilwell/PetrolGenerator.png")},
        {Type.Refinery, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Refinery/Refinery.png")},
        {Type.Drill, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Drill/forreuse.png")},
        {Type.Grinder, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Broyeur/Broyeur.png")},
        {Type.Thermogenerator, GD.Load<Texture>("res://Assets/Ressources/Imgs/Buildings/Thermogenerator/generateur thermique.png")}
    };
    
    public static Dictionary<Type, string> descriptions = new Dictionary<Type, string>
    {
        {Type.SolarPanel, "Generates energy from the sun's energy"},
        {Type.Storage, "Stores energy and charges the player's energy"},
        {Type.Printer3D, "Creates buildings"},
        {Type.Compactor, "Creates blocks"},
        {Type.Infirmary, "Heals the player"},
        {Type.O2Generator, "Gives oxygene"},
        {Type.OilPump, "Gives Oil"},
        {Type.Refinery, "Transforms oil in fuel"},
        {Type.Drill, "Extracts item from the ground"},
        {Type.Grinder, "Grinds every item into composite"},
        {Type.Thermogenerator, "Generate energy with wood or fuel"},
    };
    
    public static Dictionary<Type, float> times2Create = new Dictionary<Type, float>
    {
        {Type.SolarPanel, 60.0f},
        {Type.Storage, 120.0f},
        {Type.Printer3D, 300.0f},
        {Type.Compactor, 150.0f},
        {Type.Infirmary, 200f},
        {Type.O2Generator, 45f},
        {Type.OilPump, 60f},
        {Type.Refinery, 60f},
        {Type.Drill, 45f},
        {Type.Grinder, 150f},
        {Type.Thermogenerator, 60f},
    };
    
    public static Dictionary<Type, Drop> crafts = new Dictionary<Type, Drop>
    {
        {Type.SolarPanel, new Drop(new Drop.Loot(Item.Type.Sonar, 12), new Drop.Loot(Item.Type.Stone, 10), new Drop.Loot(Item.Type.Wood, 4))},
        {Type.Storage, new Drop(new Drop.Loot(Item.Type.Stone, 30), new Drop.Loot(Item.Type.Wood, 5))},
        {Type.Printer3D, new Drop(new Drop.Loot(Item.Type.Composite, 40), new Drop.Loot(Item.Type.Ospirit,1))},
        {Type.Compactor, new Drop( new Drop.Loot(Item.Type.Sonar,5), new Drop.Loot(Item.Type.Stone, 15), new Drop.Loot(Item.Type.Ospirit, 3))},
        {Type.Infirmary, new Drop(new Drop.Loot(Item.Type.Stone, 15), new Drop.Loot(Item.Type.Ospirit, 2), new Drop.Loot(Item.Type.Sonar,7), new Drop.Loot(Item.Type.Composite,10))},
        {Type.O2Generator, new Drop( new Drop.Loot(Item.Type.Wood,5),new Drop.Loot(Item.Type.Stone,15))},
        {Type.OilPump, new Drop(new Drop.Loot(Item.Type.Composite, 10), new Drop.Loot(Item.Type.Stone, 35), new Drop.Loot(Item.Type.Ospirit,2))},
        {Type.Refinery, new Drop(new Drop.Loot(Item.Type.Composite, 25), new Drop.Loot(Item.Type.Stone,40), new Drop.Loot(Item.Type.Ospirit,4))},
        {Type.Drill, new Drop(new Drop.Loot(Item.Type.Composite, 30),new Drop.Loot(Item.Type.Sonar, 10),new Drop.Loot(Item.Type.Ospirit, 5))},
        {Type.Grinder, new Drop(new Drop.Loot(Item.Type.Sonar, 1),new Drop.Loot(Item.Type.Dirt, 30),new Drop.Loot(Item.Type.Stone, 15))},
        {Type.Thermogenerator, new Drop(new Drop.Loot(Item.Type.Stone, 15), new Drop.Loot(Item.Type.Sonar,2))}
    };
    
    public static List<Building.Type> buildingReceiverOfEnergy = new List<Type>
    {
        Type.Storage, 
        Type.Printer3D, 
        Type.Compactor, 
        Type.Infirmary, 
        Type.O2Generator,
        Type.OilPump,
        Type.Refinery,
        Type.Drill,
        Type.Grinder
    };


    public static float powerEnergy2Player = 5;
    public static List<Building.Type> buildingGiveEnergy2Player = new List<Type>
    {
        Type.Storage,
    };

    /*
        Object abstract:  Building

        /!\ Initialisation static : STRICTEMENT NECESSAIRE.
            - Utiliser la fonction Init()
            - Verification d'initialisation : le getter IsInit

        /!\ Classe Initialisées necessaire : None

        Description de l'object :
            Un batiment (Building) est un object placable dans une scene.
            Il possede de la vie et peut ainsi en perdre. 
            La classe Building est une definition generale d'un batiment et a donc d'autre classes qui herite de celui-ci.
            Les coordonnées manipulées dans cette classe sont strictement celle de Godot.

        Description des parametres:
            (static) Node parent : est la node dans laquelle les buildings vont etre instancier en tant qu'enfant de cette node.
            (static) int zIndex : est la profondeur 2D a laquelle les buildings vont etre instancier.
            (static) bool isInit : True si Building est initialisé, false sinon.
            Vector2 location : est la position du batiment dans la scene, si le batiment n'est pas placer alors : (-1,-1). 
            bool isPlaced : true si le batiment est placer dans la scene; false sinon.
            int health, maxGHealth : represente la vie du joueur et son maximum de vie.
    */

    public static List<T> GetBuildingTypeList<T> () where T: Building
    {
        List<T> l = new List<T>();
        foreach (var building in World.placedBuildings)
        {
            if (building is T)
                l.Add((T)building);
        }
        return l;
    }
    public static List<Building> GetBuildingTypeList (Type type)
    {
        List<Building> l = new List<Building>();
        foreach (var building in World.placedBuildings)
        {
            if (building.type == type)
                l.Add(building);
        }
        return l;
    }
    public static T GetBuildingById<T>(int id) where T: Building
    {
        List<T> l = Building.GetBuildingTypeList<T>();
        T s = null;
        foreach (var building in l)
        {
            if (building.id == id)
            {
                s = building;
                break;
            }
        }
        return s;
    }
    public static Building GetBuildingById(Type type, int id)
    {
        foreach (var b in World.placedBuildings)
        {
            if (b.type == type && b.id == id)
            {
                return b;
            }
        }
        return null;
    }
    
    public static Node parent;

    private static bool isInit = false;
    public static bool IsInit => isInit;
    public static void IsInitBuildingTest(string funcName)
    {
        if (!isInit)
            throw new UninitializedException(funcName, "Building");
    } 
    /// Initialise les variables pour le fonctionnement des batiments (OBLIGATOIRE)
    public static void Init(Node parent)
    {
        isInit = true;
        Building.parent = parent;
    }
    
    
    
    /*Selection de batiment*/
    public static bool HasBuildingSelected = false;
    public static Building BuildingSelected = null;

    private bool mouseOn = false;
    
    
    public int size = 4;
    public Vector2[] corners = new Vector2[4];
    

    public Vector2 location;
    public Vector2 locationNow;
    public bool isPlaced = false;

    public Building.Type type;

    public int healthMax = 100;
    public int health;

    private bool mirrored = false;
    private float prev_x_viewport;


    public int id;
    
    public float energyMax = 200.0f;
    public float energy = 0;
    /*Variable de donnees graphiques*/
    public List<float> energyhistory = new List<float>();
    public float powerIn;
    private float sumEnergyIn;
    public List<float> powerInhistory = new List<float>();
    public float powerOut;
    private float sumEnergyOut;
    public List<float> powerOuthistory = new List<float>();
    
    public const float POWERMAXOUT = 1.5f; // e/s

    public Timer timer;
    
    /*Links*/
    public bool isLinked = false;
    public List<Building> linkedBuildings = new List<Building>();

    
    /*Structure de sauvegarde*/
    public struct SaveStruct
    {
        public Type type;
        public int healthMax;
        public int health;
        public int id;
        public float energyMax;
        public float energy;
        public Vector2 location;
        public bool isLinked;
        public List<float> energyhistory;
        public List<float> powerInhistory;
        public List<float> powerOuthistory;
    }

    public SaveStruct GetBuildingSaveStruct()
    {
        SaveStruct s = new SaveStruct();
        s.type = type;
        s.healthMax = healthMax;
        s.health = health;
        s.id = id;
        s.energy = energy;
        s.energyMax = energyMax;
        s.location = location;
        s.isLinked = isLinked;
        s.energyhistory = energyhistory;
        s.powerInhistory = powerInhistory;
        s.powerOuthistory = powerOuthistory;
        return s;
    }
    public void SetBuildingSaveStruct(SaveStruct st)
    {
        type = st.type;
        healthMax = st.healthMax;
        health = st.health;
        id = st.id;
        energy = st.energy;
        energyMax = st.energyMax;
        isLinked = st.isLinked;
        energyhistory = st.energyhistory;
        powerInhistory = st.powerInhistory;
        powerOuthistory = st.powerOuthistory;
    }
    /*************************/
    
    
    public Building(int healthMax, float energyMax)
    {
        this.health = healthMax;
        this.healthMax = healthMax;
        this.energyMax = energyMax;
    }

    public void SetType(Building.Type type)
    {
        this.type = type;
    }

    /// Place le batiment sur la map
    public void Place(Vector2 location)
    {
        IsInitBuildingTest("Place");
        if (isPlaced)
            return;
        this.location = Convertion.World2WorldBorn(location);
        corners = SetCorners(this.location);
        isPlaced = true;
        Position = Convertion.World2Location(location);
        parent.AddChild(this);
        World.placedBuildings.Add(this);
        World.placedBuildingByChunk[World.GetChunk((int)location.x)].Add(this);
    }

    /// Enleve le batiment de la map
    public void Remove()
    {
        IsInitBuildingTest("Remove");
        if (!isPlaced)
            return;
        this.location = new Vector2(-1,-1);
        isPlaced = false;
        parent.RemoveChild(this);
        World.placedBuildings.Remove(this);
        World.placedBuildingByChunk[World.GetChunk((int) location.x)].Remove(this);
    }

    // Détruit le batmiment
    public void Destroy()
    {
        Remove();
        QueueFree();
    }

    /// Donne des dégats à la structure
    public void Damage(int value)
    {
        health -= value;
        if (health<0)
            health = 0;
    }

    /// Donne de la vie a la structure
    public void Healing(int value)
    {
        health += value;
        if (health>healthMax)
            health = healthMax;
    }
    
    private Vector2[] SetCorners(Vector2 location)
    {
        Vector2[] l = new Vector2[4]
        {
            new Vector2(location.x - size / 2, location.y - size / 2),
            new Vector2(location.x - size / 2, location.y + size / 2),
            new Vector2(location.x + size / 2, location.y + size / 2),
            new Vector2(location.x + size / 2, location.y - size / 2)
        };
        return l;
    }

    public bool IsInBuilding(float x, float y)
    {
        bool res = false;
        if (x >= corners[0].x && x < corners[2].x)
        {
            if (y < corners[1].y && y >= corners[3].y)
            {
                res = true;
            }
        }
        return res;
    }


    public override void _Ready()
    {
        Vector2 p = GetViewportTransform().origin * CurrentCamera.GetXZoom();
        Vector2 vecMin = Convertion.Location2World(p) * -1;
        prev_x_viewport = vecMin.x;
        GetNode<Sprite>("OUTLINE").Material = (Material)GetNode<Sprite>("OUTLINE").Material.Duplicate();

        GetNode<Timer>("TimerEnergy").WaitTime = 1.0f;
        timerEnergyWaitTime = GetNode<Timer>("TimerEnergy").WaitTime;
        
        timer = GetNode<Timer>("Timer");
    }

    public override void _Process(float delta)
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        
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
        locationNow = Convertion.Location2World(Position);

        if (HasBuildingSelected && BuildingSelected==this)
        {
            if (!BuildingInRange(10))
            {
                ResetOutline();
                HasBuildingSelected = false;
                BuildingSelected = null;
            }
        }

        if (mouseOn)
        {
            _on_ZONE_mouse_entered();
        }
    }
    
    public float AddEnergy(float amount, bool correction = false)
    {
        energy += amount;
        float reste = 0;
        if (energy > energyMax)
        {
            reste = energy - energyMax;
            energy = energyMax;
        }

        if (correction)
        {
            sumEnergyOut -= amount - reste;
        }
        else
        {
            sumEnergyIn += amount - reste;
        }
        return reste;
    }

    public float RemoveEnergy(float amount, bool correction = false)
    {
        energy -= amount;
        float reste = 0;
        if (energy < 0)
        {
            reste = Mathf.Abs(energy);
            energy = 0;
        }

        if (correction)
        {
            sumEnergyIn -= amount - reste;
        }
        else
        {
            sumEnergyOut += amount - reste;
        }
        return amount - reste;
    }

    public void TransferToLink(float delta)
    {
        if (isLinked && energy!=0)
        {
            float power = GetPowerOut(delta);
            float energyByBuildings = power * delta / linkedBuildings.Count;
            float energyNotAdd = 0;
            foreach (var linkedBuilding in linkedBuildings)
            {
                float energyRemove = RemoveEnergy(energyByBuildings);
                energyNotAdd = linkedBuilding.AddEnergy(energyNotAdd);
                energyNotAdd += linkedBuilding.AddEnergy(energyRemove);
            }
            //sumEnergyOut +=  (energyByBuildings * linkedBuildings.Count) - energyNotAdd;
            AddEnergy(energyNotAdd, true);
        }
    }
    
    private float GetPowerOut(float delta)
    {
        float res = POWERMAXOUT;
        if (POWERMAXOUT * delta > energy)
        {
            res = energy / delta;
        }
        return res;
    }
    
    public void Link(List<Building> buildings)
    {
        isLinked = buildings.Count > 0;
        linkedBuildings = buildings;
    }
    
    /*History for graphs*/
    public void _on_TimerHistory_timeout()
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        History<float>.Add(energyhistory, energy);
        History<float>.Add(powerInhistory, powerIn);
        History<float>.Add(powerOuthistory, powerOut);
    }


    private float timerEnergyWaitTime;
    public void _on_TimerEnergy_timeout()
    {
        if (PlayerState.Is(PlayerState.State.Pause))
            return;
        powerIn = sumEnergyIn / timerEnergyWaitTime;
        powerOut = sumEnergyOut / timerEnergyWaitTime;
        sumEnergyIn = 0;
        sumEnergyOut = 0;
    }

    public bool BuildingInRange(float range)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(PlayerMouvements.GetX() - location.x, 2) +
                                    Mathf.Pow(PlayerMouvements.GetY() - location.y, 2));
        return distance <= range;
    }
    /*OUTLINE buildings*/
    public void _on_ZONE_mouse_entered()
    {
        mouseOn = true;
        Sprite p = GetNode<Sprite>("OUTLINE");
        if (PlayerState.Is(PlayerState.State.Normal, PlayerState.State.Build))
        {
            if (BuildingInRange(10))
            {
                SetOutline(1.5f, Color.Color8(0, 150, 255));
                HasBuildingSelected = true;
                BuildingSelected = this;
            }
        } else if (PlayerState.Is(PlayerState.State.Link))
        {
            if (Building.buildingReceiverOfEnergy.Contains(type))
            {
                SetOutline(1.5f, Color.Color8(0,255,0));
            }
            else
            {
                SetOutline(1.5f, Color.Color8(255,0,0));
            }
            HasBuildingSelected = true;
            BuildingSelected = this;
        }
    }
    public void _on_ZONE_mouse_exited()
    {
        mouseOn = false;
        if (HasBuildingSelected && BuildingSelected != this)
        {
            ResetOutline();
            return;
        }
        if (PlayerState.IsNot(PlayerState.State.Link))
        {
            ResetOutline();
            HasBuildingSelected = false;
            BuildingSelected = null;
        }
        else
        {
            if (this == global::Link.Building2BeLinked)
            {
                SetOutline(1.5f, Color.Color8(180,0,180));
            } else if (global::Link.BuildingTargets.Contains(this))
            {
                SetOutline(1.5f, Color.Color8(255, 255, 0));
            }
            else
            {
                ResetOutline();
            }
            HasBuildingSelected = false;
            BuildingSelected = null;
        }
    }


    public void ResetOutline()
    {
        GetNode<Sprite>("OUTLINE").Material.Set("shader_param/width", 0.0f);
    }

    public void SetOutline(float w, Color color)
    {
        Sprite p = GetNode<Sprite>("OUTLINE");
        p.Material.Set("shader_param/outline_color",color);
        p.Material.Set("shader_param/width", w);
    }

}
