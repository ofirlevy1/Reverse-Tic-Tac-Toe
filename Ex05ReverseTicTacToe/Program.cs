namespace Ex05.ReverseTicTacToeWindowsApp
{
    /// <summary>
    /// This program runs a Windows forms based Reverse Tic Tac Toe Game
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            ReverseTicTacToeWinAppUILogicManager gameUIManager = new ReverseTicTacToeWinAppUILogicManager();
            gameUIManager.Run();
        }
    }
}
