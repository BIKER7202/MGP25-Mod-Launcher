using System.IO;
using System.Resources;
using System.Runtime;
using System.Windows.Forms;

namespace MGP_25_Mod_Launcher
{
    public enum LaunchType
    {
        None,
        Modded,
        Vanilla
    }

    public partial class MainWindow : Form
    {
        private string cGameDir;
        private Settings oSettings;
        private HashChecker oHashChecker;
        private string cCurrentGame;
        private LaunchType oLaunchType;

        public MainWindow()
        {
            bool lbUsingOldMethod = false;

            parseCommandLineArgs();

            oSettings = new Settings();

            if (oLaunchType == LaunchType.None)
            {
                cCurrentGame = oSettings.getSetting("selectedGame");
            }

            // This should mean that user is upgrading from older launcher version, so we need to unpatch the exe and update the names of the settings
            lbUsingOldMethod = (oSettings.getSetting("patchedExe") != "");

            // Always default to MotoGP 25 if no game is selected
            if (string.IsNullOrEmpty(cCurrentGame))
            {
                cCurrentGame = DirConstants.cSupportedGames[0];
            }

            if (lbUsingOldMethod)
            {
                updateToNewMethod();
            }

            oSettings.setGame(cCurrentGame);
            cGameDir = oSettings.getSetting("gameDir");
            oHashChecker = new HashChecker(oSettings.getSetting("vanillaHash"), oSettings.getSetting("moddedHash"));

            if (oLaunchType == LaunchType.Modded)
            {
                GameLauncher.launchModdedGame(cGameDir);
            }
            else if (oLaunchType == LaunchType.Vanilla)
            {
                GameLauncher.launchVanillaGame(cGameDir);
            }

            InitializeComponent();
        }

        private void MainWindow_Load(object sender, System.EventArgs e)
        {
            if (!oSettings.getDoesConfigExist())
            {
                // Welcome message loaded on first launch
                Utilities.displayInformation(UIStrings.cWelcomeText, UIStrings.cWelcomeTitle);

                queryGameDirectory();
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

        private void setSelectedGameDropDown()
        {
            int liIndex = 0;
            
            if (!string.IsNullOrEmpty(cCurrentGame))
            {
                liIndex = Array.IndexOf(DirConstants.cSupportedGames, cCurrentGame);
                if (liIndex < 0) liIndex = 0;
            }

            selectGameDropdown.SelectedIndex = liIndex;
        }

        private void updateToNewMethod()
        {
            string lcDir = oSettings.getSetting("gameDir");

            HashChecker loHashChecker = new HashChecker(oSettings.getSetting("vanillaHash"), oSettings.getSetting("moddedHash"));

            // Pre hash version, so even older
            if (oHashChecker.getVanillaHash() == "" && oSettings.getSetting("patchedExe") == "true")
            {
                storeChecksums();
            }

            // If we didn't know the hash then it's a vanilla update so can ignore
            if (oHashChecker.isGameExeHashKnown(lcDir))
            {
                // Copy vanilla exe over the game one as we don't patch exe anymore
                File.Copy(DirConstants.cVanillaDir + DirConstants.cExeName, cGameDir + DirConstants.cExeDir, true);
            }
            
            oSettings.clearSettings();
            oSettings.setGame("MotoGP 25");
            oSettings.setSetting("gameDir", lcDir);
        }

        private void parseCommandLineArgs()
        {
            string[] lcCommandArgs = Environment.GetCommandLineArgs();
            int li;

            oLaunchType = LaunchType.None;

            foreach (string lcArgument in lcCommandArgs)
            {
                for (li = 0; li < DirConstants.cSupportedGames.Length; li++)
                {
                    if (lcArgument == "-" + DirConstants.cSupportedGames[li].Replace(" ", ""))
                    {
                        cCurrentGame = DirConstants.cSupportedGames[li];
                        break;
                    }
                }

                if (lcArgument == "-LaunchModded")
                {
                    oLaunchType = LaunchType.Modded;
                }
                else if (lcArgument == "-LaunchDefault")
                {
                    oLaunchType = LaunchType.Vanilla;
                }
            }
        }

        private void createShortcuts_Click(object sender, EventArgs e)
        {
            Utilities.addShortcutToDesktop(cGameDir, 0);
            Utilities.addShortcutToDesktop(cGameDir, 1);
        }

        private void selectGameDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            cCurrentGame = DirConstants.cSupportedGames[selectGameDropdown.SelectedIndex];
            oSettings.setGame(cCurrentGame);
            oSettings.setSetting("selectedGame", cCurrentGame);
            cGameDir = oSettings.getSetting("gameDir");
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
