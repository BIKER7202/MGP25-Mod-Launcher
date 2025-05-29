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

        public MainWindow()
        {
            string[] lcCommandArgs;

            InitializeComponent();

            oSettings = new Settings();
            cGameDir = oSettings.getSetting("gameDir");

            oHashChecker = new HashChecker(oSettings.getSetting("vanillaHash"), oSettings.getSetting("moddedHash"));

            if (oHashChecker.getVanillaHash() == "")
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
            lFolderBrowser.Description = UIStrings.cBrowserTitle;
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
                    lbValidDir = Utilities.isValidGameDirectory(lcDirectory);
                }

                // After so that the error appears if the dialog was closed instead of Okayed
                if (!lbValidDir)
                {
                    Utilities.displayError(UIStrings.cErrorDirText);
                }
            }

            Utilities.displayInformation(UIStrings.cSuccessDirText);

            cGameDir = Utilities.trimDirectory(lcDirectory);
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
                Utilities.displayError(UIStrings.cErrorPatchText);
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

        private void repatchExe_Click(object sender, EventArgs e)
        {
            patchGameExe();
        }

        private void setGameDirectory_Click(object sender, EventArgs e)
        {
            queryGameDirectory();
        }
    }
}
