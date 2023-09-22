/**********************************************************************************
**  File Name   : LogFileErrorAnalysis.cs                                        **
**                                                                               **
**  Purpose     : This class file is contains the method to get the log message  **
*                 details and send mail.                                         **
**                                                                               **
**********************************************************************************/
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using LogFileAnalysis.Models.AzureCollection;
using LogFileAnalysis.Models.MailRepo;
using System.Text;
using LogFileAnalysis.Constants;
using LogFileAnalysis.Controllers;
using System.IO.Compression;
using SharpCompress.Readers;

namespace LogFileAnalysis.Models.LogFileAnalysis
{
    /// <summary>
    /// Class Name		  : FilterLogMessages
    /// Class Description : This class contains the method to collect the log message details
    ///                     and send mail using SMTP server.
    /// </summary>
    internal class FilterLogMessages
    {
        /// <summary>
        /// Method Name				: FilterLogMessages
        /// Method Description		: Default constructor
        /// Requirement Id   		: 
        /// Method input Parameters	: void
        /// Method Return Parameter	: void
        /// </summary>
        public FilterLogMessages()
        {
        }

        /// <summary>
        /// Method Name				: GetLogFileMessages
        /// Method Description		: This method fetch the log messages from downloaded folder
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : downloadFolderPath - path to downloaded folder
        ///                           extractedFolderPath - folder path to extract files
        /// Method Return Parameter	: logMsg - List of log messages
        /// </summary>
        public List<string> GetLogFileMessages(string downloadFolderPath, string extractedFolderPath)
        {
            /* Variable to collect log messages */
            List<string> logMsg = new List<string>();

            try
            {
                /* Unzip downloaded folder */
                UnzipDownloadedLogFile(downloadFolderPath, extractedFolderPath);

                /* Collect the files from extracted folder */
                string[] fileCol = Directory.GetFiles(extractedFolderPath + "\\" + Settings.LOG_FOLDER, "*.*", SearchOption.AllDirectories);

                /* Loop through each files from collection */
                foreach (string file in fileCol)
                {
                    /* Read each line from file */
                    string[] lines = File.ReadAllLines(file);
                    /* Loop through each line */
                    foreach (string line in lines)
                    {
                        /* filter the log messages based on keywords */
                        if (line.Contains(Settings.ERROR_LOG_KEYWORD))
                        {
                            /* Add the log message to list */
                            logMsg.Add(line);
                        }//End of if (!userInput.excludeKeywords.Any(s => line.Contains(s)) && userInput.includeKeywords.Any(s => line.Contains(s)))
                    }//End of foreach (string line in lines)
                }//End of foreach (string file in fileCol)

            }//End of try
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
            }
            /* Return list of log messages */
            return logMsg;
        }//End of public List<string> GetLogFileMessages(string downloadFolderPath, string extractedFolderPath)

