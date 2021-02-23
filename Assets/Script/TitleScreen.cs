using Godot;
using System;

public class TitleScreen : Menu
{
    [Export] String[] Paths = new string[2];

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MenuReady(Paths);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        MenuProcess();
    }

    public override void HoverAccepted()
    {
        if (currentHover == 0)
        {
            GetTree().ChangeScene("res://Assets/Scenes/Noughts&Crosses.tscn");
        }
        else if (currentHover == Nodes.Length - 1)
        {
            GetTree().Quit();
        }
    }
}