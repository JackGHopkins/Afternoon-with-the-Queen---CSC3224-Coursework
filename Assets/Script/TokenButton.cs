using Godot;
using System;

public class TokenButton : TextureButton
{
    Texture textureCross = ResourceLoader.Load("res://Assets/Sprites/Cross3x3.png") as Texture;
    Texture textureCrossHover = ResourceLoader.Load("res://Assets/Sprites/CrossHovered3x3.png") as Texture;
    Texture textureNought = ResourceLoader.Load("res://Assets/Sprites/Nought3x3.png") as Texture;
    Texture textureNoughtHover = ResourceLoader.Load("res://Assets/Sprites/NoughtHovered3x3.png") as Texture;
    Texture textureHover = ResourceLoader.Load("res://Assets/Sprites/Hover3x3.png") as Texture;

    int value = 0;
    [Export] int row = -1;
    [Export] int col = -1;
    [Export] private bool startFocused = false;

    [Signal] delegate void CustomSignal(TokenButton button);

    public override void _Ready()
    {
        if (startFocused)
            GrabFocus();
        Reset();

        //Input.SetMouseMode(Input.MouseMode.Confined);
    }

    public override void _Process(float delta)
    {
        if (GetTree().Paused)
        {
            FocusMode = FocusModeEnum.None;
        }
        else
        {
            FocusMode = FocusModeEnum.All;
        }
    }

    public void SetCross()
    {
        value = 10;
        TextureNormal = textureCross;
        TextureHover = textureCrossHover;
    }

    public void SetNought()
    {
        value = -10;
        TextureNormal = textureNought;
        TextureHover = textureNoughtHover;
    }

    public void Reset()
    {
        value = 0;
        TextureNormal = null;
        TextureHover = textureHover;
        FocusMode = FocusModeEnum.All;
    }

    private void _on_Square_pressed()
    {
        EmitSignal("CustomSignal", this);
    }

    public int GetValue() { return value; }
    public int GetX() { return row; }
    public int GetY() { return col; }

    public void SetValue(int value) { this.value = value; }

}
