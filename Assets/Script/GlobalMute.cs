using Godot;
using System;

public class GlobalMute : Sprite
{
    bool mute = false;
    int masterIndex;

    Texture soundOn = ResourceLoader.Load("res://Assets/Sprites/SoundOn.png") as Texture;
    Texture soundOff = ResourceLoader.Load("res://Assets/Sprites/SoundOff.png") as Texture;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var masterIndex = AudioServer.GetBusIndex("Master");

        mute = false;
            
        AudioServer.SetBusMute(masterIndex, mute);
        
        if (Input.IsActionJustPressed("ToggleMute"))
        {
            if(!mute)
                this.Texture = soundOff;
            else 
                this.Texture = soundOn;

            mute = !mute;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        AudioServer.SetBusMute(masterIndex, mute);
        
        if (Input.IsActionJustPressed("ToggleMute"))
        {

            if(!mute)
                this.Texture = soundOff;
            else 
                this.Texture = soundOn;

            mute = !mute;
        }
    }
}
