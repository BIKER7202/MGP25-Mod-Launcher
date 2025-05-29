namespace MGP_25_Mod_Launcher
{
    internal static class UIStrings
    {
        public const string cWelcomeText = "Welcome to BIKER's MotoGP 25 Mod Launcher!\n" +
                                           "\nAs this is your first time opening the app you need to provide the location of your game files." +
                                           "\nTo start, hit \"OK\"";
        
        public const string cWelcomeTitle = "Welcome";
        
        public const string cBrowserTitle = "Select the MotoGP 25 game directory";

        public const string cErrorTitle = "Error";
        
        public const string cErrorDirText = "This directory is not a MotoGP 25 game directory, please select a valid MotoGP 25 game directory.";
        
        public const string cErrorPatchText = "Patching Failed!\n" +
                                              "\nMake sure your exe is a valid MotoGP 25 executable and is not already patched." +
                                              "\nThe program will now close.";

        public const string cErrorLaunchText = "Failed To Launch!\n" +
                                               "\nMake sure your the game directory is correctly set.";

        public const string cSuccessTitle = "Success";

        public const string cSuccessDirText = "Your MotoGP 25 game directory has been successfully set!\n" +
                                              "\nIf you ever need to change this in the future you can via the 'Change Game Directory' Option.";

        public const string cSuccessPatchText = "Patching Successful!\n" +
                                                "\nYou can now play with mods via the 'Launch Game With Mods' Option." +
                                                "\nYou will need to repeat this step whenever the game is updated." +
                                                "\nThis can be done via the 'Repack Exe' Option.";

        public const string cInfoTitle = "Information";

        public const string cUpdateDetectedText = "Game Update Detected!\n" +
                                                  "\nThe Launcher will now repatch this updated file";

        public const string cGameAlreadyPatched = "Game Already Patched!";
    }
}
