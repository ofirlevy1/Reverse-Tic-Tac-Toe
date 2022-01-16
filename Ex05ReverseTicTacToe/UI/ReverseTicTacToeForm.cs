using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace Ex05.ReverseTicTacToeWindowsApp
{
    public class ReverseTicTacToeForm : Form
    {
        private const string k_FormTitle = "TicTacToeMisere";
        private const string k_FormName = "TicTacToeForm";
        private const char k_Player1Char = 'X';
        private const char k_Player2Char = 'O';
        private const int k_SpaceBetweenButtons = 6;
        private const int k_ButtonSize = 50;
        private const int k_ButtonsStartingXPos = 15;
        private const int k_ButtonsStartingYPos = 10;
        private const int k_GameWindowWidthExtension = 40;
        private const int k_GameWindowHeightExtension = 100;
        private const int k_LabelBottomPadding = 75;
        private const int k_PaddingBetweenLabels = 15;
        private readonly string r_Player1Name;
        private readonly string r_Player2Name;
        private readonly int r_BoardDimension;
        private int m_Player1Score;
        private int m_Player2Score;
        private ReverseTicTacToeBoardButton[,] m_GameButtons;
        private Label m_Player1NameAndScoreLabel;
        private Label m_Player2NameAndScoreLabel;
        public event Action<int, int> BoardButtonClicked;
        public event Action UserAskedToRestartGame;

        public ReverseTicTacToeForm(int i_GameDimensions, string i_Player1Name, string i_Player2Name)
        {
            m_Player1Score = 0;
            m_Player2Score = 0;
            r_Player1Name = i_Player1Name;
            r_Player2Name = i_Player2Name;
            r_BoardDimension = i_GameDimensions;
            Text = k_FormTitle;
            Name = k_FormName;
            gameInit();
        }

        private void gameInit()
        {
            const bool v_MaximizeEnabled = true;
            const bool v_MinimizedEnabled = true;
            int blocksLength = (k_ButtonSize + k_SpaceBetweenButtons) * r_BoardDimension;

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = !v_MaximizeEnabled;
            MinimizeBox = !v_MinimizedEnabled;
            m_GameButtons = new ReverseTicTacToeBoardButton[r_BoardDimension, r_BoardDimension];
            Width = blocksLength + k_GameWindowWidthExtension;
            Height = blocksLength + k_GameWindowHeightExtension;
            m_Player1NameAndScoreLabel = new Label();
            m_Player2NameAndScoreLabel = new Label();
            Controls.Add(m_Player1NameAndScoreLabel);
            Controls.Add(m_Player2NameAndScoreLabel);
            initGameLabels();
            initGameButtons();  
        }

        private void initGameLabels()
        {
            const bool v_AutoSize = true;
            int labelsYPos = Height - k_LabelBottomPadding;
            int labelsWidth;

            m_Player1NameAndScoreLabel.AutoSize = v_AutoSize;
            m_Player2NameAndScoreLabel.AutoSize = v_AutoSize;
            updateNameAndScoreLabel(m_Player1NameAndScoreLabel, r_Player1Name, m_Player1Score);
            updateNameAndScoreLabel(m_Player2NameAndScoreLabel, r_Player2Name, m_Player2Score);
            m_Player1NameAndScoreLabel.Font = new Font(m_Player1NameAndScoreLabel.Font, FontStyle.Bold);
            m_Player1NameAndScoreLabel.Top = labelsYPos;
            m_Player2NameAndScoreLabel.Top = labelsYPos;
            labelsWidth = m_Player1NameAndScoreLabel.Width + m_Player2NameAndScoreLabel.Width + k_PaddingBetweenLabels;
            m_Player1NameAndScoreLabel.Left = (ClientSize.Width / 2) - (labelsWidth / 2);
            m_Player2NameAndScoreLabel.Left = m_Player1NameAndScoreLabel.Left + m_Player1NameAndScoreLabel.Width + k_PaddingBetweenLabels;
        }

        private void initGameButtons()
        {
            const bool v_TabStop = true;
            ReverseTicTacToeBoardButton gameButton;

            for (int i = 0; i < r_BoardDimension; i++)
            {
                for (int j = 0; j < r_BoardDimension; j++)
                {
                    gameButton = new ReverseTicTacToeBoardButton();
                    gameButton.RowIndex = i;
                    gameButton.ColIndex = j;
                    gameButton.TabStop = !v_TabStop;
                    gameButton.Width = gameButton.Height = k_ButtonSize;
                    gameButton.Location = new Point((j * (gameButton.Width + k_SpaceBetweenButtons)) + k_ButtonsStartingXPos,
                                                             (i * (gameButton.Height + k_SpaceBetweenButtons)) + k_ButtonsStartingYPos);
                    gameButton.Click += gameButton_Click;
                    Controls.Add(gameButton);
                    m_GameButtons[i, j] = gameButton;
                }
            }
        }

        private void gameButton_Click(object sender, EventArgs e)
        {
            ReverseTicTacToeBoardButton clickedButton = sender as ReverseTicTacToeBoardButton;

            OnBoardButtonClicked(clickedButton.RowIndex + 1, clickedButton.ColIndex + 1);
        }

        private void resetUI()
        {
            const bool v_Enabled = true;

            foreach (Button button in m_GameButtons)
            {
                button.Text = string.Empty;
                button.Enabled = v_Enabled;
            }

            m_Player1NameAndScoreLabel.Font = new Font(m_Player1NameAndScoreLabel.Font, FontStyle.Bold);
            m_Player2NameAndScoreLabel.Font = new Font(m_Player2NameAndScoreLabel.Font, FontStyle.Regular);
        }

        public void UpdateGameMove(int i_RowIndex, int i_ColIndex, ReverseTicTacToeLogicManager.eTurns i_CurrentTurn)
        {
            const bool v_Enabled = true;
            char playerChar = i_CurrentTurn == ReverseTicTacToeLogicManager.eTurns.Player2 ? k_Player2Char : k_Player1Char;

            switchPlayerLabelStyle(i_CurrentTurn);
            m_GameButtons[i_RowIndex, i_ColIndex].Text = playerChar.ToString();
            m_GameButtons[i_RowIndex, i_ColIndex].Enabled = !v_Enabled;
        }

        private void switchPlayerLabelStyle(ReverseTicTacToeLogicManager.eTurns i_CurrentTurn)
        {
            switch (i_CurrentTurn)
            {
                case ReverseTicTacToeLogicManager.eTurns.Player1:
                    m_Player2NameAndScoreLabel.Font = new Font(m_Player2NameAndScoreLabel.Font, FontStyle.Bold);
                    m_Player1NameAndScoreLabel.Font = new Font(m_Player1NameAndScoreLabel.Font, FontStyle.Regular);
                    break;
                case ReverseTicTacToeLogicManager.eTurns.Player2:
                    m_Player2NameAndScoreLabel.Font = new Font(m_Player2NameAndScoreLabel.Font, FontStyle.Regular);
                    m_Player1NameAndScoreLabel.Font = new Font(m_Player1NameAndScoreLabel.Font, FontStyle.Bold);
                    break;
            }
        }

        protected virtual void OnBoardButtonClicked(int i_Row, int i_Col)
        {
            if (BoardButtonClicked != null)
            {
                BoardButtonClicked.Invoke(i_Row, i_Col);
            }
        }

        public void DisplayVictoryAndAskUserForAnotherRound(ReverseTicTacToeLogicManager.eGameStatus i_GameStatus, string i_WinnerName)
        {
            string winnerMsg = string.Format("{0} Won!", i_WinnerName);

            runEndGameForm(winnerMsg, "A Win!", i_GameStatus);
        }

        public void DisplayTieAndAskUserForAnotherRound()
        {
            string tieMsg = "It's a tie!";

            runEndGameForm(tieMsg, "A Tie!", ReverseTicTacToeLogicManager.eGameStatus.TieGame);
        }

        private void runEndGameForm(string i_endGameStatusMessage, string i_Title, ReverseTicTacToeLogicManager.eGameStatus i_GameStatus)
        {
            StringBuilder userMsg = new StringBuilder(i_endGameStatusMessage);
            
            userMsg.AppendFormat("{0}Would you like to play another round?", Environment.NewLine);
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(userMsg.ToString(), i_Title, buttons);

            if (result == DialogResult.Yes)
            {
                resetUI();
                if (i_GameStatus != ReverseTicTacToeLogicManager.eGameStatus.TieGame)
                {
                    increaseWinnersScore(i_GameStatus);
                }

                OnUserAskedToRestartGame();
            }
            else
            {
                Dispose();
                Close();
            }
        }

        protected virtual void OnUserAskedToRestartGame()
        {
            if (UserAskedToRestartGame != null)
            {
                UserAskedToRestartGame.Invoke();
            }
        }

        private void increaseWinnersScore(ReverseTicTacToeLogicManager.eGameStatus i_GameStatus)
        {
            switch (i_GameStatus)
            {
                case ReverseTicTacToeLogicManager.eGameStatus.Player1Won:
                    m_Player1Score++;
                    updateNameAndScoreLabel(m_Player1NameAndScoreLabel, r_Player1Name, m_Player1Score);
                    break;
                case ReverseTicTacToeLogicManager.eGameStatus.Player2Won:
                    m_Player2Score++;
                    updateNameAndScoreLabel(m_Player2NameAndScoreLabel, r_Player2Name, m_Player2Score);
                    break;
            }
        }

        private void updateNameAndScoreLabel(Label i_NameAndScoreLabel, string i_PlayerName, int i_PlayersScore)
        {
            i_NameAndScoreLabel.Text = string.Format("{0}: {1}", i_PlayerName, i_PlayersScore);
        }
    }
}
