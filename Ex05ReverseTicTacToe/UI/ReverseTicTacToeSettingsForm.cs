using System;
using System.Windows.Forms;

namespace Ex05.ReverseTicTacToeWindowsApp
{
    public partial class ReverseTicTacToeSettingsForm : Form
    {
        private const string k_ComputerNameLabel = "[Computer]";
        public event Action<string, string, bool, int> SettingsFilled;

        public ReverseTicTacToeSettingsForm()
        {
            InitializeComponent();
        }

        private void Player2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            const bool v_Checked = true;
            const bool v_ReadOnly = true;
            const bool v_Enabled = true;

            if (Player2CheckBox.Checked == v_Checked)
            {
                Player2NameTextBox.ReadOnly = !v_ReadOnly;
                Player2NameTextBox.Enabled = v_Enabled;
                Player2NameTextBox.Text = string.Empty;
            }
            else
            {
                Player2NameTextBox.ReadOnly = v_ReadOnly;
                Player2NameTextBox.Enabled = !v_Enabled;
                Player2NameTextBox.Text = k_ComputerNameLabel; 
            }
        }

        private void RowsUpDown_ValueChanged(object sender, EventArgs e)
        {
            ColsUpDown.Value = RowsUpDown.Value;
        }

        private void ColsUpDown_ValueChanged(object sender, EventArgs e)
        {
            RowsUpDown.Value = ColsUpDown.Value;
        }
        // $G$ CSS-013 (-5) Bad input variable name (should be in the form of i_PascalCased)
        // $G$ CSS-011 (-3) Bad private method name. Should be pascalCased.
        private void StartButton_Click(object sender, EventArgs e)
        {
            OnSettingsFilled();
        }

        protected virtual void OnSettingsFilled()
        {
            if (SettingsFilled != null)
            {
                SettingsFilled.Invoke(Player1NameTextBox.Text, Player2NameTextBox.Text,
                                      Player2CheckBox.Checked, (int)RowsUpDown.Value);
            }
        }
    }
}
