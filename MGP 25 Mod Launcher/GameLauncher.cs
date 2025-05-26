using System.Diagnostics;

namespace MGP_25_Mod_Launcher
{
    public static class GameLauncher
    {
        public static void launchVanillaGame(string pcGameDir)
        {
            launchGame(0, pcGameDir);
        }

        public static void launchModdedGame(string pcGameDir)
        {
            launchGame(1, pcGameDir);
        }

        public static void launchGame(int piModded, string pcGameDir)
        {
            string lcExeToCopy;
            string lcEACConfigToCopy;
            string lcProcess = pcGameDir + DirConstants.cExeDir;

            ProcessStartInfo loProcessToStart = new ProcessStartInfo();
            loProcessToStart.UseShellExecute = false;
            loProcessToStart.CreateNoWindow = true;
            loProcessToStart.FileName = lcProcess;

            if (piModded == 0)
            {
                lcExeToCopy = DirConstants.cVanillaDir + DirConstants.cExeName;
                lcEACConfigToCopy = DirConstants.cVanillaDir + DirConstants.cEACConfigName;
            }
            else
            {
                lcExeToCopy = DirConstants.cModdedDir + DirConstants.cExeName;
                lcEACConfigToCopy = DirConstants.cModdedDir + DirConstants.cEACConfigName;
            }

            if (File.Exists(lcProcess) && File.Exists(lcExeToCopy) && File.Exists(lcEACConfigToCopy))
            {
                File.Copy(lcExeToCopy, lcProcess, true);
                File.Copy(lcEACConfigToCopy, pcGameDir + DirConstants.cEACConfigDir, true);
                Process.Start(loProcessToStart);
                Environment.Exit(0);
            }
        }
    }
}
