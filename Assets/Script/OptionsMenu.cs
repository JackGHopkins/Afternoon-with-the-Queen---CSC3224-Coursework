using Godot;
using System;

public class OptionsMenu : Control
{

    [Signal] delegate void AIDifficulty();

    public override void _Ready()
    {

    }

    public void _on_AIDifficulty_item_selected()
    {
        EmitSignal("AIDifficulty", this);
        GD.Print("AI changed");
    }
}
