using Godot;
using System;

public class PauseMenu : Control
{

    [Export] Boolean paused = false;
    SceneTree sceneTree;
    ColorRect pauseOverlay;

    public override void _Ready()
    {
        paused = false;
        sceneTree = GetTree();
        pauseOverlay = GetNode<ColorRect>("PauseOverlay");
        pauseOverlay.Visible = false;
    }

    public override void _Process(float delta)
    {
        if (!pauseOverlay.Visible)
        {
            paused = false;
        }

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

        public void _on_ResumeButton_pressed()
    {
        SetPaused(!paused);
    }
}
