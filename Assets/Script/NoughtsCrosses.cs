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
    [Export(PropertyHint.File)] String[] endScenePath;
    [Export] bool singlePlayer = true;
    [Export] bool bestAI;

    int[] aiMoveCoord = new int[2];

    Timer delayTimer;

    TokenButton[,] board = new TokenButton[3, 3];

    private int currentPlayer = Token.Cross;
    private int turnCount = 0;
    private int maxTurnCount = 0;

    public override void _Ready()
    {
        GetTree().Paused = false;
        delayTimer = GetNode<Timer>("AIDelay");
        delayTimer.OneShot = true;

        maxTurnCount = (int)Math.Pow(board.GetLength(0), 2);
        // Making the Board
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
        GD.Print("TokenPressed: " + button.GetX() + button.GetY());

        AddToken(button.GetX(), button.GetY(), currentPlayer);

        if (singlePlayer == true && currentPlayer == Token.Nought && turnCount != maxTurnCount)
        {
            if (bestAI)
            {
                AITurn();
            }
            else
            {
                Random random = new Random();
                int pause = random.Next(2, 15);

                delayTimer.WaitTime = (float)pause / 10;
                GD.Print("The Queen is thinking...");
                delayTimer.Start();
                GetTree().Paused = true;
                GD.Print("The queen has moved. Time: " + (float)pause / 10);
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
        else if (board[x, y].GetValue() != 0 && token == Token.Nought && singlePlayer == true)
        {
            GD.Print("Values are: [" + x + "," + y + "] - Cannot be placed.");
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
        if (turnCount == maxTurnCount)
        {
            PrintWinner();
        }
    }

    private void PrintWinner()
    {
        EndScene endScene = new EndScene();
        //String winningText = "";
        if (currentPlayer == Token.Cross)
        {
            GetTree().ChangeScene(endScenePath[0]);
        }
        else if (currentPlayer == Token.Nought && turnCount < maxTurnCount)
        {
            GetTree().ChangeScene(endScenePath[1]);
        }
        else if (turnCount == maxTurnCount)
        {
            GetTree().ChangeScene(endScenePath[2]);
        }
        else if (currentPlayer != Token.Cross && currentPlayer != Token.Nought)
        {
            GD.Print("Cannot find Winner. CurrentPlayer: " + currentPlayer);
        }
        //endScene.SetWinnerText(winningText);

        ClearBoard();
    }

    public void AITurn()
    {
        if (bestAI == true)
        {
            BestMove();
        }
        else
        {
            RandomMove();
        }
        AddToken(aiMoveCoord[0], aiMoveCoord[1], Token.Nought);
    }

    public void BestMove()
    {
        int score = 0;
        int bestScore = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].GetValue() == 0)
                {
                    board[i, j].SetValue(Token.Nought);
                    score = MiniMax();
                    board[i, j].SetValue(Token.Null);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        aiMoveCoord[0] = i;
                        aiMoveCoord[1] = j;
                    }
                }
            }
        }
    }

    public int MiniMax()
    {
        return 1;
    }

    /*
        The idea is to get the number of free cells left, and pick a random one between them. 
        Iterate through cells, counting up only when a cell is free. When you find the cell, 
        then you assign its x and y values to aiMoveCoord.
    */
    public void RandomMove()
    {
        Random rnd = new Random();
        int movesLeft = maxTurnCount - turnCount - 1;
        int randCell = rnd.Next(0, movesLeft + 1); // creates a random number between 0 and number of movesLeft. 
        int cellCount = 0;

        GD.Print("movesLeft: " + movesLeft);
        GD.Print("randCell: " + randCell);

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].GetValue() == 0)
                {
                    if (randCell == cellCount)
                    {
                        aiMoveCoord[0] = i;
                        aiMoveCoord[1] = j;
                        return;
                    }
                    cellCount++;
                }
            }
        }
    }

    private void _on_Square_pressed()
    {
        GD.Print(55);
        EmitSignal("CustomSignal", this, turnCount);
    }

    private void _on_AI_Turn()
    {
        GetTree().Paused = false;
        AITurn();
    }
}