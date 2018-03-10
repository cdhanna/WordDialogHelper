using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin
{
    public static class ConfigHelper
    {

        

        public const string FILE_NAME = "WordHelper.json";
        public static string FilePath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FILE_NAME); }
        }
        
        public static Config Config
        {
            get;
            private set;
        }
        
        static ConfigHelper()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(FilePath));

            } catch (FileNotFoundException ex)
            {
                Config = new Config();
            }
        }

        public static void Save()
        {
            try
            {
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(Config, Formatting.Indented));
            } catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class Config
    {
        [JsonProperty("isPanelOpen")]
        public bool IsPanelOpen { get; set; }

        [JsonProperty("defaultCSVPath")]
        public string DefaultCSVPath { get; set; }

        [JsonProperty("optionShowFirstError")]
        public bool OptionShowFirstError { get; set; }

        [JsonProperty("optionValidateVariables")]
        public bool OptionValidateVariables { get; set; }
    }
}
