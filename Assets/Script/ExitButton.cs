using Godot;
using System;

public class ExitButton : Button
{
    public void _on_exit_pressed()
    {
        GetTree().Quit(); 
    }
}
