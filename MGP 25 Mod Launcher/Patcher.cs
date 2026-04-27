using System.Diagnostics;

namespace MGP_25_Mod_Launcher
{
    internal static class Patcher
    {
        public static bool patchSettingsFile(string pcGameDir, string pcGameName)
        {
            string lcEACConfigFilePath = pcGameDir + DirConstants.cEACConfigDir;
            string[] lcEACConfigFile;
            string lcSubDir = "//" + pcGameName + "//";

            if (File.Exists(lcEACConfigFilePath))
            {
                lcEACConfigFile = File.ReadAllLines(lcEACConfigFilePath);

                // Modifies anti-cheat config so that the modified client can launch (offline only)
                for (int i = 0; i < lcEACConfigFile.Length; i++)
                {
                    if (lcEACConfigFile[i].Contains("productid") || lcEACConfigFile[i].Contains("sandboxid"))
                    {
                        lcEACConfigFile[i] = lcEACConfigFile[i].Substring(0, lcEACConfigFile[i].Length - 2)
                                            + "A" + lcEACConfigFile[i].Substring(lcEACConfigFile[i].Length - 2);
                    }
                }

                if (!Directory.Exists(DirConstants.cModdedDir + lcSubDir))
                {
                    Directory.CreateDirectory(DirConstants.cModdedDir + lcSubDir);
                }

                File.WriteAllLines(DirConstants.cModdedDir + lcSubDir + DirConstants.cEACConfigName, lcEACConfigFile);
            }

            return File.Exists(DirConstants.cModdedDir + lcSubDir + DirConstants.cEACConfigName);
        }

        public static void backupOriginalGameFiles(string pcExeFilePath, string pcEAConfigCFilePath, string pcGameName = "")
        {
            string lcSubDir = "\\" + pcGameName + "\\";

            if (pcGameName == "")
            {
                lcSubDir = "";
            }

            if (!Directory.Exists(DirConstants.cVanillaDir + lcSubDir))
            {
                Directory.CreateDirectory(DirConstants.cVanillaDir + lcSubDir);
            }

            if (!Directory.Exists(DirConstants.cModdedDir + lcSubDir))
            {
                Directory.CreateDirectory(DirConstants.cModdedDir + lcSubDir);
            }

            File.Copy(pcExeFilePath, DirConstants.cVanillaDir + lcSubDir + DirConstants.cBootstrapName, true);

            // EAC config never changes between updates, so does not need to be recopied
            if (!File.Exists(DirConstants.cVanillaDir + lcSubDir + DirConstants.cEACConfigName))
            {
                File.Copy(pcEAConfigCFilePath, DirConstants.cVanillaDir + lcSubDir + DirConstants.cEACConfigName, true);
            }              

        }
    }
}
