using System.Windows.Forms;

namespace Ex05.ReverseTicTacToeWindowsApp
{
    public class ReverseTicTacToeBoardButton : Button
    {
        private int m_Row, m_Col;

        public int RowIndex
        {
            get
            {

                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int ColIndex
        {
            get
            {

                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }
    }
}
