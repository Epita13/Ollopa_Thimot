using Godot;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class Notifications : VBoxContainer
{

    private static float time = 2.5f;
    private static float s;
    
    private static Notifications instance;
    public static Notifications GetInstance() => instance;
    
    private static Queue<(string, float)> notifications = new Queue<(string, float)>();
    
    private List<Label> textes = new List<Label>();


    public static void Add(String texte)
    {
        if (notifications.Count == 4)
        {
            notifications.Dequeue();
        }
        notifications.Enqueue((texte, s));
    }
    
    
    public override void _Ready()
    {
        s = 0;
        textes.Add(GetNode<Label>("0"));
        textes.Add(GetNode<Label>("1"));
        textes.Add(GetNode<Label>("2"));
        textes.Add(GetNode<Label>("3"));
        
        
    }

    public override void _Process(float delta)
    {
        s += delta;
        if (s >= float.MaxValue - 1.0f)
        {
            s = 0;
        }

        /*int i = 0;
        while (i < textes.Count)
        {
            if ()
            var o = notifications.;
            textes[i].Text = o.Item1;
            i++;
        }*/

    }
}
