using Godot;
using System;

public class RestartButton : Button
{
    public void _on_restart_pressed()
    {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }
}
