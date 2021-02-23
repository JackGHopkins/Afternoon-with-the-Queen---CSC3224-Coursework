using Godot;
using System;

public class PauseOverlay : Control
{

    public override void _Ready()
    {
        GetTree().Paused = false;
        Visible = false;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            var newPauseState = GetTree().Paused;
            GetTree().Paused = !newPauseState;
            Visible = newPauseState;
        }
    }
}
