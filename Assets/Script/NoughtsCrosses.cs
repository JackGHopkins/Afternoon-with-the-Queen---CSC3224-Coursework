using Godot;
using System;

public class Token
{
    public const int Null = 0;
    public const int Cross = 1;
    public const int Nought = 10;
}

public class NoughtsCrosses : Node
{
    TokenButton[,] board = new TokenButton[3, 3];

    private int currentPlayer = Token.Cross;
    private int turnCount = 0;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                var button = GetNode<TokenButton>("Square" + i.ToString() + "," + j.ToString());
                board[i, j] = button;
                GD.Print(button.ToString());
                board[i, j].Connect("CustomSignal", this, "OnTokenPressed");
            }
        }
    }

    // Fires signal
    public void OnTokenPressed(TokenButton button)
    {
        GD.Print("TokenPressed: " + button.GetX() + button.GetY() );
        
        AddToken(button.GetX(), button.GetY(), currentPlayer);
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

    public void AddToken(int x, int y, int token)
    {
        if (board[x, y].GetValue() == 0)
        {
            board[x, y].SetValue(token);
            turnCount++;
            CheckWin(x, y, token);

            if (currentPlayer != Token.Cross && currentPlayer != Token.Nought)
            {
                GD.Print("currentPlayer not Cross or Nought: " + currentPlayer);
            }

            if (currentPlayer == Token.Cross)
            {
                board[x, y].SetCross();
                currentPlayer = Token.Nought;
            }
            else
            {
                board[x, y].SetNought();
                currentPlayer = Token.Cross;
            }
        }
    }

    public void CheckWin(int x, int y, int token)
    {
        // Checking Column.
        for (int i = 0; i < board.GetLength(1); i++)
        {
            if (board[x, i].GetValue() != token)
                break;
            if (i == board.GetLength(1) - 1)
            {
                PrintWinner();
            }
        }

        // Checking Row.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            if (board[i, y].GetValue() != token)
                break;
            if (i == board.GetLength(0) - 1)
            {
                PrintWinner();
            }
        }

        // Checking Diagonal.
        if (x == y)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (board[i, i].GetValue() != token)
                    break;
                if (i == board.GetLength(0) - 1)
                {
                    PrintWinner();
                }
            }
        }

        // Checking Anti-Diagonal.
        if (x + y == board.GetLength(0) - 1)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (board[i, (board.GetLength(0) - 1) - i].GetValue() != token)
                    break;
                if (i == board.GetLength(0) - 1)
                {
                    PrintWinner();
                }
            }
        }

        // Checking for Stalemate.
        if (turnCount == (Math.Pow(board.GetLength(0), 2) - 1))
        {
            GD.Print("Draw");
        }
    }

    private void PrintWinner()
    {
        if (currentPlayer == Token.Cross)
        {
            GD.Print("Crosses Wins!");
        }

        if (currentPlayer == Token.Nought)
        {
            GD.Print("Noughts Wins!");
        }

        if (currentPlayer != Token.Cross && currentPlayer != Token.Nought)
        {
            GD.Print("Cannot find Winner. CurrentPlayer: " + currentPlayer);
        }

        ClearBoard();
    }

    private void _on_Square_pressed()
    {
        GD.Print(55);
        EmitSignal("CustomSignal", this, turnCount);
    }
}