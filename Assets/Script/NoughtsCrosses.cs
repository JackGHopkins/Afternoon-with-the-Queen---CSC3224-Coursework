using Godot;
using System;

public class Token
{
    public const int Null = 0;
    public const int Cross = 10;
    public const int Nought = -10;
}

public enum WinState
{
    Null = 0,
    Winner = 1,
    Tie = 2,
}

public class NoughtsCrosses : Sprite
{
    [Export(PropertyHint.File)] String[] endScenePath;
    [Export] bool singlePlayer = true;
    [Export] bool bestAI;
    [Export] bool timerToggle = true;
    [Export] NodePath optionsMenuPath;
    [Export] NodePath aiPath;
    [Export] NodePath timerQueenPath;
    [Export] NodePath timerPrisonerPath;

    int[] aiMoveCoord = new int[2];

    Timer delayTimer;
    PlayerTimer playerTimer;

    TokenButton[,] mainBoard = new TokenButton[3, 3];

    public int currentPlayer { get; set; }
    private int turnCount = 0;
    private int maxTurnCount = 0;
    private bool hasWinner = false;
    private Control optionsMenu;
    private Label aiStatus;
    private Label timerQueen;
    private Label timerPrisoner;

    public override void _Ready()
    {
        GetTree().Paused = false;
        delayTimer = GetNode<Timer>("AIDelay");
        delayTimer.OneShot = true;
        currentPlayer = Token.Cross;

        maxTurnCount = (int)Math.Pow(mainBoard.GetLength(0), 2);
        // Making the Board
        for (int i = 0; i < mainBoard.GetLength(0); i++)
        {
            for (int j = 0; j < mainBoard.GetLength(1); j++)
            {
                var button = GetNode<TokenButton>("Square" + i.ToString() + "," + j.ToString());
                mainBoard[i, j] = button;
                GD.Print(button.ToString());
                mainBoard[i, j].Connect("CustomSignal", this, "OnTokenPressed");
            }
        }

        // Connecting AI Options to script
        optionsMenu = GetNode<Control>(optionsMenuPath);
        //optionsMenu.Connect("AIDifficulty", this, "HandleAIDifficulty");

        // Setting up AI Status indicator
        aiStatus = GetNode<Label>(aiPath);

        // Setting up Timer
        timerQueen = GetNode<Label>(timerQueenPath);
        timerPrisoner = GetNode<Label>(timerPrisonerPath);
    }

    public override void _Process(float delta)
    {
        AIStatus();
    }

    public void AIStatus()
    {
        if (Input.IsActionPressed("ToggleSinglePlayer"))
        {
            singlePlayer = false;
            aiStatus.Text = "Two Player";
        }

        if (Input.IsActionPressed("ToggleRandomAI"))
        {
            singlePlayer = true;
            bestAI = false;
            aiStatus.Text = "Random AI";
        }

        if (Input.IsActionPressed("ToggleBestAI"))
        {
            singlePlayer = true;
            bestAI = true;
            aiStatus.Text = "Unbeatable";
        }
    }

    public void ClearBoard(TokenButton[,] board)
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

