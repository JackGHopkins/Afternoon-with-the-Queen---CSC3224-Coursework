using Godot;
using System;

public class Music : AudioStreamPlayer
{   
    public override void _Process(float delta)
    {
        if (this.Playing == false){
            this.Play();
        }
    }
}
