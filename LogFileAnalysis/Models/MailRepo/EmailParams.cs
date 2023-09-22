/**********************************************************************************
**  File Name   : EmailParams.cs                                                 **
**                                                                               **
**  Purpose     : This class file contains the parameters required to form       **
*                 the email body.                                                **
**                                                                               **
**********************************************************************************/
using Microsoft.AspNetCore.Mvc;

namespace LogFileAnalysis.Models.MailRepo
{
    /// <summary>
    /// Class Name		  : EmailParams
    /// Class Description : This class contains the email parameters read from 
    ///                     server request body.
    /// </summary>
    public class EmailParams
    {
        /// <summary>
        /// Variable Name		 : LogFilePath
        /// Variable Description : Path of log file name to be downloaded from Azure
        /// Variable Type 		 : string
        /// </summary>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Variable Name		 : SenderEmail
        /// Variable Description : Sender email address
        /// Variable Type 		 : string
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Variable Name		 : SenderEmail
        /// Variable Description : Sender email address
        /// Variable Type 		 : string
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Variable Name		 : ChargingStationID
        /// Variable Description : Charging station ID
        /// Variable Type 		 : string
        /// </summary>
        public string ChargingStationID { get; set; }

        /// <summary>
        /// Variable Name		 : ChargingStationSerialNo
        /// Variable Description : Charging station Serial number
        /// Variable Type 		 : string
        /// </summary>
        public string ChargingStationSerialNo { get; set; }

        /// <summary>
        /// Variable Name		 : CustomerEnteredText
        /// Variable Description : Text which is entered by customer in UI
        /// Variable Type 		 : string
        /// </summary>
        public string CustomerEnteredText { get; set; }

        /// <summary>
        /// Variable Name		 : CustomerName
        /// Variable Description : Name of the customer
        /// Variable Type 		 : string
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Variable Name		 : CustomerSupportEmail
        /// Variable Description : Email ID of customer
        /// Variable Type 		 : string
        /// </summary>
        public string CustomerSupportEmail { get; set; }

    }//End of public class EmailParams

    /// <summary>
    /// Class Name		  : ResponseHeader
    /// Class Description : This class contains header content from server.
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        /// Variable Name		 : Authorization
        /// Variable Description : API Authorization Key
        /// Variable Type 		 : string
        /// </summary>
        [FromHeader]
        public string Authorization { get; set; }
    }//End of public class ResponseHeader
}//End of namespace LogFileAnalysis.Models.MailRepo
