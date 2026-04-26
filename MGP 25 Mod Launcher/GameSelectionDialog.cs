using System.Windows.Forms;

namespace MGP_25_Mod_Launcher
{
    public partial class GameSelectionDialog : Form
    {
        public string cSelectedGame { get; private set; }

        public GameSelectionDialog()
        {
            InitializeComponent();
            cSelectedGame = DirConstants.cSupportedGames[0];
        }

        private void InitializeComponent()
        {
            this.Text = UIStrings.cSelectGameTitle;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(450, 180);
            this.DialogResult = DialogResult.None;

            Label instructionLabel = new Label();
            instructionLabel.Text = UIStrings.cSelectGameText;
            instructionLabel.Location = new System.Drawing.Point(20, 20);
            instructionLabel.Size = new System.Drawing.Size(400, 30);
            instructionLabel.AutoSize = true;
            this.Controls.Add(instructionLabel);

            ComboBox gameComboBox = new ComboBox();
            gameComboBox.Name = "gameComboBox";
            gameComboBox.Location = new System.Drawing.Point(20, 60);
            gameComboBox.Size = new System.Drawing.Size(400, 25);
            gameComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            gameComboBox.Items.Clear();
            gameComboBox.Items.AddRange(DirConstants.cSupportedGames);
            gameComboBox.SelectedIndex = 0;
            this.Controls.Add(gameComboBox);

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new System.Drawing.Point(185, 110);
            okButton.Size = new System.Drawing.Size(75, 25);
            okButton.DialogResult = DialogResult.OK;
            okButton.Click += (sender, e) => {
                cSelectedGame = gameComboBox.SelectedItem.ToString();
                this.Close();
            };
            this.Controls.Add(okButton);

            this.AcceptButton = okButton;
        }
    }
}
