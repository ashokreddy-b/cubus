/**********************************************************************************
**  File Name   : Settings.cs                                                    **
**                                                                               **
**  Purpose     : This class file contains the configuration parameter details   **
*                 used in this application.                                      **
**                                                                               **
**********************************************************************************/

namespace LogFileAnalysis.Constants
{
    /// <summary>
    /// Class Name		  : Settings
    /// Class Description : This class contains the configuration parameter details.
    /// </summary>
    internal static partial class Settings
    {
        public static string ERROR_LOG_KEYWORD = GetSetting("ERROR_LOG_KEYWORD");
        public static string CONTAINER_NAME = GetSetting("CONTAINER_NAME");
        public static string AZURE_CONNECT = GetSetting("AZURE_CONNECT");
        public static string DOWNLOAD_FILE_FOLDER = GetSetting("DOWNLOAD_FILE_FOLDER");
        public static string EXTRACT_FILE_FOLDER = GetSetting("EXTRACT_FILE_FOLDER");
        public static string EMAIL_RECEIPIENT_ADDRESS = GetSetting("EMAIL_RECEIPIENT_ADDRESS");
        public static string SENDER_PASSWORD = GetSetting("SENDER_PASSWORD");
        public static string SMTP_HOST = GetSetting("SMTP_HOST");
        public static string SMTP_PORT = GetSetting("SMTP_PORT");
        public static string DATE_FORMAT = GetSetting("DATE_FORMAT");
        public static string LOG_FOLDER = GetSetting("LOG_FOLDER");
        public static string POLICY_NAME = GetSetting("POLICY_NAME");
        public static string FRONT_END_CORS_ORIGIN = GetSetting("FRONT_END_CORS_ORIGIN");
        public static string API_KEY_VALUE = GetSetting("API_KEY_VALUE");
    }//End of internal static partial class Settings
}//End of namespace LogFileAnalysis.Constants
