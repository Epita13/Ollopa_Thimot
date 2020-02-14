using Godot;
using System;

public class UninitializedException : NullReferenceException
{
    public UninitializedException(string funcName, string className) 
    : base(funcName+": The class "+className+" must be initialize before.")
    {}
}
