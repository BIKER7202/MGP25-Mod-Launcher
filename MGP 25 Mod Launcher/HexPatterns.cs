﻿namespace MGP_25_Mod_Launcher
{
    internal static class HexPatterns
    {
        public static byte[] pattern1 = [0x53, 0x05, 0x00, 0x39, 0x70, 0x08, 0x74];

        public static byte[] replacement1 = [0x75];

        public static byte[] pattern2 = [0x84, 0xC0, 0x74, 0x04, 0xB0, 0x01, 0xEB, 0x02, 0x32, 0xC0, 0x41, 0x88, 0x46, 0x30];

        public static byte[] replacement2 = [0x90, 0x90, 0x90, 0x90];
    }
}
