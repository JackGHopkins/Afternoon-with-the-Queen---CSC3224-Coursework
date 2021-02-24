using Godot;
using System;

public class PauseMenu : Control
{

    SceneTree sceneTree;
    ColorRect pauseOverlay;
    Boolean paused;

    public override void _Ready()
    {
        paused = false;
        sceneTree = GetTree();
        pauseOverlay = GetNode<ColorRect>("PauseOverlay");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            SetPaused(!paused);
            sceneTree.SetInputAsHandled();
        }
    }

    public void SetPaused(bool value)
    {
        paused = value;
        sceneTree.Paused = value;
        pauseOverlay.Visible = value;
    }

}