        /// <summary>
        /// Method Name				: UnzipDownloadedLogFile
        /// Method Description		: This method unzip the downloded folder
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : downloadFolderPath - path to downloaded folder
        ///                           extractedFolderPath - folder path to extract files
        /// Method Return Parameter	: void
        /// </summary>
        private void UnzipDownloadedLogFile(string downloadFolderPath, string extractedFolderPath)
        {
            try
            {
                /* Open the .tar.gz file using a FileStream */
                using FileStream fileStream = new FileStream(downloadFolderPath, FileMode.Open);

                /* Decompress the GZip stream */
                using GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);

                /* Use SharpCompress to create a reader for the TAR archive */
                using var reader = ReaderFactory.Open(gzipStream);

                /* Loop through each entry in the archive */
                while (reader.MoveToNextEntry())
                {
                    /* Combine the output folder path with the entry key to create the destination path */
                    string entryPath = Path.Combine(extractedFolderPath, reader.Entry.Key);

                    /* Check if the entry is a directory */
                    if (reader.Entry.IsDirectory)
                    {
                        /*Create directory for output folder */
                        Directory.CreateDirectory(entryPath);
                    }//End of if (reader.Entry.IsDirectory)
                    else
                    {
                        /* Get the parent directory path */
                        string entryDirectory = Path.GetDirectoryName(entryPath);
                        if (!string.IsNullOrEmpty(entryDirectory))
                        {
                            /* Create the parent directory if it doesn't exist */
                            Directory.CreateDirectory(entryDirectory);
                        }//End of if (!string.IsNullOrEmpty(entryDirectory))

                        /* Create a FileStream for the entry and extract its content to the destination file */
                        using FileStream entryStream = File.OpenWrite(entryPath);
                        reader.WriteEntryTo(entryStream);
                    }//End of else
                }//End of while (reader.MoveToNextEntry())
            }//End of try
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
            }//End of catch(Exception exe)
        }//End of private void UnzipDownloadedLogFile(string downloadFolderPath, string extractedFolderPath)

        /// <summary>
        /// Method Name				: StartLogFileAnalysis
        /// Method Description		: This method start the log file analysis
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : emailParams - parameter values based on server request body
        /// Method Return Parameter	: void
        /// </summary>
        public void StartLogFileAnalysis(EmailParams emailParams)
        {
            try
            {
                /* Variable containing download folder path */
                string downloadFolderPath = Settings.DOWNLOAD_FILE_FOLDER + "_" + emailParams.SenderEmail;

                /* Variable containing extracted folder path */
                string extractedFolderPath = Settings.EXTRACT_FILE_FOLDER + "_" + emailParams.SenderEmail;

                /* Find correct log file */
                AzureLogFiles azureLogFiles = new AzureLogFiles();
                Task<string> localFilePath = azureLogFiles.DownloadLogFromAzure(emailParams, downloadFolderPath);

                /* Condition to check downloaded folder path */
                if (!string.IsNullOrEmpty(localFilePath.Result))
                {
                    /* Error analysis with the given keywords */
                    _ = new FilterLogMessages();
                    List<string> logMsg = GetLogFileMessages(localFilePath.Result, extractedFolderPath);

                    //THIS PART IS ADDED FOR TESTING - TBD 
                    if (logMsg.Count == 0)
                    {
                        string testString = "";
                        foreach (string test in logMsg)
                        {
                            testString += test + Environment.NewLine;
                        }
                        WriteLogToTextFile(testString, emailParams);
                    }
                    else
                    {
                        AnalyseLogFile.ExceptionOccurred = "Filtered log messaged not available to create file";
                    }

                    /* Form a string containing email body */
                    EmailBody emailBody = new EmailBody();
                    string emailBodyContent = emailBody.FormEmailBody(emailParams, logMsg);

                    /* Send email to support team */
                    SMTPClientMail sMTPClientMail = new SMTPClientMail();
                    sMTPClientMail.SendMail(emailBodyContent, localFilePath, emailParams);

                    /* Clean up the data */
                    DeleteCreatedFolder(downloadFolderPath);
                    DeleteCreatedFolder(extractedFolderPath);
                }//End of if (!string.IsNullOrEmpty(localFilePath.ToString()))
                else
                {
                    AnalyseLogFile.ExceptionOccurred = "Failed to download file from AZURE!!!";
                }//End of else
            }//End of try
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
            }//End of catch (Exception ex)
        }//End of public void StartLogFileAnalysis(EmailParams emailParams)

        //THIS PART IS ADDED FOR TESTING - TBD
        internal void WriteLogToTextFile(string logMsg, EmailParams emailParams)
        {
            string createFilePath = "CUBOS_" + emailParams.LogFilePath + ".txt";
            try
            {
                if (File.Exists(createFilePath))
                {
                    File.Delete(createFilePath);
                }
                FileStream fs = File.Create(createFilePath);
                byte[] author = new UTF8Encoding(true).GetBytes(logMsg);

                fs.Write(author, 0, author.Length);
                fs.Close();
            }
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
            }
        }

        /// <summary>
        /// Method Name				: DeleteCreatedFolder
        /// Method Description		: This method is used to delete the created fodler for log message extract
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : folderPath - Path to folder which to be deleted
        /// Method Return Parameter	: void
        /// </summary>
        private static void DeleteCreatedFolder(string folderPath, int maxRetries = 3, int delayMilliseconds = 15000)
        {
            int retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    if (Directory.Exists(folderPath))
                    {
                        /* Delete the folder */
                        Directory.Delete(folderPath, true);
                        Console.WriteLine($"Folder '{folderPath}' deleted successfully.");
                        return;
                    }//End of if (Directory.Exists(folderPath))
                    else
                    {
                        Console.WriteLine($"Folder '{folderPath}' does not exist.");
                        return;
                    }//End of else
                }//End of try
                catch (IOException ex)
                {
                    /* Folder is in use; retry after a short delay */
                    Console.WriteLine($"Error while deleting folder: {ex.Message}");
                    retryCount++;
                    System.Threading.Thread.Sleep(delayMilliseconds);
                }//End of catch (IOException ex)
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while deleting folder: {ex.Message}");
                    return;
                }//End of catch (Exception ex)
            }//End of while (retryCount < maxRetries)
        }//End of private static void DeleteCreatedFolder(string folderPath, int maxRetries = 3, int delayMilliseconds = 15000)
    }//End of internal class FilterLogMessages
}//End of namespace LogFileAnalysis.Models.LogFileAnalysis
