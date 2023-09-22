/**********************************************************************************
**  File Name   : AnalyseLogFile.cs                                              **
**                                                                               **
**  Purpose     : This class file is created to invoke the server which will     **
*                 help to start the log file analysis.                           **
**                                                                               **
**********************************************************************************/
using LogFileAnalysis.Constants;
using LogFileAnalysis.Models.LogFileAnalysis;
using LogFileAnalysis.Models.MailRepo;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LogFileAnalysis.Controllers
{
    /// <summary>
    /// Class Name		  : AnalyseLogFile
    /// Class Description : This class contains the POST method to start the log file analysis.
    /// </summary>
    /// 1. HTTP Method: POST
    /// 2. Endpoint URL: cubos/logFileAnalysis
    /// 3. Description: This will help to start log file analysis
    /// 4. Request Parameters: -
    /// 5. Request Body: JSON format is used
    /// 6. Response: 
    ///     BadRequest - 400: when request body from server is empty
    ///     NotFound - 404: when API key is not available in header part of server response
    ///     Unauthorized - 401: api value from server is not matching with the value mentioned in code
    ///     Ok - 200: on successful log file analysis completion
    ///     Internal server error – 500: In case of any exception occurred from implemented methods
    [Route("api/v1/cubos/logfileanalysis")]
    [ApiVersion("1.0")]
    public class AnalyseLogFile : Controller
    {
        /// <summary>
        /// Variable Name		 : ExceptionOccurred
        /// Variable Description : variable used get exception message
        /// Variable Type 		 : string
        /// </summary>
        public static string ExceptionOccurred { get; set; }

        /// <summary>
        /// Method Name				: AnalyseLogFile
        /// Method Description		: Default constructor
        /// Requirement Id   		: 
        /// Method input Parameters	: void
        /// Method Return Parameter	: void
        /// </summary>
        public AnalyseLogFile()
        {
        }//End of public AnalyseLogFile(ILogger<AnalyseLogFile> logger)

        /// <summary>
        /// Method Name				: StartLogFileAnalysis
        /// Method Description		: POST API method for log file analsis
        /// Requirement Id   		: 
        /// Method input Parameters	: 
        /// Param Name	            : emailParams - request body required for email body
        ///                           key - API key for authentication
        /// Method Return Parameter	: IActionResult - response to server
        /// </summary>
        [HttpPost]
        public IActionResult StartLogFileAnalysis([FromBody] EmailParams emailParams, [FromHeader] ResponseHeader key)
        {
            try
            {
                /* Variable to get the Authentication key from server header */
                string apiKey = key.Authorization;

                if (emailParams == null)
                {
                    return BadRequest("Require request-body to process");
                }//End of if (emailParams == null)

                if (string.IsNullOrEmpty(apiKey))
                {
                    return NotFound("API not found");
                }//End of if (string.IsNullOrEmpty(apiKey))

                if (!apiKey.Equals(Settings.API_KEY_VALUE))
                {
                    return Unauthorized("Unauthorized - Invalid API Key");
                }//End of if (!apiKey.Equals(Settings.API_KEY_VALUE))

                /* Invoke StartLogFileAnalysis() to start the log file analysis */
                FilterLogMessages logFileErrorAnalysis = new FilterLogMessages();
                logFileErrorAnalysis.StartLogFileAnalysis(emailParams);

                if (!string.IsNullOrEmpty(ExceptionOccurred))
                {
                    return StatusCode(500, "An internal server error occurred. " + ExceptionOccurred);
                }
                else
                {
                    /* Send response back to front end (UI) */
                    return Ok("Data received and processed successfully!");
                }
            }//End of try
            catch (Exception exe)
            {
                AnalyseLogFile.ExceptionOccurred = exe.Message;
                return Content("An error occurred: " + exe.Message);
            }//End of catch (Exception exe)
        }//End of public IActionResult StartLogFileAnalysis([FromBody] EmailParams emailParams, [FromHeader] ResponseHeader key)
    }//End of public class AnalyseLogFile : Controller
}//End of namespace LogFileAnalysis.Controllers
