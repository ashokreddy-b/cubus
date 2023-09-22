/**********************************************************************************
**  File Name   : Program.cs                                                     **
**                                                                               **
**  Purpose     : This class file is used to hosting a web server and running    **
**                the web application.                                           **
**                                                                               **
**********************************************************************************/
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LogFileAnalysis
{
    /// <summary>
    /// Class Name		  : Program
    /// Class Description : This class is entry point to the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Method Name				: Main
        /// Method Description		: This method is used to  build the web host 
        ///                           and runs the web application.
        /// Requirement Id   		: 
        /// Method input Parameters	: args - command-line arguments
        /// Method Return Parameter	: void
        /// </summary>
        public static void Main(string[] args)
        {
            /* Build the web host and runs the web application */
            CreateHostBuilder(args).Build().Run();
        }//End of public static void Main(string[] args)

        /// <summary>
        /// Method Name				: CreateHostBuilder
        /// Method Description		: This method is responsible for configuring the web host.
        /// Requirement Id   		: 
        /// Method input Parameters	: args - command-line arguments
        /// Method Return Parameter	: void
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) //sets up a default configuration for the web host, including configuration from environment variables and app settings files
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }//End of public class Program
}//End of namespace LogFileAnalysis
