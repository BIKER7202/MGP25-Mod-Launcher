using System.Runtime;

namespace MGP_25_Mod_Launcher
{
    internal class HashChecker
    {
        private string lcVanillaHash;
        private string lcModdedHash;

        public HashChecker(string pcVanillaHash, string pcModdedHash)
        {
            lcVanillaHash = pcVanillaHash;
            lcModdedHash = pcModdedHash;
        }

        public bool isGameExeHashKnown(string pcGameDir)
        {
            string lcGameExeHash = Utilities.getMD5HashAsBase64(pcGameDir + DirConstants.cExeDir);

            return (lcGameExeHash == lcVanillaHash || lcGameExeHash == lcModdedHash);
        }

        public void checkBackedUpExeHashes()
        {
            if(File.Exists(DirConstants.cVanillaDir + DirConstants.cExeName) && File.Exists(DirConstants.cModdedDir + DirConstants.cExeName))
            {
                lcVanillaHash = Utilities.getMD5HashAsBase64(DirConstants.cVanillaDir + DirConstants.cExeName);
                lcModdedHash = Utilities.getMD5HashAsBase64(DirConstants.cModdedDir + DirConstants.cExeName);
            }
        }

        public string getModdedHash()
        {
            return lcModdedHash;
        }

        public string getVanillaHash()
        {
            return lcVanillaHash;
        }

        public void setModdedHash(string pcModdedHash)
        {
            lcModdedHash = pcModdedHash;
        }

        public void setVanillaHash(string pcVanillaHash)
        {
            lcVanillaHash = pcVanillaHash;
        }
    }
}
