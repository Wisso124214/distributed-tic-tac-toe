using System;
using System.Linq;
using TicTacToe.Shared;

namespace TicTacToe.Server;

public class GameLogic
{
  public GameState State { get; private set; } = new GameState();
  public char[] Symbols = ['O', 'X'];
  public int[] Score = new int[2]; // [0]=O, [1]=X
  public int Ties = 0;
  private static readonly int[][] WinLines = new int[][]
  {
        [0,1,2], [3,4,5], [6,7,8], // filas
        [0,3,6], [1,4,7], [2,5,8], // columnas
        [0,4,8], [2,4,6]           // diagonales
  };

  public void Reset()
  {
    State.Board = Enumerable.Repeat(' ', 9).ToArray();
    State.Status = GameStatus.Playing;
    State.CurrentTurn = Symbols[(Score[0] + Score[1] + Ties) % 2];
  }

  public bool SetMove(int pos, char symbol)
  {
    if (pos < 0 || pos > 8 || State.Board[pos] != ' ' || State.Status != GameStatus.Playing)
      return false;
    State.Board[pos] = symbol;
    if (CheckWin(symbol))
    {
      State.Status = GameStatus.GameOver;
      if (symbol == 'O') Score[0]++; else Score[1]++;
    }
    else if (State.Board.All(c => c != ' '))
    {
      State.Status = GameStatus.GameOver;
      Ties++;
    }
    else
    {
      State.CurrentTurn = symbol == 'O' ? 'X' : 'O';
    }
    State.PlayerOWins = Score[0];
    State.PlayerXWins = Score[1];
    State.Ties = Ties;
    return true;
  }

  private bool CheckWin(char symbol)
  {
    foreach (var line in WinLines)
      if (line.All(i => State.Board[i] == symbol))
        return true;
    return false;
  }
}
