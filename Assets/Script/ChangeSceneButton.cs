using Godot;
using System;

public class ChangeSceneButton : Button
{
    [Export(PropertyHint.File)] String nextScenePath;

    public void _on_change_scene_pressed()
    {
        GetTree().ChangeScene(nextScenePath);
    }
}
