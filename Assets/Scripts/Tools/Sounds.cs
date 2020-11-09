using Godot;
using System;
using System.Collections.Generic;

public static class Sounds
{
    /*Player sounds*/
    public enum Type
    {
        PlayerDeath,
        PlayerGetloot,
        PlayerLaser,
        BlockBreak,
        PlayerHurt,
        PlayerStep,
        PlayerPlouf,
        PlayerLanding
    }
    public static Dictionary<Type, AudioStream> sounds = new Dictionary<Type, AudioStream>
    {
        {Type.PlayerDeath, GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Player/player_death.wav")},
        {Type.PlayerGetloot, GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Player/player_getloot.wav")},
        {Type.PlayerLaser, GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Player/player_laser.wav")},
        {Type.BlockBreak, GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Blocks/BlockBreak.wav")},
        {Type.PlayerHurt, GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Player/player_hurt.wav")},
        {Type.PlayerStep,GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Blocks/BlockBreak.wav")},
        {Type.PlayerPlouf,GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Player/player_plouf.wav")},
        {Type.PlayerLanding, GD.Load<AudioStream>("res://Assets/Ressources/Sounds/Blocks/BlockBreak.wav")},
    };
    
    public static Dictionary<Type, float> soundAjust = new Dictionary<Type, float>
    {
        {Type.PlayerDeath, -15},
        {Type.PlayerGetloot, -25},
        {Type.PlayerLaser, -15},
        {Type.BlockBreak, -20},
        {Type.PlayerHurt, -15},
        {Type.PlayerStep, -37},
        {Type.PlayerPlouf, -20},
        {Type.PlayerLanding, -28}
    };
}
