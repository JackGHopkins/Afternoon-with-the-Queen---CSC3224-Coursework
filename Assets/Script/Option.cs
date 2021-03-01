using Godot;
using System;

public class Option : Button
{
    [Export] NodePath Path;
    Control menu;

    public override void _Ready()
    {
        menu = GetNode<Control>(Path);
        menu.Visible = false;
    }

    public void _on_Option_pressed(){
        menu.Visible = !menu.Visible;
    }
}
