using Godot;
using System;

public class PlayerTimer : Label
{
    [Export] bool isQueen = true;
    [Export] bool isGuard = false;
    [Export] int time = 60;
    [Export(PropertyHint.File)] String[] endScenePath;

    Timer timer;
    bool timerEnabled = true;

    NoughtsCrosses grid;

    public override void _Ready()
    {
        if (isQueen)
            this.Text = "Queen - " + TimeToString();
        else if (isGuard)
            this.Text = "Guard - " + TimeToString();
        else
            this.Text = ".Prisoner - " + TimeToString();

        timer = GetNode<Timer>("Timer");
        timer.WaitTime = 1;
        if (!isQueen && !isGuard)
            timer.Start();

        grid = GetNode<NoughtsCrosses>("../Grid3x3");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        grid = GetNode<NoughtsCrosses>("../Grid3x3");

        // Turn Queen's timer of if its not her turn.
        if (isQueen && grid.currentPlayer != Token.Nought)
        {
            timer.Stop();
            this.Text = "Queen - " + TimeToString();
        }
        else if (isGuard && grid.currentPlayer != Token.Nought)
        {
            timer.Stop();
            this.Text = "Guard - " + TimeToString();
        }
        // Turn Player's timer of if its not her turn.
        else if ((!isQueen || !isGuard)  && grid.currentPlayer != Token.Cross)
        {
            timer.Stop();
            this.Text = "Prisoner - " + TimeToString();
        }
        else if (timer.IsStopped() && timerEnabled)
        {
            if (isQueen)
                this.Text = ".Queen - " + TimeToString();
            else
                this.Text = ".Prisoner - " + TimeToString();

            timer.Start();
        }

        if (time == -1)
        {
            if (isQueen || isGuard)
            {
                GetTree().ChangeScene(endScenePath[0]);
            }
            else
            {
                GetTree().ChangeScene(endScenePath[1]);
            }
        }

        if (Input.IsActionJustPressed("ToggleTimers")){
            GD.Print("Timers Toggled");
            timerEnabled = !timerEnabled;
        }
        
        if(!timerEnabled)
            timer.Stop();
    }

    // Formats time to look like a digital clock.
    public string TimeToString()
    {
        int min = time / 60;
        int sec = time % 60;

        // Formating necessary 0s.
        if (min < 10 && sec < 10)
            return "0" + min.ToString() + ":0" + sec.ToString();
        if (sec < 10)
            return min.ToString() + ":0" + sec.ToString();
        if (min < 10)
            return "0" + min.ToString() + ":" + sec.ToString();

        return min.ToString() + ":" + sec.ToString();
    }

    /* SIGNALS */
    public void _on_PlayerTimer_timeout()
    {
        time--;
        if (isQueen)
            this.Text = ".Queen - " + TimeToString();
        else
            this.Text = ".Prisoner - " + TimeToString();
    }
}
