using Godot;
using System;

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

    public enum State
    {
        Normal,
        Build,
        Menu,
        Dead
    }

    /// Retourne l'etat du joueur 
    public static State GetState()
    {
        return state;
    }

    /// Change l'etat du joueur
    public static void SetState(State st)
    {
        state = st;
    }

}
