using Godot;
using System;

public class TokenButton : TextureButton
{
    Texture textureNought = ResourceLoader.Load("res://Assets/Sprites/Nought3x3.png") as Texture;
    Texture textureCross = ResourceLoader.Load("res://Assets/Sprites/Cross3x3.png") as Texture;
    Texture textureHover = ResourceLoader.Load("res://Assets/Sprites/Hover3x3.png") as Texture;

    NoughtsCrosses Grid;

    [Export] int row = -1;
    [Export] int col = -1;
    [Export] private bool startFocused = false;

    [Signal] delegate void CustomSignal(TextureButton button);
    int turnCount = 0;

    public override void _Ready()
    {
        if (startFocused)
            GrabFocus();

        Reset();
        //this.Connect("pressed", this, "OnPressed");
    }

    void OnPressed(){
        SetToken(turnCount);
        // Grid.SetTurnCount();
        GD.Print("TurnCount is: ");
        Disabled = true;
    }

    public void SetToken(int turnCount)
    {
        if (turnCount % 2 == 0)
        {
            SetCross();
        }
        else
        {
            SetNought();
        }

    }
    public void SetCross()
    {
        TextureNormal = textureCross;
    }

    public void SetNought()
    {
        TextureNormal = textureNought;
    }

    public void Reset()
    {
        TextureNormal = null;
        TextureHover = textureHover;
        FocusMode = FocusModeEnum.All;
    }

        private void _on_Square_pressed(){
        GD.Print(row + "," + col);
        EmitSignal("CustomSignal", this);
    }

    public int GetCol(){
        return col;
    }
}
