﻿namespace Ex05.ReverseTicTacToeWindowsApp
{
    using System;

    public class ReverseTicTacToeLogicManager
    {
        public enum eGameStatus
        {
            Ongoing,
            Player1Won,
            Player2Won,
            TieGame,
        }

        public enum eLastActionStatus
        {
            Good,
            FailedSlotTaken,
            FailedGameOver,
            FailedOutOfBounds
        }

        public enum eGameMode
        {
            PvP = 1,
            PvC = 2
        }

        public enum eTurns
        {
            Player1,
            Player2,
        }

        private enum eBoardScanningDirections
        {
            Up,
            Down,
            Right,
            Left,
            TopRight,
            TopLeft,
            BottomRight,
            BottomLeft,
        }

        public enum eSlotStatus
        {
            None, Empty, Player1, Player2
        }

        private const int k_DefaultBoardDimension = 3;
        public const string k_DefaultPlayer1Name = "Player1";
        public const string k_DefaultPlayer2Name = "Player2";
        public const string k_DefaultPCPlayerName = "Computer";
        private const eTurns k_DefaultFirstTurn = eTurns.Player1;
        public static readonly int sr_MinimumGameBoardDimension = 3;
        public static readonly int sr_MaximumGameBoardDimension = 9;

        // NOTE! -  be CAREFULL when chaning these AI Probing constants! 
        // changing the AI-probing values may result in a TREMENDOUS increase in
        // the AI running times, which most PC's wouldn't be able to handle within
        // a reasonable time frame.
        private const int k_AILargeScanningAreaDefaultProbingDepth = 2;
        private const int k_AIMediumScanningAreaDefaultProbingDepth = 3;
        private const int k_AILowScanningAreaDefaultProbingDepth = 4;
        private const int k_AILowScanningAreaMax = 10;
        private const int k_AIMediumScanningAreaMin = 10;
        private const int k_AILargeScanningAreaMin = 12;
        private const double k_AINoLossScenarioIncreaseFactor = 1.1;
        private eSlotStatus[,] m_GameBoard;
        private int m_BoardDimension, m_MovesMadeCounter;
        private eGameMode m_GameMode;
        private eTurns m_CurrentTurn;
        private eGameStatus m_GameStatus;
        private Player m_Player1, m_Player2;
        public event Action<int, int> MoveMade;
        public event Action GameOver;

        public static bool IsInRange(int i_Num, int i_Min, int i_Max)
        {

            return i_Num <= i_Max && i_Num >= i_Min;
        }

        public ReverseTicTacToeLogicManager(int i_BoardDimension, eGameMode i_GameMode, string player1Name, string player2Name)
        {
            m_BoardDimension = IsInRange(i_BoardDimension, sr_MinimumGameBoardDimension, sr_MaximumGameBoardDimension)
                                   ? i_BoardDimension
                                   : k_DefaultBoardDimension;
            m_GameMode = i_GameMode;
            m_CurrentTurn = k_DefaultFirstTurn;
            m_GameBoard = new eSlotStatus[m_BoardDimension, m_BoardDimension];
            clearBoard();
            m_Player1 = new Player(player1Name);
            m_Player2 = m_GameMode == eGameMode.PvP ? new Player(player2Name) : new Player(k_DefaultPCPlayerName);
            m_GameStatus = eGameStatus.Ongoing;
        }

        protected virtual void OnMoveMade(int i_Row, int i_Col)
        {
            if (MoveMade != null)
            {
                MoveMade.Invoke(i_Row, i_Col);
            }
        }

        protected virtual void OnGameOver()
        {
            if (GameOver != null)
            {
                GameOver.Invoke();
            }
        }

        public int BoardSize
        {
            get
            {

                return m_BoardDimension;
            }

            set
            {
                if (IsBoardClear() && IsInRange(value, ReverseTicTacToeLogicManager.sr_MinimumGameBoardDimension,
                    ReverseTicTacToeLogicManager.sr_MaximumGameBoardDimension))
                {
                    m_BoardDimension = value;
                    m_GameBoard = new eSlotStatus[m_BoardDimension, m_BoardDimension];
                    clearBoard();
                }
            }
        }

        public bool IsBoardClear()
        {

            return m_MovesMadeCounter == 0;
        }

        public bool IsSlotIndexWithinBounds(int i_Row, int i_Col)
        {

            return IsInRange(i_Row, 1, m_BoardDimension) && IsInRange(i_Col, 1, m_BoardDimension);
        }

        public bool AttemptMove(int i_Row, int i_Col, out eLastActionStatus o_MoveStatus)
        {
            o_MoveStatus = eLastActionStatus.Good;
            const bool v_MoveMadeSuccessfully = true;
            bool isMoveMadeSuccessfully = !v_MoveMadeSuccessfully;

            if (!IsSlotIndexWithinBounds(i_Row, i_Col))
            {
                o_MoveStatus = eLastActionStatus.FailedOutOfBounds;
            }
            else if (IsGameOver())
            {
                o_MoveStatus = eLastActionStatus.FailedGameOver;
            }
            else if (!IsSlotFree(i_Row, i_Col))
            {
                o_MoveStatus = eLastActionStatus.FailedSlotTaken;
            }
            else
            {
                o_MoveStatus = eLastActionStatus.Good;
                makeMove(i_Row, i_Col);
                isMoveMadeSuccessfully = v_MoveMadeSuccessfully;
                if (m_GameMode == eGameMode.PvC && !IsGameOver() && m_CurrentTurn == eTurns.Player2)
                {
                    makeComputerMove();
                }
            }

            return isMoveMadeSuccessfully;
        }

        // This functions assumes the move is legal and can be done.
        private void makeMove(int i_Row, int i_Col)
        {
            m_GameBoard[i_Row - 1, i_Col - 1] = GetCurrentPlayersSlotStatus();
            OnMoveMade(i_Row, i_Col);
            m_MovesMadeCounter++;
            if (isSlotPartOfFullSequence(i_Row, i_Col))
            {
                if (m_CurrentTurn == eTurns.Player1)
                {
                    m_GameStatus = eGameStatus.Player2Won;
                    m_Player2.Score++;
                }
                else
                {
                    m_GameStatus = eGameStatus.Player1Won;
                    m_Player1.Score++;
                }
                OnGameOver();
            }
            else if (IsBoardFull())
            {
                m_GameStatus = eGameStatus.TieGame;
                OnGameOver();
            }
            else
            {
                switchTurns();
            }
            
        }

        private void makeComputerMove()
        {
            int chosenRow, chosenCol;

            chooseSlotAI(out chosenRow, out chosenCol, getAIProbingDepth());
            makeMove(chosenRow, chosenCol);
        }

        public string getCurrentPlayerName()
        {

            return m_CurrentTurn == eTurns.Player1 ? m_Player1.Name : m_Player2.Name;
        }

        public int getAIProbingDepth()
        {
            int probeDepth = k_AILargeScanningAreaDefaultProbingDepth;
            int emptySlots = GetCurrentEmptySlotsNumber();

            if (emptySlots >= k_AILowScanningAreaMax && emptySlots < k_AILargeScanningAreaMin)
            {
                probeDepth = k_AIMediumScanningAreaDefaultProbingDepth;
            }
            else if (emptySlots < k_AILowScanningAreaMax)
            {
                probeDepth = k_AILowScanningAreaDefaultProbingDepth;
            }

            return probeDepth;
        }

        public int GetCurrentEmptySlotsNumber()
        {

            return (m_BoardDimension * m_BoardDimension) - m_MovesMadeCounter;
        }

        /*
         * General concept for AI method:
         *
         * "ProbeDepth" - is how far ahead/deep into the "future" playes we're doing the probing. for example if ProbeDepth = 3, the
         * function will check every possible scenario within 3 moves ahead.
         *
         * a "Rating" of a possible move is the AI's way to compare between it's optional moves. The higher the rating is, the "better" the move
         * is in it's eyes. And when the probing is done, the move with the highest rating will be chosen.
         *
         * The rating of a play is calculated based on two factors:
         * 1. how many "no-loss" scenarios that play leads to (each one get rating)
         * 2. how many "win scenarios" that play leads to (each one get rating)
         * 3. how many "loss" scenarios that play leads to (they get no rating (0 rating...))
         *
         * The rating given for each factor also depends on how far are we into the probe. for example -
         * if we've just started the probing proccess, the rating each scenario will get will be HIGHER
         * than if we were deeper/further in it. and it will be smaller by a factor of SQUARE ROOT. 
         *
         * In addition to the above, the rating a "No-loss" scenario will recieve a ceratin increase in it's rating
         * (defined by the constant "k_AINoLossSCenarioIncreaseFactor") - The reasoning behind that - the possible AI "win" scenarios
         * will presumably NOT be elected by the human, unless he has no other choice, but a "no-loss" scanario is a "sure thing",
         * If the AI will decide to choose it - it'll definitely happen. Thus those are favorable by it. or in other words, the AI
         * decision making, based a bit more on "in which future scenarios i WON'T LOSE?" than on "in which future scenarios will i WIN?"
         *
         * EXAMPLE:
         * if our probeDepth is 3: each current "no-loss" scenario will increase rating by 256*k_AINoLossSCenarioIncreaseFactor points, and each
         * "win" scenario will increase it by 16 points - because win scenarios are 1 level DEEPER into our probe.
         *
         * for each empty slot on the board, the AI will do the following:
         * check if playing that slot will NOT lead to immediate loss(a "no-loss" scenario). if it won't, the AI will increase
         * the rating. AND it'll also simulate playing that slot, and then, for each remaining empty slot,
         * it will simulate the rival's(human) play, and check if the rival will lose playing them. for each time the rival infact
         * loses, the AI will again increase that rate(a "win" scenario).
        *
        */
        private ulong chooseSlotAI(out int o_Row, out int o_Col, int i_ProbeDepth)
        {
            ulong overallRating = 0;
            ulong currentHighestMoveRating = 0;

            o_Row = 1;
            o_Col = 1;
            if (GetCurrentEmptySlotsNumber() != 0 && i_ProbeDepth > 0)
            {
                if (GetCurrentEmptySlotsNumber() == 1)
                {
                    getNextEmptySlotOnBoard(ref o_Row, ref o_Col);
                }
                else
                {
                    ulong individualScenarioRating = 0, currentMoveRating = 0;
                    int currentBestMoveRow = 1, currentBestMoveCol = 1;
                    int rowWinScan = 1, colWinScan = 1, currentRow = 1, currentCol = 1;
                    int rowTemp, colTemp;
                    ulong noLossScenarioRating = (ulong)(Math.Pow(2, Math.Pow(2, i_ProbeDepth)) * k_AINoLossScenarioIncreaseFactor);
                    ulong winScenarioRating = (ulong)Math.Pow(2, Math.Pow(2, i_ProbeDepth - 1));

                    for (int i = 1; i <= GetCurrentEmptySlotsNumber(); i++)
                    {
                        getNextEmptySlotOnBoard(ref currentRow, ref currentCol);
                        m_GameBoard[currentRow - 1, currentCol - 1] = eSlotStatus.Player2; // filling an empty slot
                        m_MovesMadeCounter++;

                        if (!isSlotPartOfFullSequence(currentRow, currentCol)) // if it's not leading to immediate loss
                        {
                            currentMoveRating += noLossScenarioRating; // we increase it's rating
                            overallRating += noLossScenarioRating;
                            if (i_ProbeDepth > 1)
                            {
                                for (int j = 1; j < GetCurrentEmptySlotsNumber(); j++) // now checking possible moves by the (human) player, in response to our simlated move
                                {
                                    getNextEmptySlotOnBoard(ref rowWinScan, ref colWinScan);
                                    m_GameBoard[rowWinScan - 1, colWinScan - 1] = eSlotStatus.Player1;
                                    m_MovesMadeCounter++;
                                    if (isSlotPartOfFullSequence(rowWinScan, colWinScan))
                                    {
                                        currentMoveRating += winScenarioRating; // for each possible winning scenario - increasing that slot's rating
                                        overallRating += winScenarioRating;
                                    }
                                    else
                                    {
                                        // for each computer possible move, that doesn't lead to an immediate loss, we simulate a move for the (human)
                                        // player, and if that doesn't lead to our immediate win - then we start probing RECURSIVLY.
                                        individualScenarioRating = chooseSlotAI(out rowTemp, out colTemp, i_ProbeDepth - 2);
                                        currentMoveRating += individualScenarioRating;
                                        overallRating += individualScenarioRating;
                                    }

                                    m_GameBoard[rowWinScan - 1, colWinScan - 1] = eSlotStatus.Empty; // emptying back the filled slot
                                    m_MovesMadeCounter--;
                                }
                            }
                        }

                        // now we choose the move with the highest rating:
                        if (currentMoveRating >= currentHighestMoveRating)
                        {
                            currentHighestMoveRating = currentMoveRating;
                            currentBestMoveRow = currentRow;
                            currentBestMoveCol = currentCol;
                        }

                        m_GameBoard[currentRow - 1, currentCol - 1] = eSlotStatus.Empty; // emptying back the filled slot
                        m_MovesMadeCounter--;
                        currentMoveRating = 0;
                    }

                    o_Row = currentBestMoveRow;
                    o_Col = currentBestMoveCol;
                }
            }

            return overallRating;
        }

        private bool getNextEmptySlotOnBoard(ref int io_Row, ref int io_Col)
        {
            int slotScannedCounter = 0;
            const bool v_EmptySlotFound = true;
            bool isEmptySlotFound = !v_EmptySlotFound;

            if (io_Col != m_BoardDimension)
            {
                io_Col++;
            }
            else
            {
                io_Col = 1;
                io_Row = (io_Row == m_BoardDimension) ? 1 : io_Row + 1;
            }

            if (!IsBoardFull())
            {
                while (slotScannedCounter <= (m_BoardDimension * m_BoardDimension) && !isEmptySlotFound)
                {
                    while (slotScannedCounter <= (m_BoardDimension * m_BoardDimension) && !isEmptySlotFound && io_Col <= m_BoardDimension)
                    {
                        if (m_GameBoard[io_Row - 1, io_Col - 1] == eSlotStatus.Empty)
                        {
                            isEmptySlotFound = v_EmptySlotFound;
                        }
                        else
                        {
                            slotScannedCounter++;
                            io_Col++;
                        }
                    }

                    if (!isEmptySlotFound)
                    {
                        io_Col = 1;
                        io_Row = (io_Row == m_BoardDimension) ? 1 : io_Row + 1;
                    }
                }
            }

            return isEmptySlotFound;
        }

        private eSlotStatus[,] cloneGameBoard()
        {
            eSlotStatus[,] gameBoardCopy = new eSlotStatus[m_BoardDimension, m_BoardDimension];

            for (int i = 0; i < m_BoardDimension; i++)
            {
                for (int j = 0; j < m_BoardDimension; j++)
                {
                    gameBoardCopy[i, j] = m_GameBoard[i, j];
                }
            }

            return gameBoardCopy;
        }

        public void RestartGame()
        {
            clearBoard();
            m_CurrentTurn = k_DefaultFirstTurn;
            m_GameStatus = eGameStatus.Ongoing;
        }

        public void ClearScores()
        {
            m_Player1.Score = 0;
            m_Player2.Score = 0;
        }

        public void RestartGameAndClearScores()
        {
            RestartGame();
            ClearScores();
        }

        public bool Forfeit(out eLastActionStatus o_ActionStatus)
        {
            const bool v_ForfeitSuccessful = true;
            bool isForfeitSuccessful = !v_ForfeitSuccessful;

            if (IsGameOver())
            {
                o_ActionStatus = eLastActionStatus.FailedGameOver;
            }
            else
            {
                switch (m_CurrentTurn)
                {
                    case eTurns.Player1:
                        m_Player2.Score++;
                        m_GameStatus = eGameStatus.Player2Won;
                        break;
                    case eTurns.Player2:
                        m_Player1.Score++;
                        m_GameStatus = eGameStatus.Player1Won;
                        break;
                }

                o_ActionStatus = eLastActionStatus.Good;
                isForfeitSuccessful = v_ForfeitSuccessful;
            }

            return isForfeitSuccessful;
        }

        public eSlotStatus GetCurrentPlayersSlotStatus()
        {
            eSlotStatus currentPlayerChar;

            if (m_CurrentTurn == eTurns.Player1)
            {
                currentPlayerChar = eSlotStatus.Player1;
            }
            else
            {
                currentPlayerChar = eSlotStatus.Player2;
            }

            return currentPlayerChar;
        }

        public bool IsBoardFull()
        {

            return m_MovesMadeCounter == m_BoardDimension * m_BoardDimension;
        }

        // This function gets a row and a col indexes and checks if by adding this move to the board
        // will complete a winning sequence.
        private bool isSlotPartOfFullSequence(int i_Row, int i_Col)
        {
            const bool v_SequenceFound = true;
            bool isPartOfFullSequence = !v_SequenceFound;
            int verticalSequence = 1 + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.Up)
                                       + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.Down);
            int horizontalSequence = 1 + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.Right)
                                       + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.Left);
            int declinedDiagonalSequence = 1 + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.TopLeft)
                                       + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.BottomRight);
            int inclinedDiagonalSequence = 1 + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.BottomLeft)
                                       + SequenceSizeFromSlotToDirection(i_Row, i_Col, eBoardScanningDirections.TopRight);

            if (verticalSequence == m_BoardDimension || horizontalSequence == m_BoardDimension
                                                    || declinedDiagonalSequence == m_BoardDimension
                                                    || inclinedDiagonalSequence == m_BoardDimension)
            {
                isPartOfFullSequence = v_SequenceFound;
            }

            return isPartOfFullSequence;
        }

        // This function will scan the board, STARTING from the given slot(i_Row, i_Col), and TOWARDS the given direction.
        // It will return how many EQUAL SLOTS (to the starting slot) there are in that direction - NOT including the original slot.
        private int SequenceSizeFromSlotToDirection(int i_Row, int i_Col, eBoardScanningDirections i_ScanningDirection)
        {
            const bool v_insideSequence = true;
            bool isInsideSequence = v_insideSequence;
            int sequenceSize = 0;
            int nextRow = i_Row, nextCol = i_Col;

            advanceToNextSlotInGivenDirection(ref nextRow, ref nextCol, i_ScanningDirection);
            while (IsSlotIndexWithinBounds(nextRow, nextCol) && isInsideSequence)
            {
                if (m_GameBoard[i_Row - 1, i_Col - 1] == m_GameBoard[nextRow - 1, nextCol - 1])
                {
                    sequenceSize++;
                    advanceToNextSlotInGivenDirection(ref nextRow, ref nextCol, i_ScanningDirection);
                }
                else
                {
                    isInsideSequence = !v_insideSequence;
                }
            }

            return sequenceSize;
        }

        // NOTE - This function does NOT check for out of bounds when advancing the indexes.
        private void advanceToNextSlotInGivenDirection(ref int io_Row, ref int io_Col,
            eBoardScanningDirections i_Direction)
        {
            switch (i_Direction)
            {
                case eBoardScanningDirections.Up:
                    io_Row--;
                    break;
                case eBoardScanningDirections.TopRight:
                    io_Row--;
                    io_Col++;
                    break;
                case eBoardScanningDirections.BottomLeft:
                    io_Row++;
                    io_Col--;
                    break;
                case eBoardScanningDirections.BottomRight:
                    io_Row++;
                    io_Col++;
                    break;
                case eBoardScanningDirections.Down:
                    io_Row++;
                    break;
                case eBoardScanningDirections.Left:
                    io_Col--;
                    break;
                case eBoardScanningDirections.Right:
                    io_Col++;
                    break;
                case eBoardScanningDirections.TopLeft:
                    io_Row--;
                    io_Col--;
                    break;
            }
        }

        private void switchTurns()
        {
            m_CurrentTurn = (m_CurrentTurn == eTurns.Player1) ? eTurns.Player2 : eTurns.Player1;
        }

        public bool IsGameOver()
        {

            return m_GameStatus == eGameStatus.Player1Won || m_GameStatus == eGameStatus.Player2Won
                                                           || m_GameStatus == eGameStatus.TieGame;
        }

        public bool IsSlotFree(int i_Row, int i_Col)
        {
            const bool v_SlotFree = true;
            bool isSlotFree;

            if (!IsSlotIndexWithinBounds(i_Row, i_Col))
            {
                isSlotFree = !v_SlotFree;
            }
            else
            {
                isSlotFree = m_GameBoard[i_Row - 1, i_Col - 1] == eSlotStatus.Empty;
            }

            return isSlotFree;
        }

        public bool GetSlotStatusByIndex(int i_Row, int i_Col, out eSlotStatus o_Status)
        {
            const bool v_Successful = true;
            bool isSuccessful = !v_Successful;

            if (!IsSlotIndexWithinBounds(i_Row, i_Col))
            {
                o_Status = eSlotStatus.None;
            }
            else
            {
                o_Status = m_GameBoard[i_Row - 1, i_Col - 1];
                isSuccessful = v_Successful;
            }

            return isSuccessful;
        }

        private void clearBoard()
        {
            for (int i = 0; i < m_BoardDimension; i++)
            {
                for (int j = 0; j < m_BoardDimension; j++)
                {
                    m_GameBoard[i, j] = eSlotStatus.Empty;
                }
            }

            m_MovesMadeCounter = 0;
        }

        public int Player1Score
        {
            get
            {

                return m_Player1.Score;
            }

            set
            {
                if (value > 0)
                {
                    m_Player1.Score = value;
                }
            }
        }

        public int Player2Score
        {
            get
            {

                return m_Player2.Score;
            }

            set
            {
                if (value >= 0)
                {
                    m_Player2.Score = value;
                }
            }
        }

        public string Player1Name
        {
            get
            {

                return m_Player1.Name;
            }
        }

        public string Player2Name
        {
            get
            {

                return m_Player2.Name;
            }
        }

        public eGameMode GameMode
        {
            get
            {

                return m_GameMode;
            }

            set
            {
                if (m_GameStatus != eGameStatus.Ongoing && value != m_GameMode)
                {
                    RestartGame();
                    m_GameMode = value;
                    if (m_GameMode == eGameMode.PvC)
                    {
                        m_Player2 = new Player(k_DefaultPCPlayerName);
                    }
                }
            }
        }

        public eTurns CurrentTurn
        {
            get
            {

                return m_CurrentTurn;
            }

            set
            {
                m_CurrentTurn = value;
            }
        }

        public eGameStatus GameStatus
        {
            get
            {

                return m_GameStatus;
            }
        }

        public class Player
        {
            private readonly string r_Name;
            private int m_Score;

            public Player(string i_Name)
            {
                r_Name = new string(i_Name.ToCharArray());
                m_Score = 0;
            }

            public string Name
            {
                get
                {

                    return r_Name;
                }
            }

            public int Score
            {
                get
                {

                    return m_Score;
                }

                set
                {
                    if (value >= 0)
                    {
                        m_Score = value;
                    }
                }
            }
        }
    }
}
