using Godot;
using System;
using System.Collections.Generic;

public static class Link
{
    public static void Reset()
    {
        building2BeLinked = null;
        buildingTargets = new List<Building>();
        foreach (var building in World.placedBuildings)
        {
            building.ResetOutline();
        }
    }

    public static void Init(Building building2BeLinked)
    {
        SetBuilding2BeLinked(building2BeLinked);
        SetBuildingTarget(Building2BeLinked.linkedBuildings);
        foreach (var building in World.placedBuildings)
        {
            if (building == Link.building2BeLinked)
            {
                building.SetOutline(1.5f, Color.Color8(180,0,180));
            }else if (Link.buildingTargets.Contains(building))
            {
                building.SetOutline(1.5f, Color.Color8(255, 255, 0));
            }
        }
    }
    
    private static Building building2BeLinked = null;
    public static Building Building2BeLinked => building2BeLinked;
    public static void SetBuilding2BeLinked(Building b) => building2BeLinked = b;


    public static void AddLink(Building b)
    {
        if (!buildingTargets.Contains(b))
        {
            buildingTargets.Add(b);
        }
    }
    
    public static void RemoveLink(Building b)
    {
        if (buildingTargets.Contains(b))
        {
            buildingTargets.Remove(b);
        }
    }

    public static void AddOrRemoveLink(Building b)
    {
        if (buildingTargets.Contains(b))
        {
            RemoveLink(b);
        }
        else
        {
            AddLink(b);
        }
    }

    
    private static List<Building> buildingTargets = new List<Building>();
    public static List<Building> BuildingTargets => buildingTargets;
    public static void SetBuildingTarget(List<Building> bs) => buildingTargets = bs;

    public static void _Link()
    {
        building2BeLinked.Link(buildingTargets);
    }
}
