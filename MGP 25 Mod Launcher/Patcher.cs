using System.Diagnostics;

namespace MGP_25_Mod_Launcher
{
    internal static class Patcher
    {
        public static bool patchGame(string pcGameDir)
        {
            string lcEACConfigFilePath = pcGameDir + DirConstants.cEACConfigDir;
            string lcExeFilePath = pcGameDir + DirConstants.cExeDir;
            int liIndexOfHex1;
            int liIndexOfHex2;
            byte[] lExeFile;
            string[] lcEACConfigFile;

            if (File.Exists(lcExeFilePath) && File.Exists(lcEACConfigFilePath))
            {
                // Creates folders for the modded + default files to be copied to
                createFolders();

                lExeFile = File.ReadAllBytes(lcExeFilePath);

                liIndexOfHex1 = findIndexOfBytes(ref HexPatterns.pattern1, ref lExeFile);
                liIndexOfHex2 = findIndexOfBytes(ref HexPatterns.pattern2, ref lExeFile);

                /* Replaces bytes within exe that allow custom paks to be loaded 
                   (uses sequences instead of offsets so that it works for subsequent updates) */
                if (liIndexOfHex1 > -1 && liIndexOfHex2 > -1)
                {
                    // Only want to do this if we know exe is not already patched - hence why in this if
                    backupOriginalGameFiles(lcExeFilePath, lcEACConfigFilePath);                    

                    replaceBytes(ref lExeFile, ref HexPatterns.replacement1, 
                                 liIndexOfHex1 + HexPatterns.pattern1.Length - HexPatterns.replacement1.Length);
                    
                    replaceBytes(ref lExeFile, ref HexPatterns.replacement2,
                                 liIndexOfHex2 + HexPatterns.pattern2.Length - HexPatterns.replacement2.Length);

                    if(!Directory.Exists(DirConstants.cModdedDir))
                    {
                        Directory.CreateDirectory(DirConstants.cModdedDir);
                    }

                    File.WriteAllBytes(DirConstants.cModdedDir + DirConstants.cExeName, lExeFile);
                }
                else
                {
                    // If hex values aren't found, game is either already patched or it is not MotoGP 25
                    return false;
                }

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

                if (!Directory.Exists(DirConstants.cModdedDir))
                {
                    Directory.CreateDirectory(DirConstants.cModdedDir);
                }

                File.WriteAllLines(DirConstants.cModdedDir + DirConstants.cEACConfigName, lcEACConfigFile);
            }

            return File.Exists(DirConstants.cModdedDir + DirConstants.cExeName) 
                   && File.Exists(DirConstants.cModdedDir + DirConstants.cEACConfigName);
        }

        private static int findIndexOfBytes(ref byte[] pSequenceToFind, ref byte[] pBytesToSearch)
        {
            if (pSequenceToFind.Length == 0 || pBytesToSearch.Length == 0 
                || pBytesToSearch.Length < pSequenceToFind.Length) return -1;

            for (int i = 0; i < pBytesToSearch.Length - pSequenceToFind.Length; i++)
            {
                if (pBytesToSearch[i] == pSequenceToFind[0])
                {
                    for (int j = 1; j < pSequenceToFind.Length; j++)
                    {
                        if (pBytesToSearch[i + j] != pSequenceToFind[j])
                            break;

                        if (j == pSequenceToFind.Length - 1)
                            return i;
                    }
                }
            }

            return -1;
        }

        private static void replaceBytes(ref byte[] pByteArray, ref byte[] pReplacementBytes, int piStartingIndex) 
        {
            if (piStartingIndex < pByteArray.Length && pReplacementBytes.Length <= pByteArray.Length
                && pReplacementBytes.Length + piStartingIndex < pByteArray.Length)
            {
                for (int i = 0; i < pReplacementBytes.Length; i++)
                {
                    pByteArray[piStartingIndex + i] = pReplacementBytes[i];
                }
            }
        }

        // Creates folders for where the exes and jsons will be stored - then copied into game dir when launching
        private static void createFolders()
        {
            if (!Directory.Exists(DirConstants.cModdedDir))
            {
                Directory.CreateDirectory(DirConstants.cModdedDir);
            }

            if (!Directory.Exists(DirConstants.cVanillaDir))
            {
                Directory.CreateDirectory(DirConstants.cVanillaDir);
            }
        }

        private static void backupOriginalGameFiles(string pcExeFilePath, string pcEAConfigCFilePath)
        {
            File.Copy(pcExeFilePath, DirConstants.cVanillaDir + DirConstants.cExeName, true);

            // EAC config never changes between updates, so does not need to be recopied
            if (!File.Exists(DirConstants.cVanillaDir + DirConstants.cEACConfigName))
            {
                File.Copy(pcEAConfigCFilePath, DirConstants.cVanillaDir + DirConstants.cEACConfigName, true);
            }              

        }
    }
}
