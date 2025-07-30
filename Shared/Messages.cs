namespace TicTacToe.Shared;

public enum GameStatus { Playing, GameOver }

public class GameState
{
    public char[] Board { get; set; } = new char[9];
    public GameStatus Status { get; set; }
    public char CurrentTurn { get; set; }
    public int PlayerOWins { get; set; }
    public int PlayerXWins { get; set; }
    public int Ties { get; set; }
}

public class ServerToClientMessage
{
    public GameState State { get; set; } = new GameState();
    public char YourSymbol { get; set; }
    public bool YourTurn { get; set; }
    public bool CanPlayAgain { get; set; }
}

public class ClientToServerMessage
{
    public int? Move { get; set; } // null si no es jugada
    public bool? PlayAgain { get; set; } // null si no responde aún
}
