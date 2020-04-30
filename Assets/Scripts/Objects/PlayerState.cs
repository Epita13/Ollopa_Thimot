using Godot;
using System;
using System.Runtime.CompilerServices;

public static class PlayerState
{

    /*
        Classe static contenant l'etat du joueur :
            - Normal : Mode de base dans lequel le joueur peux utiliser les outils et les blocks
            - Build : Mode dans lequel le joueur peut poser un batiment
            - Menu: Mode dans lequel le joueur est dans un quelconque Menu (inventaire,..)
            - Dead: Mode qui intervient a la mort du joueur.
    */


    public static State state = State.Normal;
    public static State prec_state = State.Normal;

    public enum State
    {
        Normal,
        Build,
        Inventory,
        Dead,
        BuildingInterface,
        Link
    }

    /// Retourne l'etat du joueur 
    public static State GetState()
    {
        return state;
    }

    /// Change l'etat du joueur
    public static void SetState(State st)
    {
        if (st == State.Build || st == State.Normal || st == State.Link)
            prec_state = st;
        state = st;
    }

    public static bool Is(params State[] states)
    {
        bool res = false;
        foreach (var state in states)
        {
            if (GetState() == state)
            {
                res = true;
            }
        }
        return res;
    }
    
    public static bool IsNot(params State[] states)
    {
        bool res = true;
        foreach (var state in states)
        {
            if (GetState() == state)
            {
                res = false;
            }
        }
        return res;
    }
}
