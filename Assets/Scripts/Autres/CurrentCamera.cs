using Godot;
using System;

public class CurrentCamera : Camera2D
{
    private static Camera2D current;

    public static void Init(Camera2D cam)
    {
        current = cam;
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
