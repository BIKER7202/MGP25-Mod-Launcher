namespace MGP_25_Mod_Launcher
{
    internal static class DirConstants
    {
        public static readonly string cWorkingDir = Directory.GetCurrentDirectory(); // Complains when const - readonly instead

        public static readonly string cDesktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); // Same as above
        
        public static readonly string cAppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // Same as above

        public static readonly string cLauncherName = AppDomain.CurrentDomain.FriendlyName;

        public const string cDefaultGameDir = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\MotoGP™25";
        
        public const string cGameName = "MotoGP™25";

        public const string cExeName = "motogp25-Win64-Shipping.exe";

        public const string cExeDir = "\\motogp25\\Binaries\\Win64\\" + cExeName;

        public const string cBootstrapName = "start_protected_game.exe";

        public const string cEACConfigName = "Settings.json";

        public const string cEACConfigDir = "\\EasyAntiCheat\\" + cEACConfigName;

        public static readonly string cModdedDir = cWorkingDir + "\\modded\\";
        
        public static readonly string cVanillaDir = cWorkingDir + "\\vanilla\\";

        public static readonly string cSettingsDir = cAppDataDir + "\\MGP 25 Mod Launcher";

        public static readonly string[] cSupportedGames = new string[] { "MotoGP 25", "MotoGP 26", "RIDE 6" };
    }
}
