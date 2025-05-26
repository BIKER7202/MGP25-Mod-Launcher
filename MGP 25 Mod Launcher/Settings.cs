using System.Collections.Generic;
using System.IO;
using System.Runtime;

namespace MGP_25_Mod_Launcher
{
    internal class Settings
    {
        private string cConfigLocation;
        private List<string> cConfigFileLines;
        Dictionary<string, string> oSettings;
        private bool bConfigExists;


        public Settings() 
        {
            bConfigExists = false;
            cConfigLocation = DirConstants.cWorkingDir + "\\settings.txt";
            cConfigFileLines = new List<string>();
            oSettings = new Dictionary<string, string>();
            loadSettings();
        }

        private void loadSettings()
        {
            string lcSettingName;
            string lcSettingValue;

            bConfigExists = File.Exists(cConfigLocation);

            if (bConfigExists)
            {
                cConfigFileLines = File.ReadAllLines(cConfigLocation).ToList();

                foreach (string cSetting in cConfigFileLines)
                {
                    if (cSetting.IndexOf("=") > -1 && cSetting.Length > cSetting.IndexOf("=") + 1)
                    {
                        lcSettingName = cSetting.Substring(0, cSetting.IndexOf("="));
                        lcSettingValue = cSetting.Substring(cSetting.IndexOf("=") + 1);

                        if (!oSettings.ContainsKey(lcSettingName))
                        {
                            oSettings.Add(lcSettingName, lcSettingValue);
                        }
                    }                 
                }
            }
        }

        private void saveSettings() 
        {
            if (File.Exists(cConfigLocation))
            {
                File.Delete(cConfigLocation);
            }

            foreach (KeyValuePair<string, string> oSetting in oSettings)
            {
                File.AppendAllText(cConfigLocation, string.Format("{0}{1}{2}{3}", oSetting.Key, "=", oSetting.Value, Environment.NewLine));
            }
        }

        public bool getDoesConfigExist()
        {
            return bConfigExists;
        }

        public string getSetting(string pcSettingName)
        {
            if (oSettings.ContainsKey(pcSettingName))
            {
                return oSettings[pcSettingName];
            }
            
            return "";
        }

        public void setSetting(string pcSettingName, string pcSettingValue)
        {
            if (oSettings.ContainsKey(pcSettingName))
            {
                oSettings[pcSettingName] = pcSettingValue;
            }
            else
            {
                oSettings.Add(pcSettingName, pcSettingValue);
            }

            saveSettings();
        }
    }
}
