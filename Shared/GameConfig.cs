using System;

namespace TicTacToe.Shared
{
  public static class GameConfig
  {
    public static readonly int[][] WinLines =
    [
        [0,1,2], [3,4,5], [6,7,8], // Filas
        [0,3,6], [1,4,7], [2,5,8], // Columnas
        [0,4,8], [2,4,6]           // Diagonales
    ];

    public static ConsoleColor ConfigColorO { get; set; } = ConsoleColor.Green;
    public static ConsoleColor ConfigColorX { get; set; } = ConsoleColor.Red;
  }
}
