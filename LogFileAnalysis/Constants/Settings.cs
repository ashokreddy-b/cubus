/**********************************************************************************
**  File Name   : Settings.cs                                                    **
**                                                                               **
**  Purpose     : This class file is created to access the configuration         **
*                 settings used in this application.                             **
**                                                                               **
**********************************************************************************/
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace LogFileAnalysis.Constants
{
    /// <summary>
    /// Class Name		  : Settings
    /// Class Description : This class contains the methods to get the configurations.
    /// </summary>
    internal static partial class Settings
    {
        private static IConfigurationRoot config;
        private static IConfigurationRoot debugConfig;

        /// <summary>
        /// Method Name				: Config
        /// Method Description		: This method is used to get the configuration settings.
        /// Requirement Id   		: 
        /// Method input Parameters	: void
        /// Method Return Parameter	: config - configuration data
        /// </summary>
        public static IConfigurationRoot Config
        {
            get
            {
                try
                {
                    if (config == null)
                    {
                        /* Get the configuraiton details */
                        var assemblyConfigurationAttribute = typeof(Settings).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
                        var buildConfigurationName = assemblyConfigurationAttribute?.Configuration;
                        string settingFile = string.Format("settings.{0}.json", buildConfigurationName);
                        config = new ConfigurationBuilder()
                             .AddJsonFile(settingFile)
                             .AddEnvironmentVariables()
                             .Build();
                    }//End of if (config == null)
                }//End of try
                catch(Exception exe)
                {
                    Console.WriteLine("Error in loading configuration: " + exe.Message);
                }//End of catch(Exception exe)
                return config;
            }
        }//End of public static IConfigurationRoot Config        

        /// <summary>
        /// Method Name				: Config
        /// Method Description		: This method is used to get the specific configuration settings.
        /// Requirement Id   		: 
        /// Method input Parameters	: void
        /// Method Return Parameter	: debugConfig - specific configuration data
        /// </summary>
        public static IConfigurationRoot DebugConfig
        {
            get
            {
                try
                {
                    if (debugConfig == null)
                    {
                        string settingFile = "settings.Debug.json";
                        debugConfig = new ConfigurationBuilder()
                                .AddJsonFile(settingFile)
                                .AddEnvironmentVariables()
                                .Build();
                    }//End of if (debugConfig == null)
                }//End of try
                catch(Exception exe)
                {
                    Console.WriteLine("Error in loading specific configuration: " + exe.Message);
                }//End of catch(Exception exe)
                return debugConfig;
            }
        }//End of public static IConfigurationRoot DebugConfig

        /// <summary>
        /// Method Name				: GetSetting
        /// Method Description		: This method is used to retrieve a configuration setting 
        ///                           based on the provided setting key settings.
        /// Requirement Id   		: 
        /// Method input Parameters	: setting - key of the configuration setting
        /// Method Return Parameter	: Config[setting] - returns the value associated with the provided setting key
        /// </summary>
        internal static string GetSetting(string setting)
        {
            /* Returns the value associated with the provided setting key */
            return string.IsNullOrEmpty(Config[setting]) ? DebugConfig[setting] : Config[setting];
        }//End of internal static string GetSetting(string setting)
    }//End of internal static partial class Settings
}//End of namespace LogFileAnalysis.Constants
