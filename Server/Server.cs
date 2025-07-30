using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using TicTacToe.Shared;

class Server
{
  static async Task Main()
  {
    try
    {
      var listener = new TcpListener(IPAddress.Any, 9000);
      listener.Start();
      Console.WriteLine("Esperando jugadores...");

      var clients = new TcpClient[2];
      var streams = new NetworkStream[2];
      for (int i = 0; i < 2; i++)
      {
        clients[i] = await listener.AcceptTcpClientAsync();
        streams[i] = clients[i].GetStream();
        Console.WriteLine($"Jugador {i + 1} conectado");
      }

      // Lógica principal del juego
      var logic = new TicTacToe.Server.GameLogic();
      char[] symbols = ['O', 'X'];
      bool[] playAgain = new bool[2];
      bool[] canPlayAgain = new bool[2];
      logic.Reset();

      while (true)
      {
        // Enviar estado a ambos clientes
        for (int i = 0; i < 2; i++)
        {
          var msg = new ServerToClientMessage
          {
            State = logic.State,
            YourSymbol = symbols[i],
            YourTurn = logic.State.CurrentTurn == symbols[i] && logic.State.Status == GameStatus.Playing,
            CanPlayAgain = logic.State.Status == GameStatus.GameOver
          };
          var json = JsonSerializer.Serialize(msg);
          var buffer = System.Text.Encoding.UTF8.GetBytes(json + "\n");
          await streams[i].WriteAsync(buffer, 0, buffer.Length);
        }

        // Esperar jugada del jugador cuyo turno es
        int turnIdx = logic.State.CurrentTurn == 'O' ? 0 : 1;
        var reader = new StreamReader(streams[turnIdx], System.Text.Encoding.UTF8);
        var line = await reader.ReadLineAsync();
        if (line == null) break;
        var clientMsg = JsonSerializer.Deserialize<ClientToServerMessage>(line);
        if (logic.State.Status == GameStatus.Playing && clientMsg.Move.HasValue)
        {
          logic.SetMove(clientMsg.Move.Value, symbols[turnIdx]);
        }
        else if (logic.State.Status == GameStatus.GameOver && clientMsg.PlayAgain.HasValue)
        {
          playAgain[turnIdx] = clientMsg.PlayAgain.Value;
          canPlayAgain[turnIdx] = true;
          // Si ambos respondieron
          if (canPlayAgain[0] && canPlayAgain[1])
          {
            if (playAgain[0] && playAgain[1])
            {
              logic.Reset();
              canPlayAgain[0] = canPlayAgain[1] = false;
            }
            else
            {
              // Avisar a ambos clientes que el juego termina definitivamente
              string finMsg = "El otro jugador no desea continuar. El servidor se cerrará.";
              foreach (var s in streams)
              {
                var buffer = System.Text.Encoding.UTF8.GetBytes(finMsg + "\n");
                await s.WriteAsync(buffer, 0, buffer.Length);
              }
              Console.WriteLine("Uno de los jugadores no desea continuar. El servidor se cerrará.");
              Environment.Exit(0);
            }
          }
        }
      }
    }
    catch (IOException)
    {
      Console.WriteLine("Uno de los jugadores se ha desconectado. El servidor se cerrará.");
      Environment.Exit(1);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error inesperado: {ex.Message}");
      Environment.Exit(1);
    }
  }
}
