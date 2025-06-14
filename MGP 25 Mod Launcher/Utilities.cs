﻿using IWshRuntimeLibrary;
using System.Security.Cryptography;

namespace MGP_25_Mod_Launcher
{
    internal static class Utilities
    {
        public static bool isValidGameDirectory(string pcDirectory)
        {
            if (string.IsNullOrWhiteSpace(pcDirectory) || !Directory.Exists(pcDirectory)) return false;

            // Determines if this directory is *actually* the game directory
            return System.IO.File.Exists(trimDirectory(pcDirectory) + DirConstants.cExeDir);
        }

        public static string trimDirectory(string pcDirectory)
        {
            int liTrimmedStringLength = pcDirectory.LastIndexOf(DirConstants.cGameName) + DirConstants.cGameName.Length;

            if (liTrimmedStringLength >= DirConstants.cGameName.Length)
            {
                return pcDirectory.Substring(0, liTrimmedStringLength);
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

        public static void addShortcutToDesktop(string pcGameDir, int piModded)
        {
            dynamic loMyShortCut;
            string lcLinkName = DirConstants.cGameName;
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
    }
}
