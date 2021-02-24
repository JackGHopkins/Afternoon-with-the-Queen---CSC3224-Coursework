using Godot;
using System;

public class EndScene : Control
{
    [Export(PropertyHint.File)] string filepath;
    Label winnerText;

    public void SetWinnerText(string winner)
    {
        winnerText.GetNode<Label>("PauseOverlay/Crosses Wins");
        winnerText.Text = winner;
    }
}
