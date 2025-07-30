using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using TicTacToe.Shared;

class PCClient
{
  static async Task Main()
  {
    using var client = new TcpClient();
    await client.ConnectAsync("127.0.0.1", 9000);
    using var stream = client.GetStream();
    var reader = new StreamReader(stream);
    var writer = new StreamWriter(stream) { AutoFlush = true };
    var rand = new Random();
    try
    {
      while (true)
      {
        var line = await reader.ReadLineAsync();
        if (line == null) break;
        var msg = JsonSerializer.Deserialize<ServerToClientMessage>(line);
        PrintBoard(msg.State.Board);
        Console.WriteLine($"PC es '{msg.YourSymbol}'. Turno: '{msg.State.CurrentTurn}'");
        Console.WriteLine($"Marcador: O: {msg.State.PlayerOWins} X: {msg.State.PlayerXWins} Empates: {msg.State.Ties}");
        if (msg.State.Status == GameStatus.GameOver)
        {
          if (msg.CanPlayAgain)
          {
            var resp = new ClientToServerMessage { PlayAgain = true };
            await writer.WriteLineAsync(JsonSerializer.Serialize(resp));
          }
          continue;
        }
        if (msg.YourTurn)
        {
          // Elegir movimiento aleatorio válido
          var moves = msg.State.Board.Select((c, i) => (c, i)).Where(x => x.c == ' ').Select(x => x.i).ToList();
          int move = moves[rand.Next(moves.Count)];
          Console.WriteLine($"PC pensando...");
          await Task.Delay(700);
          var play = new ClientToServerMessage { Move = move };
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
          Console.Write(idx);
        }
        if (j < 2) Console.Write(" |");
      }
      Console.WriteLine();
      if (i < 6) Console.WriteLine("---+---+---");
    }
    Console.WriteLine();
  }
}
