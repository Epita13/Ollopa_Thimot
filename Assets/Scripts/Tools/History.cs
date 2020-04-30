using Godot;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

public static class History<T>
{
    //public const float time = 0.1f;
    public const int valuesCount = 10000;

    public static void Add(List<T> liste, T value)
    {
        liste.Add(value);
        if (liste.Count > valuesCount)
        {
            liste.RemoveAt(0);
        }
    }
    
}
