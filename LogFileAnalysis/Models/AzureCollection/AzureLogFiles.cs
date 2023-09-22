/**********************************************************************************
**  File Name   : AzureLogFiles.cs                                               **
**                                                                               **
**  Purpose     : This class file is created to connect to Azure and download    **
*                 the required log folder.                                       **
**                                                                               **
**********************************************************************************/
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LogFileAnalysis.Constants;
using LogFileAnalysis.Controllers;
using LogFileAnalysis.Models.MailRepo;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LogFileAnalysis.Models.AzureCollection
{
    /// <summary>
    /// Class Name		  : AzureLogFiles
    /// Class Description : This class contains the method to download the folder containing
    ///                     log files to download from Azure.
    /// </summary>
    public class AzureLogFiles
    {
        /// <summary>
        /// Method Name				: AzureLogFiles
        /// Method Description		: Default constructor
        /// Requirement Id   		: 
        /// Method input Parameters	: void
        /// Method Return Parameter	: void
        /// </summary>
        public AzureLogFiles()
        {
        }

        /// <summary>
        /// Method Name				: DownloadLogFromAzure
        /// Method Description		: This method is used to download the log folder
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : emailParams - parameters read from server response
        ///                           downloadFolderPath - folder to download the files from azure
        /// Method Return Parameter	: localFilePath - downloaded folder path
        /// </summary>
        public async Task<string> DownloadLogFromAzure(EmailParams emailParams, string downloadFolderPath)
        {
            string localFilePath = "";
            try
            {
                /* Connect to Azure - Retrieve the connection string for use with the application. */
                string connectionString = Settings.AZURE_CONNECT;

                /* Download files from Azure to code folder path */
                /*Variable to get the container */
                string containerName = Settings.CONTAINER_NAME;

                /* Get the instance of existing container */
                var blobContainerClient = new BlobContainerClient(connectionString, containerName);

                /* Create local folder path to download */                
                Directory.CreateDirectory(downloadFolderPath);
                BlobClient blobClient;

                /* Loop through the blobs available in the given container */
                await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
                {
                    /* Get the required blob */
                    if (blobItem.Name.Equals(emailParams.LogFilePath))
                    {
                        /* Path with blob name */
                        localFilePath = Path.Combine(downloadFolderPath, blobItem.Name);

                        /* Get a reference to a blob */
                        blobClient = blobContainerClient.GetBlobClient(blobItem.Name);

                        /* Download the blob to local path */
                        await blobClient.DownloadToAsync(localFilePath);
                        break;
                    }//End of if (blobItem.Name.Equals(emailParams.LogFilePath))
                }//End of await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
            }//End of try
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
            }//End of catch(Exception exe)

            /* Return local path containing downloaded file */
            return localFilePath;
        }//End of public async Task<string> DownloadLogFromAzure(EmailParams emailParams, string downloadFolderPath)
    }//End of public class AzureLogFiles
}//End of namespace LogFileAnalysis.AzureCollection
