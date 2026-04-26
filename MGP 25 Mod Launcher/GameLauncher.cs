using System.Diagnostics;

namespace MGP_25_Mod_Launcher
{
    public static class GameLauncher
    {
        public static void launchVanillaGame(string pcGameDir, string pcGameName)
        {
            launchGame(0, pcGameDir, pcGameName);
        }

        public static void launchModdedGame(string pcGameDir, string pcGameName)
        {
            launchGame(1, pcGameDir, pcGameName);
        }

        public static void launchGame(int piModded, string pcGameDir, string pcGameName)
        {
            string lcAsiLoaderPath;
            string lcBinaryPath;
            string lcSigBypassPath;
            string lcExeToCopy = "";
            string lcGameNameSimplified;
            string lcEACConfigToCopy;
            string lcExeToReplace = pcGameDir + "\\" + DirConstants.cBootstrapName;

            ProcessStartInfo loProcessToStart = new ProcessStartInfo();
            loProcessToStart.UseShellExecute = false;
            loProcessToStart.CreateNoWindow = true;
            loProcessToStart.FileName = lcExeToReplace;

            lcGameNameSimplified = pcGameName.Replace(" ", "").ToLower();
            lcBinaryPath = pcGameDir + "\\" + lcGameNameSimplified + DirConstants.cBinaryDir;
            lcAsiLoaderPath = lcBinaryPath + DirConstants.cDllName;
            lcSigBypassPath = lcBinaryPath + DirConstants.cSigBypassName;

            if (piModded == 0)
            {
                lcExeToCopy = DirConstants.cVanillaDir + pcGameName + "\\" + DirConstants.cBootstrapName;
                lcEACConfigToCopy = DirConstants.cVanillaDir + pcGameName + "\\" + DirConstants.cEACConfigName;

                if (!File.Exists(lcExeToCopy))
                {
                    return;
                }
            }
            else
            {
                lcEACConfigToCopy = DirConstants.cModdedDir + pcGameName + "\\" + DirConstants.cEACConfigName;
            }

            if (File.Exists(lcExeToReplace) && File.Exists(lcEACConfigToCopy))
            {
                File.Copy(lcEACConfigToCopy, pcGameDir + DirConstants.cEACConfigDir, true);

                // If running vanilla these must be put back to or EAC will stop game launching
                if (piModded == 0)
                {
                    if (File.Exists(lcAsiLoaderPath))
                    {
                        File.Delete(lcAsiLoaderPath);
                    }

                    if (File.Exists(lcSigBypassPath))
                    {
                        File.Delete(lcSigBypassPath);
                    }

                    File.Copy(lcExeToCopy, lcExeToReplace, true);
                }
                else
                {
                    File.WriteAllBytes(lcSigBypassPath, Properties.Resources.sigBypass);
                    File.WriteAllBytes(lcAsiLoaderPath, Properties.Resources.asiLoader);
                    File.WriteAllBytes(lcExeToReplace, Properties.Resources.eacExe);
                }

                Process.Start(loProcessToStart);
                Environment.Exit(0);
            }
        }
    }
}
