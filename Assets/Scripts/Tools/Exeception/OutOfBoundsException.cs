using Godot;
using System;

public class OutOfBoundsException1D : ArgumentException
{
    public OutOfBoundsException1D(string funcName, int value, int minValue, int maxValue) 
    : base( funcName + ": value1="+value+" is out of bounds. it must be in ["+minValue+", "+maxValue+"]" )
    {}
}

public class OutOfBoundsException2D : ArgumentException
{
    public OutOfBoundsException2D(string funcName, int value1, int minValue1, int maxValue1, int value2, int minValue2, int maxValue2)
     : base( funcName + ": value1="+value1+", value2="+value2+" are out of bounds. they must be in value1=["+minValue1+", "+maxValue1+"], value2=["+minValue2+", "+maxValue2+"]" )
    {}
}
