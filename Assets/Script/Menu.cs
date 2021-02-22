using Godot;
using System;

public class Menu : MarginContainer
{
    Label hoverOne;
    Label hoverTwo;

    private int currentHover;
    private const int noMenuOptions = 1;

    [Export] bool PauseOn = false;
    [Export] String[] Paths = new string[noMenuOptions + 1];
    Label[] Nodes = new Label[noMenuOptions + 1];

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < Paths.GetLength(0); i++)
        {
            Nodes[i] = GetNode<Label>(Paths[i].ToString());
        }
        currentHover = 0;
        SetCurrentHover();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_down") && currentHover < noMenuOptions)
        {
            currentHover++;
            SetCurrentHover();
        }
        else if (Input.IsActionJustPressed("ui_up") && currentHover > 0)
        {
            currentHover--;
            SetCurrentHover();
        }
        else if (Input.IsActionJustPressed("ui_down") && currentHover == noMenuOptions)
        {
            currentHover = 0;
            SetCurrentHover();
        }
        else if (Input.IsActionJustPressed("ui_up") && currentHover == 0)
        {
            currentHover = noMenuOptions;
            SetCurrentHover();
        }
        else if (Input.IsActionJustPressed("ui_accept"))
        {
            HoverAccepted();
        }

        else if(Input.IsActionJustPressed("ui_cancel") && PauseOn == true){
            GetTree().Paused = !GetTree().Paused;
        }
    }

    public void SetCurrentHover()
    {
        for (int i = 0; i < Nodes.GetLength(0); i++)
        {
            Nodes[i].Text = "";

            if (currentHover == i)
            {
                Nodes[i].Text = ">";
            }
        }
    }

    public void HoverAccepted()
    {
        if (currentHover == 0)
        {
            GetTree().ChangeScene("res://Assets/Scenes/Noughts&Crosses.tscn");
        }
        else if (currentHover == Nodes.GetLength(0) - 1)
        {
            GetTree().Quit();
        }
    }
}
