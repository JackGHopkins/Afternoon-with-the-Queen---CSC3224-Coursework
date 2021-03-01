using Godot;
using System;

public class Introduction : Control
{
    [Export(PropertyHint.File)] String nextScenePath;

    public override void _Process(float delta)
     {
         if (Input.IsActionPressed("ui_accept"))
            GetTree().ChangeScene(nextScenePath);
     }
}
