using IWshRuntimeLibrary;
using System.Security.Cryptography;

namespace MGP_25_Mod_Launcher
{
    internal static class Utilities
    {
        public static string returnValidGameDirectory(string pcDirectory)
        {
            int liNumberOfFolders;

            if (string.IsNullOrWhiteSpace(pcDirectory) || !Directory.Exists(pcDirectory)) return "";

            liNumberOfFolders = pcDirectory.Split("\\").Length - 1;

            // Goes through each folder in the path and if the relative locations of the bootstrap + shipping exe can be found it is a valid directory
            for (int li = 0; li < liNumberOfFolders; li++)
            {
                if (System.IO.File.Exists(pcDirectory + DirConstants.cEACConfigDir) && System.IO.File.Exists(pcDirectory + "\\" + DirConstants.cBootstrapName))
                {
                    return pcDirectory;
                }
                pcDirectory = pcDirectory.Substring(0, pcDirectory.LastIndexOf("\\"));
            }

            return "";
        }

        public static void displayError(string pcError)
        {
            MessageBox.Show(pcError, UIStrings.cErrorTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void displayInformation(string pcInfo, string pcTitle = UIStrings.cSuccessTitle)
        {
            MessageBox.Show(pcInfo, pcTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void addShortcutToDesktop(string pcGameDir, int piModded, string pcGameName = "")
        {
            dynamic loMyShortCut;
            string lcLinkName = string.IsNullOrEmpty(pcGameName) ? DirConstants.cGameName : pcGameName;
            string lcArguments;

            if (piModded == 0)
            {
                lcLinkName += " Without Mods";
                lcArguments = "-LaunchDefault";
            }
            else
            {
                lcLinkName += " With Mods";
                lcArguments = "-LaunchModded";
            }

            lcArguments += " -" + pcGameName.Replace(" ", "");

            loMyShortCut = new WshShell().CreateShortcut(DirConstants.cDesktopDir + "\\" + lcLinkName + ".lnk");
            loMyShortCut.IconLocation = pcGameDir + DirConstants.cExeDir;
            loMyShortCut.TargetPath = DirConstants.cWorkingDir + "\\" + DirConstants.cLauncherName + ".exe";
            loMyShortCut.Arguments = lcArguments;
            loMyShortCut.WorkingDirectory = DirConstants.cWorkingDir;
            loMyShortCut.Save();
        }

        public static string getMD5HashAsBase64(string pcFileName)
        {
            byte[] lwMD5Hash;
            
            using (MD5 loMD5 = MD5.Create())
            {
                using (FileStream loFileStream = System.IO.File.OpenRead(pcFileName))
                {
                    lwMD5Hash = loMD5.ComputeHash(loFileStream);
                    return Convert.ToBase64String(lwMD5Hash);
                }
            }
        }

        public static string getMD5HashAsBase64(byte[] pcFiletoHash)
        {
            byte[] lwMD5Hash;

            using (MD5 loMD5 = MD5.Create())
            {
                lwMD5Hash = loMD5.ComputeHash(pcFiletoHash);
                return Convert.ToBase64String(lwMD5Hash);
            }
        }
    }
}
