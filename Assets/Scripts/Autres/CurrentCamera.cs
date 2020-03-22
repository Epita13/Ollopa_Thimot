using Godot;
using System;

public class CurrentCamera : Camera2D
{
    private static Camera2D current;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Current)
            current = this;
    }

      // Called every frame. 'delta' is the elapsed time since the previous frame.
      public override void _Process(float delta)
      {
          if (Current)
              current = this;
      }

      public static float GetXZoom()
      {
          return current.Zoom.x;
      }
}
