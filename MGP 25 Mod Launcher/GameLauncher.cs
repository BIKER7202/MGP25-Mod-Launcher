using System.Diagnostics;
using System.Reflection;

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

        private static byte[] getResourceByGameName(string pcGameName, string pcResourceType)
        {
            string lcResourceName = $"{pcGameName} Test Mod {pcResourceType}";
            byte[]? lwTemp;

            PropertyInfo? loResourceProperty = typeof(Properties.Resources).GetProperty(
                lcResourceName.Replace(" ", "_"),
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.IgnoreCase
            );

            if (loResourceProperty != null && loResourceProperty.CanRead)
            {
                // Safely cast/GetValue and handle possible nulls
                lwTemp = loResourceProperty.GetValue(null) as byte[];
                if (lwTemp != null)
                {
                    return lwTemp;
                }
            }

            return Array.Empty<byte>();
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
            string lcTestModPath;

            ProcessStartInfo loProcessToStart = new ProcessStartInfo();
            loProcessToStart.UseShellExecute = false;
            loProcessToStart.CreateNoWindow = true;
            loProcessToStart.FileName = lcExeToReplace;

            lcGameNameSimplified = pcGameName.Replace(" ", "").ToLower();
            lcBinaryPath = pcGameDir + "\\" + lcGameNameSimplified + DirConstants.cBinaryDir;
            lcAsiLoaderPath = lcBinaryPath + DirConstants.cDllName;
            lcSigBypassPath = lcBinaryPath + DirConstants.cSigBypassName;
            lcTestModPath = pcGameDir + "\\" + lcGameNameSimplified + DirConstants.cPakDir + DirConstants.cTestModName;

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

                    // Dynamically copy pak, utoc, and ucas files based on game name
                    File.WriteAllBytes(lcTestModPath + ".pak", getResourceByGameName(pcGameName, "Pak"));
                    File.WriteAllBytes(lcTestModPath + ".utoc", getResourceByGameName(pcGameName, "Utoc"));
                    File.WriteAllBytes(lcTestModPath + ".ucas", getResourceByGameName(pcGameName, "Ucas"));
                }

                Process.Start(loProcessToStart);
                Environment.Exit(0);
            }
        }
    }
}
