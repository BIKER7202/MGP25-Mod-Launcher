namespace MGP_25_Mod_Launcher
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            createShortcuts = new Button();
            repatchExe = new Button();
            setGameDirectory = new Button();
            launchModdedGame = new Button();
            launchVanillaGame = new Button();
            SuspendLayout();
            // 
            // createShortcuts
            // 
            createShortcuts.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            createShortcuts.Location = new Point(51, 153);
            createShortcuts.Name = "createShortcuts";
            createShortcuts.Size = new Size(311, 65);
            createShortcuts.TabIndex = 0;
            createShortcuts.Text = "Create Desktop Shortcuts";
            createShortcuts.UseVisualStyleBackColor = true;
            createShortcuts.Click += createShortcuts_Click;
            // 
            // repatchExe
            // 
            repatchExe.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            repatchExe.Location = new Point(51, 238);
            repatchExe.Name = "repatchExe";
            repatchExe.Size = new Size(311, 65);
            repatchExe.TabIndex = 1;
            repatchExe.Text = "Repatch Exe";
            repatchExe.UseVisualStyleBackColor = true;
            repatchExe.Click += repatchExe_Click;
            // 
            // setGameDirectory
            // 
            setGameDirectory.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            setGameDirectory.Location = new Point(51, 323);
            setGameDirectory.Name = "setGameDirectory";
            setGameDirectory.Size = new Size(311, 65);
            setGameDirectory.TabIndex = 2;
            setGameDirectory.Text = "Change Game Directory";
            setGameDirectory.UseVisualStyleBackColor = true;
            setGameDirectory.Click += setGameDirectory_Click;
            // 
            // launchModdedGame
            // 
            launchModdedGame.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            launchModdedGame.Location = new Point(51, 408);
            launchModdedGame.Name = "launchModdedGame";
            launchModdedGame.Size = new Size(311, 65);
            launchModdedGame.TabIndex = 4;
            launchModdedGame.Text = "Launch Game With Mods";
            launchModdedGame.UseVisualStyleBackColor = true;
            launchModdedGame.Click += launchModdedGame_Click;
            // 
            // launchVanillaGame
            // 
            launchVanillaGame.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            launchVanillaGame.Location = new Point(51, 493);
            launchVanillaGame.Name = "launchVanillaGame";
            launchVanillaGame.Size = new Size(311, 65);
            launchVanillaGame.TabIndex = 3;
            launchVanillaGame.Text = "Launch Game Without Mods";
            launchVanillaGame.UseVisualStyleBackColor = true;
            launchVanillaGame.Click += launchVanillaGame_Click;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.WindowBackdrop;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(794, 591);
            Controls.Add(setGameDirectory);
            Controls.Add(createShortcuts);
            Controls.Add(repatchExe);
            Controls.Add(launchModdedGame);
            Controls.Add(launchVanillaGame);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "BIKER's MotoGP 25 Mod Launcher";
            Load += MainWindow_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button createShortcuts;
        private Button setGameDirectory;
        private Button repatchExe;
        private Button launchModdedGame;
        private Button launchVanillaGame;
    }
}
