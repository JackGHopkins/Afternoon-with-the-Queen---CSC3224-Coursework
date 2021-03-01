using Godot;
using System;

public class PauseMenu : Control
{

    [Export] Boolean paused = false;
    [Export] NodePath optionsMenuPath;
    [Signal] delegate void AIDifficultyFromPause();
    SceneTree sceneTree;
    ColorRect pauseOverlay;
    private Button optionsMenu;

    public override void _Ready()
    {
        paused = false;
        sceneTree = GetTree();
        pauseOverlay = GetNode<ColorRect>("PauseOverlay");
        pauseOverlay.Visible = false;

        // Connecting AI Options to script
        optionsMenu = GetNode<Button>(optionsMenuPath);
        optionsMenu.Connect("AIDifficulty", this, "HandleAIDifficulty");
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

    public void HandleAIDifficulty(){
        GD.Print("PauseMenu");
    }
}
