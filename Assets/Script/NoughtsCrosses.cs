using Godot;
using System;

public class NoughtsCrosses : Node
{
    int board[3][3];


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
     
    }
}

class enum Token
{
    None = 0, Nought, Cross;
}
