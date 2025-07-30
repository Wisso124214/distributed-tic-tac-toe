using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using TicTacToe.Shared;

class UserClient
{
  static async Task Main()
  {
    using var client = new TcpClient();
    await client.ConnectAsync("127.0.0.1", 9000);
    using var stream = client.GetStream();
    var reader = new StreamReader(stream);
    var writer = new StreamWriter(stream) { AutoFlush = true };
    try
    {
      while (true)
      {
        var line = await reader.ReadLineAsync();
        if (line == null) break;
        // Si el mensaje no es JSON, es un mensaje de cierre del servidor
        if (!line.TrimStart().StartsWith("{"))
        {
          Console.WriteLine(line);
          break;
        }
        var msg = JsonSerializer.Deserialize<ServerToClientMessage>(line);
        PrintBoard(msg.State.Board);
        Console.WriteLine($"Eres '{msg.YourSymbol}'. Turno: '{msg.State.CurrentTurn}'");
        Console.WriteLine($"Marcador: O: {msg.State.PlayerOWins} X: {msg.State.PlayerXWins} Empates: {msg.State.Ties}");
        if (msg.State.Status == GameStatus.GameOver)
        {
          Console.WriteLine("El juego ha terminado.");
          Console.Write("¿Jugar de nuevo? (s/n): ");
          var again = Console.ReadLine();
          var resp = new ClientToServerMessage { PlayAgain = again?.Trim().ToLower() == "s" };
          Console.WriteLine($"Respuesta: {again?.Trim().ToLower() == "s"}");
          await writer.WriteLineAsync(JsonSerializer.Serialize(resp));
        }
        if (msg.YourTurn)
        {
          int move;
          while (true)
          {
            Console.Write("Elige tu movimiento (1-9): ");
            if (int.TryParse(Console.ReadLine(), out move) && move >= 1 && move <= 9 && msg.State.Board[move - 1] == ' ')
              break;
            Console.WriteLine("Movimiento inválido. Intenta de nuevo.");
          }
          var play = new ClientToServerMessage { Move = move - 1 };
          await writer.WriteLineAsync(JsonSerializer.Serialize(play));
        }
        else
        {
          Console.WriteLine("Esperando al otro jugador...");
        }
      }
    }
    catch (System.IO.IOException)
    {
      Console.WriteLine("La conexión ha sido interrumpida por el host remoto.");
    }
  }

  static void PrintBoard(char[] board)
  {
    Console.WriteLine();
    for (int i = 0; i < 9; i += 3)
    {
      for (int j = 0; j < 3; j++)
      {
        Console.Write(" ");
        int idx = i + j;
        if (board[idx] == 'O')
        {
          Console.ForegroundColor = GameConfig.ConfigColorO;
          Console.Write("O");
          Console.ResetColor();
        }
        else if (board[idx] == 'X')
        {
          Console.ForegroundColor = GameConfig.ConfigColorX;
          Console.Write("X");
          Console.ResetColor();
        }
        else
        {
          Console.Write(idx + 1);
        }
        if (j < 2) Console.Write(" |");
      }
      Console.WriteLine();
      if (i < 6) Console.WriteLine("---+---+---");
    }
    Console.WriteLine();
  }
}
