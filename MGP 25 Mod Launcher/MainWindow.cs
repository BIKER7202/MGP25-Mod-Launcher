using System.IO;
using System.Resources;
using System.Runtime;
using System.Windows.Forms;

namespace MGP_25_Mod_Launcher
{
    public partial class MainWindow : Form
    {
        private string cGameDir;
        private Settings oSettings;
        private HashChecker oHashChecker;
        private string cCurrentGame;

        public MainWindow()
        {
            string[] lcCommandArgs;
            string lcSelected;
            int liIndex = 0;

            InitializeComponent();

            oSettings = new Settings();
            cGameDir = oSettings.getSetting("gameDir");

            // Populate the Select Game dropdown from DirConstants and set selected item from settings
            selectGameDropdown.Items.Clear();
            selectGameDropdown.Items.AddRange(DirConstants.cSupportedGames);

            lcSelected = oSettings.getSetting("selectedGame");
            if (!string.IsNullOrEmpty(lcSelected))
            {
                liIndex = Array.IndexOf(DirConstants.cSupportedGames, lcSelected);
                if (liIndex < 0) liIndex = 0;
            }

            selectGameDropdown.SelectedIndex = liIndex;
            cCurrentGame = DirConstants.cSupportedGames[selectGameDropdown.SelectedIndex];

            oHashChecker = new HashChecker(oSettings.getSetting("vanillaHash"), oSettings.getSetting("moddedHash"));

            if (oHashChecker.getVanillaHash() == "" && oSettings.getSetting("patchedExe") == "true")
            {
                storeChecksums();
            }

            lcCommandArgs = Environment.GetCommandLineArgs();

            foreach (string lcArgument in lcCommandArgs)
            {
                if (lcArgument == "-LaunchModded")
                {
                    if (!oHashChecker.isGameExeHashKnown(cGameDir))
                    {
                        Utilities.displayInformation(UIStrings.cUpdateDetectedText, UIStrings.cInfoTitle);
                        patchGameExe();
                    }

                    GameLauncher.launchModdedGame(cGameDir);
                }
                else if (lcArgument == "-LaunchDefault")
                {
                    if (!oHashChecker.isGameExeHashKnown(cGameDir))
                    {
                        Utilities.displayInformation(UIStrings.cUpdateDetectedText, UIStrings.cInfoTitle);
                        patchGameExe();
                    }

                    GameLauncher.launchVanillaGame(cGameDir);
                }
            }
        }

        private void MainWindow_Load(object sender, System.EventArgs e)
        {
            if (cGameDir == "")
            {
                // Welcome message loaded on first launch
                Utilities.displayInformation(UIStrings.cWelcomeText, UIStrings.cWelcomeTitle);

                queryGameDirectory();
            }

            if (oSettings.getSetting("patchedExe") != "true")
            {
                patchGameExe();
            }

            // Users were having issues when game updates were applied, now hashing Exe for detecting updates
            if (oSettings.getSetting("storedChecksums") != "true")
            {
                storeChecksums();
            }
        }

        private void queryGameDirectory()
        {
            bool lbValidDir = false;
            string lcDirectory = "";
            FolderBrowserDialog lFolderBrowser;
            DialogResult lDialogResult;

            lFolderBrowser = new FolderBrowserDialog();
            lFolderBrowser.Description = UIStrings.cBrowserTitle.Replace("&1", cCurrentGame);
            lFolderBrowser.UseDescriptionForTitle = true;

            if (Directory.Exists(DirConstants.cDefaultGameDir))
            {
                lFolderBrowser.SelectedPath = DirConstants.cDefaultGameDir;
            }

            while (!lbValidDir)
            {
                lDialogResult = lFolderBrowser.ShowDialog();
                lcDirectory = lFolderBrowser.SelectedPath;

                if (lDialogResult == DialogResult.OK)
                {
                    lcDirectory = Utilities.returnValidGameDirectory(lcDirectory);
                    lbValidDir = (lcDirectory != "");
                }

                // Prevents loop that would previously occur when cancelling
                else if (lDialogResult == DialogResult.Cancel)
                {
                    if(cGameDir != "")
                    {
                        return;
                    }
                    else
                    {
                        Utilities.displayError(UIStrings.cErrorDirExitText.Replace("&1", cCurrentGame));
                        Environment.Exit(0);
                    }
                }

                // After so that the error appears if the dialog was closed instead of Okayed
                if (!lbValidDir)
                {
                    Utilities.displayError(UIStrings.cErrorDirText.Replace("&1", cCurrentGame));
                }
            }

            Utilities.displayInformation(UIStrings.cSuccessDirText.Replace("&1", cCurrentGame));

            cGameDir = lcDirectory;
            oSettings.setSetting("gameDir", cGameDir);
        }

        private void patchGameExe()
        {
            if (oHashChecker.isGameExeHashKnown(cGameDir))
            {
                Utilities.displayInformation(UIStrings.cGameAlreadyPatched, UIStrings.cInfoTitle);
                return;
            }

            if (Patcher.patchGame(cGameDir))
            {
                Utilities.displayInformation(UIStrings.cSuccessPatchText);
                oSettings.setSetting("patchedExe", "true");
            }
            else
            {
                Utilities.displayError(UIStrings.cErrorPatchText.Replace("&1", cCurrentGame));
                Application.Exit();
            }

            storeChecksums();
        }

        private void storeChecksums()
        {
            oHashChecker.checkBackedUpExeHashes();
            oSettings.setSetting("vanillaHash", oHashChecker.getVanillaHash());
            oSettings.setSetting("moddedHash", oHashChecker.getModdedHash());
            oSettings.setSetting("storedChecksums", "true");
        }

        private void createShortcuts_Click(object sender, EventArgs e)
        {
            Utilities.addShortcutToDesktop(cGameDir, 0);
            Utilities.addShortcutToDesktop(cGameDir, 1);
        }

        private void selectGameDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lcSelected = selectGameDropdown.SelectedItem as string;
            if (!string.IsNullOrEmpty(lcSelected))
            {
                oSettings.setSetting("selectedGame", lcSelected);
                cCurrentGame = lcSelected;
            }
        }

        private void launchModdedGame_Click(object sender, EventArgs e)
        {
            if (!oHashChecker.isGameExeHashKnown(cGameDir))
            {
                Utilities.displayInformation(UIStrings.cUpdateDetectedText, UIStrings.cInfoTitle);
                patchGameExe();
            }

            GameLauncher.launchModdedGame(cGameDir);
            Utilities.displayError(UIStrings.cErrorLaunchText);
        }

        private void launchVanillaGame_Click(object sender, EventArgs e)
        {
            if (!oHashChecker.isGameExeHashKnown(cGameDir))
            {
                Utilities.displayInformation(UIStrings.cUpdateDetectedText, UIStrings.cInfoTitle);
                patchGameExe();
            }

            GameLauncher.launchVanillaGame(cGameDir);
            Utilities.displayError(UIStrings.cErrorLaunchText);
        }



        private void setGameDirectory_Click(object sender, EventArgs e)
        {
            queryGameDirectory();
        }
    }
}
