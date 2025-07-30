using System;
using System.Collections.Generic;

namespace TicTacToe.Components
{
  public static class GameConfig
  {
    public static readonly int[][] WinLines =
    [
      [0,1,2], [3,4,5], [6,7,8], // Filas
      [0,3,6], [1,4,7], [2,5,8], // Columnas
      [0,4,8], [2,4,6]           // Diagonales
    ];

    /* Colores disponibles de consola:
      * Black
      * DarkBlue
      * DarkGreen
      * DarkCyan
      * DarkRed
      * DarkMagenta
      * DarkYellow
      * Gray
      * DarkGray
      * Blue
      * Green
      * Cyan
      * Red
      * Magenta
      * Yellow
      * White
    */

    public static ConsoleColor ConfigColorO { get; set; } = ConsoleColor.Green;
    public static ConsoleColor ConfigColorX { get; set; } = ConsoleColor.Red;
  }
}
