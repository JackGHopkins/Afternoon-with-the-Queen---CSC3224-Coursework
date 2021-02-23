using Godot;
using System;

public class Menu : MarginContainer
{
    Label hoverOne;
    Label hoverTwo;

    protected int currentHover { get; set; }
    protected int noMenuOptions { get; set; }

    protected Label[] Nodes { get; set; }

    

    public void MenuReady(string[] Paths)
    {
        noMenuOptions = Paths.Length - 1;
        Nodes = new Label[noMenuOptions + 1];
        for (int i = 0; i < Paths.GetLength(0); i++)
        {
            Nodes[i] = GetNode<Label>(Paths[i].ToString());
        }
        currentHover = 0;
        SetCurrentHover();
    }

    public void MenuProcess()
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

    // Virtual Function
    public virtual void HoverAccepted(){}
}
