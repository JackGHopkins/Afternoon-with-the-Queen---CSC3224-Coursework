using Godot;
using System;

public class ResumeButton : Button
{
    [Export] NodePath ParentPath;
    ColorRect pauseOverlay;

    public override void _Ready()
    {
        ColorRect pauseOverlay = GetNode<ColorRect>(ParentPath);
    }

    public void _on_ResumeButton_pressed()
    {
        ColorRect pauseOverlay = GetNode<ColorRect>(ParentPath);
        pauseOverlay.Visible = false;
        GetTree().Paused = false;
    }
}
