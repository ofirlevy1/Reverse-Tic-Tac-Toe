namespace Ex05.ReverseTicTacToeWindowsApp
{
    public class ReverseTicTacToeWinAppUILogicManager
    {
        private ReverseTicTacToeSettingsForm m_GameSettingsForm;
        private ReverseTicTacToeLogicManager m_Game;
        private ReverseTicTacToeForm m_GameForm;

        public void Run()
        {
            m_GameSettingsForm = new ReverseTicTacToeSettingsForm();
            m_GameSettingsForm.SettingsFilled += gameSettingsForm_SettingsFilled;
            m_GameSettingsForm.ShowDialog();
        }

        private void createGame(string i_Player1Name, string i_Player2Name,
                                bool i_IsPvP, int i_GameDimension)
        {
            string player1Name, player2Name;
            ReverseTicTacToeLogicManager.eGameMode gameMode;

            if (i_Player1Name == string.Empty)
            {
                player1Name = ReverseTicTacToeLogicManager.k_DefaultPlayer1Name;
            }
            else
            {
                player1Name = i_Player1Name;
            }

            if (i_IsPvP == false)
            {
                gameMode = ReverseTicTacToeLogicManager.eGameMode.PvC;
                player2Name = ReverseTicTacToeLogicManager.k_DefaultPCPlayerName;
            }
            else
            {
                gameMode = ReverseTicTacToeLogicManager.eGameMode.PvP;
                if (i_Player2Name == string.Empty)
                {
                    player2Name = ReverseTicTacToeLogicManager.k_DefaultPlayer2Name;
                }
                else
                {
                    player2Name = i_Player2Name;
                }
            }

            m_Game = new ReverseTicTacToeLogicManager(i_GameDimension, gameMode, player1Name, player2Name);
            m_Game.MoveMade += game_MoveMade;
            m_Game.GameOver += game_GameOver;
        }

        private void gameSettingsForm_SettingsFilled(string i_Player1Name, string i_Player2Name,
                                                     bool i_IsPvP, int i_Dimension)
        {
            m_GameSettingsForm.Dispose();
            m_GameSettingsForm.Close();
            createGame(i_Player1Name, i_Player2Name, i_IsPvP, i_Dimension);
            m_GameForm = new ReverseTicTacToeForm(m_Game.BoardSize, m_Game.Player1Name, m_Game.Player2Name);
            m_GameForm.BoardButtonClicked += gameForm_BoardButtonClicked;
            m_GameForm.UserAskedToRestartGame += gameForm_RestartGame;
            m_GameForm.ShowDialog();
        }

        private void gameForm_RestartGame()
        {
            m_Game.RestartGame();
        }

        private void game_GameOver()
        {
            switch (m_Game.GameStatus)
            {
                case ReverseTicTacToeLogicManager.eGameStatus.TieGame:
                    m_GameForm.DisplayTieAndAskUserForAnotherRound();
                    break;
                case ReverseTicTacToeLogicManager.eGameStatus.Player1Won:
                    m_GameForm.DisplayVictoryAndAskUserForAnotherRound(m_Game.GameStatus, m_Game.Player1Name);
                    break;
                case ReverseTicTacToeLogicManager.eGameStatus.Player2Won:
                    m_GameForm.DisplayVictoryAndAskUserForAnotherRound(m_Game.GameStatus, m_Game.Player2Name);
                    break;
            }
        }

        private void game_MoveMade(int i_Row, int i_Col)
        {
            m_GameForm.UpdateGameMove(i_Row - 1, i_Col - 1, m_Game.CurrentTurn);
        }

        private void gameForm_BoardButtonClicked(int i_Row, int i_Col)
        {
            ReverseTicTacToeLogicManager.eLastActionStatus actionStatus;

            m_Game.AttemptMove(i_Row, i_Col, out actionStatus);
        }
    }
}
