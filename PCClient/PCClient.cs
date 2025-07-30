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
        Console.Clear();
        PrintBoard(msg.State.Board);
        Console.Write("PC es '");
        PrintWithColor(msg.YourSymbol, msg.YourSymbol == 'O' ? GameConfig.ConfigColorO : GameConfig.ConfigColorX);
        Console.Write("'. Turno: '");
        PrintWithColor(msg.State.CurrentTurn, msg.State.CurrentTurn == 'O' ? GameConfig.ConfigColorO : GameConfig.ConfigColorX);
        Console.WriteLine("'");
        Console.Write("Marcador: ");
        PrintWithColor('O', GameConfig.ConfigColorO);
        Console.Write($": {msg.State.PlayerOWins} ");
        PrintWithColor('X', GameConfig.ConfigColorX);
        Console.Write($": {msg.State.PlayerXWins} Empates: {msg.State.Ties}\n");
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
          PrintWithColor('O', GameConfig.ConfigColorO);
        }
        else if (board[idx] == 'X')
        {
          PrintWithColor('X', GameConfig.ConfigColorX);
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

  static void PrintWithColor(char symbol, ConsoleColor color)
  {
    var prev = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.Write(symbol);
    Console.ForegroundColor = prev;
  }
}
