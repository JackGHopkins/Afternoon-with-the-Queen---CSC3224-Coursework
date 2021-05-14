using Godot;
using System;

public class Autoload : Label
{
    float time = 0;
    [Export] bool isTime = true;
    [Export] bool isFPS = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        time += delta;

        if(isTime)
            this.Text = "Time Played: " + TimeToString();
        else if (isFPS)
            this.Text = "FPS: " + Engine.GetFramesPerSecond();   
        
        if (Input.IsActionJustPressed("ToggleStats"))
            this.Visible = !this.Visible;
    }

    public string TimeToString()
    {
        int min = (int)time / 60;
        int sec = (int)time % 60;

        // Formating necessary 0s.
        if (min < 10 && sec < 10)
            return "0" + min.ToString() + ":0" + sec.ToString();
        if (sec < 10)
            return min.ToString() + ":0" + sec.ToString();
        if (min < 10)
            return "0" + min.ToString() + ":" + sec.ToString();

        return min.ToString() + ":" + sec.ToString();
    }
}
