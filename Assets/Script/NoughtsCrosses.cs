using Godot;
using System;

public class Token
{
    public const int Null = 0;
    public const int Cross = 1;
    public const int Nought = 2;
}

public class NoughtsCrosses : Node
{
    TextureButton[,] board = new TextureButton[3, 3];

    private int turnCount = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                var button = GetNode<TextureButton>("Square" + i.ToString() + "," + j.ToString());
                board[i,j] = button;
                GD.Print(button.ToString());
                board[i,j].Connect( "CustomSignal", this, "OnTokenPressed");
            }
        }
    }

    public void OnTokenPressed(TextureButton button)
    {
        GD.Print("TokenPressed");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {

            }
        }
    }


    public void ClearBoard()
    {
        turnCount = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                board[i, j].Disabled = false;
            }
        }
    }

    public int GetTurnCount() { return turnCount; }
    public void SetTurnCount() { turnCount = turnCount + 1; }

    // public void AddToken(int x, int y, int token)
    // {
    //     if (board[x, y] == 0) {
    //         board[x, y] = token;
    //         turnCount++;
    //         CheckWin(x, y, token);
    //     }
    // }

    // public void CheckWin(int x, int y, int token)
    // {
    //     // Checking Column.
    //     for (int i = 0; i < board.GetLength(1); i++)
    //     {
    //         if (board[x,i] != token)
    //             break;
    //         if (i == board.GetLength(1) - 1)
    //         {
    //             //report win for s
    //         }
    //     }

    //     // Checking Row.
    //     for (int i = 0; i < board.GetLength(0); i++)
    //     {
    //         if (board[i, y] != token)
    //             break;
    //         if (i == board.GetLength(0) - 1)
    //         {
    //             //report win for s
    //         }
    //     }

    //     // Checking Diagonal.
    //     if (x == y)
    //     {
    //         for (int i = 0; i < board.GetLength(0); i++)
    //         {
    //             if (board[i, i] != token)
    //                 break;
    //             if (i == board.GetLength(0) - 1)
    //             {
    //                 //report win for s
    //             }
    //         }
    //     }

    //     // Checking Anti-Diagonal.
    //     if (x + y == board.GetLength(0) - 1)
    //     {
    //         for (int i = 0; i < board.GetLength(0); i++)
    //         {
    //             if (board[i, (board.GetLength(0) - 1) - i] != token)
    //                 break;
    //             if (i == board.GetLength(0) - 1)
    //             {
    //                 //report win for s
    //             }
    //         }
    //     }

    //     // Checking for Stalemate.
    //     if (turnCount == (Math.Pow(board.GetLength(0), 2) - 1))
    //     {
    //         //report draw
    //     }
    // }

    private void _on_Square_pressed(){
        GD.Print(55);
        EmitSignal("CustomSignal", this);
    }
}