    public void AddToken(int x, int y, int token, TokenButton[,] board)
    {
        if (board[x, y].GetValue() == 0)
        {
            board[x, y].SetValue(token);
            turnCount++;
            //PrintBoard(board);
            PrintWinner(CheckWin(board));

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

    public WinState CheckWin(TokenButton[,] board)
    {
        // Checking Column.
        for (int i = 0; i < board.GetLength(1); i++)
        {
            // Checking if all three are equal to each other. And not Empty.
            if (board[i, 0].GetValue() == board[i, 1].GetValue() && board[i, 1].GetValue() == board[i, 2].GetValue() && board[i, 0].GetValue() != 0)
                return WinState.Winner;
        }

        // Checking Row.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            // Checking if all three are equal to each other. And not Empty.
            if (board[0, i].GetValue() == board[1, i].GetValue() && board[1, i].GetValue() == board[2, i].GetValue() && board[0, i].GetValue() != 0)
                return WinState.Winner;
        }

        // Checking Diagonal. Checking if all three are equal to each other. And not Empty.
        if (board[0, 0].GetValue() == board[1, 1].GetValue() && board[1, 1].GetValue() == board[2, 2].GetValue() && board[1, 1].GetValue() != 0)
            return WinState.Winner;

        // Checking Anti-Diagonal. Checking if all three are equal to each other. And not Empty.
        if (board[2, 0].GetValue() == board[1, 1].GetValue() && board[1, 1].GetValue() == board[0, 2].GetValue() && board[1, 1].GetValue() != 0)
            return WinState.Winner;

        // Checking number of free cells
        int cellsFree = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].GetValue() == Token.Null)
                    cellsFree++;
            }
        }


        // Checking for Stalemate.
        if (cellsFree == 0)
        {
            return WinState.Tie;
        }

        return WinState.Null;
    }

    private void PrintWinner(WinState state)
    {
        if (state == WinState.Null)
        {
            return;
        }
        else if (state == WinState.Winner)
        {
            if (currentPlayer == Token.Cross)
            {
                GetTree().ChangeScene(endScenePath[0]);
            }
            else if (currentPlayer == Token.Nought && turnCount < maxTurnCount)
            {
                GetTree().ChangeScene(endScenePath[1]);
            }
            hasWinner = true;
        }
        else if (state == WinState.Tie)
        {
            if (turnCount == maxTurnCount)
            {
                GetTree().ChangeScene(endScenePath[2]);
            }
        }
        else if (currentPlayer != Token.Cross && currentPlayer != Token.Nought)
        {
            GD.Print("Cannot find Winner. CurrentPlayer: " + currentPlayer);
        }
        ClearBoard(mainBoard);
    }

    public void AITurn(TokenButton[,] copyBoard)
    {
        if (bestAI == true)
        {
            BestMove(copyBoard);
        }
        else
        {
            RandomMove(copyBoard);
        }
        AddToken(aiMoveCoord[0], aiMoveCoord[1], Token.Nought, copyBoard);
    }

    public void BestMove(TokenButton[,] board)
    {
        GD.Print("\n");
        GD.Print("Turn:" + turnCount);
        int bestScore = int.MaxValue;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                // Is spot available?
                if (board[i, j].GetValue() == Token.Null)
                {

                    board[i, j].SetValue(Token.Nought);

                    int score = MiniMax(board, 0, true, Token.Nought, int.MinValue, int.MaxValue);
                    board[i, j].SetValue(Token.Null);

                    GD.Print("Score: " + score + ": [" + i + "," + j + "]");

                    if (score < bestScore)
                    {
                        GD.Print("BestScore: " + bestScore + ": [" + i + "," + j + "]");
                        bestScore = score;
                        GD.Print("BestScore [Updated]: " + bestScore + ": [" + i + "," + j + "]");
                        aiMoveCoord[0] = i;
                        aiMoveCoord[1] = j;
                    }
                }
            }
        }
    }

    public int MiniMax(TokenButton[,] tempBoard, int depth, bool isMaximising, int token, int alpha, int beta)
    {
        WinState state = CheckWin(tempBoard);
        if (state == WinState.Winner)
        {
            if (token == Token.Cross)
                return (int)token;
            else if (token == Token.Nought)
                return (int)token;
        }
        else if (state == WinState.Tie)
        {
            return 0;
        }

        if (isMaximising) // Crosses is playing.
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < tempBoard.GetLength(0); i++)
            {
                for (int j = 0; j < tempBoard.GetLength(1); j++)
                {
                    if (tempBoard[i, j].GetValue() == Token.Null)
                    {
                        tempBoard[i, j].SetValue(Token.Cross);
                        int score = MiniMax(tempBoard, depth + 1, false, Token.Cross, alpha, beta);
                        tempBoard[i, j].SetValue(Token.Null);

                        bestScore = Math.Max(score, bestScore);

                        //Beta
                        alpha = Math.Max(alpha, score);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }
            return bestScore;
        }
        else // If minimising. Noughts is playing.
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < tempBoard.GetLength(0); i++)
            {
                for (int j = 0; j < tempBoard.GetLength(1); j++)
                {
                    if (tempBoard[i, j].GetValue() == Token.Null)
                    {
                        tempBoard[i, j].SetValue(Token.Nought);
                        int score = MiniMax(tempBoard, depth + 1, true, Token.Nought, alpha, beta);
                        tempBoard[i, j].SetValue(Token.Null);

                        bestScore = Math.Min(score, bestScore);

                        //Beta
                        beta = Math.Min(beta, score);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }
            return bestScore;
        }
    }


    /*
        The idea is to get the number of free cells left, and pick a random one between them. 
        Iterate through cells, counting up only when a cell is free. When you find the cell, 
        then you assign its x and y values to aiMoveCoord.
    */
    public void RandomMove(TokenButton[,] board)
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

    // SIGNALS
    public void OnTokenPressed(TokenButton button)
    {
        GD.Print("TokenPressed: " + button.GetX() + button.GetY());

        AddToken(button.GetX(), button.GetY(), currentPlayer, mainBoard);

        if (singlePlayer == true && currentPlayer == Token.Nought && turnCount != maxTurnCount && !hasWinner)
        {
            if (bestAI)
            {
                AITurn(mainBoard);
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

    public void HandleAIDifficulty()
    {
        GD.Print("NoughtsxCross Ai Changed");
    }

    private void _on_Square_pressed()
    {
        GD.Print(55);
        EmitSignal("CustomSignal", this, turnCount);
    }

    private void _on_AI_Turn()
    {
        GetTree().Paused = false;
        AITurn(mainBoard);
    }

    void PrintBoard(TokenButton[,] printBoard)
    {
        GD.Print(" " + printBoard[0, 0].GetValue() + " | " + printBoard[0, 1].GetValue() + " | " + printBoard[0, 2].GetValue());
        GD.Print("-------------------------");
        GD.Print(" " + printBoard[1, 0].GetValue() + " | " + printBoard[1, 1].GetValue() + " | " + printBoard[1, 2].GetValue());
        GD.Print("-------------------------");
        GD.Print(" " + printBoard[2, 0].GetValue() + " | " + printBoard[2, 1].GetValue() + " | " + printBoard[2, 2].GetValue());
    }
}