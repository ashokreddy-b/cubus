/**********************************************************************************
**  File Name   : SMTPClientMail.cs                                              **
**                                                                               **
**  Purpose     : This class file is contains the method to send email to CUBOS  **
*                 support email.                                                 **
**                                                                               **
**********************************************************************************/
using LogFileAnalysis.Constants;
using LogFileAnalysis.Controllers;
using System;
using System.Net;
using System.Net.Mail;

namespace LogFileAnalysis.Models.MailRepo
{
    /// <summary>
    /// Class Name		  : SMTPClientMail
    /// Class Description : This class contains the method which sends email using  
    ///                     SMTP server.
    /// </summary>
    internal class SMTPClientMail
    {
        /// <summary>
        /// Method Name				: SMTPClientMail
        /// Method Description		: Default constructor
        /// Requirement Id   		: 
        /// Method input Parameters	: void
        /// Method Return Parameter	: void
        /// </summary>
        public SMTPClientMail()
        {
        }

        /// <summary>
        /// Method Name				: SendMail
        /// Method Description		: This method send email using SMTP sever details to 
        ///                           CUBOS support team.
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : emailBody - email body content
        ///                           localFilePath - path in which file is downloaded from AZURE
        ///                           emailParams - parameters read from POST request body
        /// Method Return Parameter	: void
        /// </summary>
        internal void SendMail(string emailBody, System.Threading.Tasks.Task<string> localFilePath, EmailParams emailParams)
        {
            try
            {
                /* Sender's email address and password */
                string senderEmail = emailParams.SenderEmail;
                string senderPassword = Settings.SENDER_PASSWORD;

                /* Recipient's email address */
                string recipientEmail = Settings.EMAIL_RECEIPIENT_ADDRESS;

                /* Create a new MailMessage */
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(senderEmail)
                };
                mail.To.Add(recipientEmail);
                mail.Subject = emailParams.EmailSubject;
                mail.Body = emailBody;
                mail.IsBodyHtml = true;
                Attachment data = new Attachment(localFilePath.Result);
                mail.Attachments.Add(data);

                /* Create a new SmtpClient instance with server and port details */
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = Settings.SMTP_HOST,
                    Port = int.Parse(Settings.SMTP_PORT),
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    /* Enable SSL */
                    EnableSsl = false
                };

                /* Send the email */
                smtpClient.Send(mail);
            }//End of try
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
            }//End of catch (Exception exe)
        }//End of internal void SendMail(string emailBody, System.Threading.Tasks.Task<string> localFilePath, EmailParams emailParams)
    }//End of internal class SMTPClientMail
}//End of LogFileAnalysis.Models.MailRepo
