/**********************************************************************************
**  File Name   : EmailBody.cs                                                   **
**                                                                               **
**  Purpose     : This class file contains the method to form the email body     **
*                 based on the server request body and predefined values.        **
**                                                                               **
**********************************************************************************/
using LogFileAnalysis.Constants;
using LogFileAnalysis.Controllers;
using System;
using System.Collections.Generic;

namespace LogFileAnalysis.Models.MailRepo
{
    /// <summary>
    /// Class Name		  : FormEmailBody
    /// Class Description : This class contains the method which created the email body. 
    /// </summary>
    public class EmailBody
    {
        /// <summary>
        /// Method Name				: EmailBody
        /// Method Description		: Default constructor
        /// Requirement Id   		: 
        /// Method input Parameters	: void
        /// Method Return Parameter	: void
        /// </summary>
        public EmailBody()
        {
        }

        /// <summary>
        /// Method Name				: FormEmailBody
        /// Method Description		: This method create email body
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : emailParams - paremeters based on server request body
        ///                           logMsg - list containing log message details
        /// Method Return Parameter	: string - email body
        /// </summary>
        internal string FormEmailBody(EmailParams emailParams, List<string> logMsg)
        {
            /* Variable to contain the email body */
            string emailBody = "";
            int errorCount = logMsg.Count;
            bool errorLogFound = false;
            if (errorCount != 0)
            {
                errorLogFound = true;
            }

            string logText = $@"<p>- The following errors were found: <br></p>
                                <ul>
                                    {GenerateListItems(logMsg)}
                                </ul>";

            string logNoText = @"<p>- The following errors were found: 0 errors</p>";
            try
            {
                /* Collect the dynamic values based on the function arguments */
                string CurrentDate = DateTime.UtcNow.Date.ToString(Settings.DATE_FORMAT);
                string SenderEmail = emailParams.SenderEmail;
                string ChargingStationID = emailParams.ChargingStationID;
                string CustomerName = emailParams.CustomerName;
                string CustomerSupportEmail = emailParams.CustomerSupportEmail;
                string ChargingStationSerialNo = emailParams.ChargingStationSerialNo;
                string CustomerEnteredText = emailParams.CustomerEnteredText;

                /* Create the HTML content based on function arguments */
                emailBody = $@"
                            <html>
                            <body>
                                <h1>Analyse Log Files</h1>
                                    <P>Log file analysis details are as mentioned below:</P>
                        
                                        <p>- Ticket created on <strong>{CurrentDate}</strong> by <strong>{SenderEmail}</strong></p>

                                        <p>- Charging station <strong>{ChargingStationID}</strong> from the customer <strong>{CustomerName}</strong> with the e-mail <strong>{CustomerSupportEmail}</strong></p>                
                
                                        <p>- Serial number of the charging station: <strong>{ChargingStationSerialNo}</strong></p>                
                
                                        <p>- Text of the customer: <strong>{CustomerEnteredText}</strong></p>
                                        {(errorLogFound ? logText : logNoText)}                        
                            </body>
                            </html>";

            }//End of try
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
            }//End of catch(Exception exe)

            /* return email body content */
            return emailBody;
        }//End of internal string FormEmailBody(EmailParams emailParams, List<string> logMsg)

        /// <summary>
        /// Method Name				: GenerateListItems
        /// Method Description		: This method create the html content part based on the log messages list
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : logMsg - list of log messages
        /// Method Return Parameter	: object - html content for list
        /// </summary>
        private static object GenerateListItems(List<string> logMsg)
        {
            object listItems = "";
            /* Loop through log message list */
            foreach (var item in logMsg)
            {
                /* Create points for log messages in html */
                listItems += $"<li>{item}</li>";
            }//End of foreach (var item in logMsg)
            return listItems;
        }//End of private static object GenerateListItems(List<string> logMsg)
    }//End of public class EmailBody
}//End of namespace LogFileAnalysis.Models.MailRepo
