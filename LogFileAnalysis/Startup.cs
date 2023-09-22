/**********************************************************************************
**  File Name   : Startup.cs                                                     **
**                                                                               **
**  Purpose     : This class file is used to configures the essential components **
**                of the ASP.NET Core application, such as services, middleware, **
**                and routing.                                                   **
**                                                                               **
**********************************************************************************/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using System;
using Microsoft.OpenApi.Models;
using LogFileAnalysis.Constants;

namespace LogFileAnalysis
{
    /// <summary>
    /// Class Name		  : Startup
    /// Class Description : This class configures the application's services, middleware, 
    ///                     and request pipeline.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Method Name				: Startup
        /// Method Description		: This is the default constructor.
        /// Requirement Id   		: 
        /// Method input Parameters	: configuration - configuration settings of the application
        /// Method Return Parameter	: void
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }//End of public Startup(IConfiguration configuration)

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Method Name				: ConfigureServices
        /// Method Description		: This method is used to add services to the dependency injection
        ///                           container.
        /// Requirement Id   		: 
        /// Method input Parameters	: services - collection of services
        /// Method Return Parameter	: void
        /// </summary>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddSwaggerGen();
            // Add Swagger with API key authentication support
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Add the API key security definition
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API key needed to access the endpoints.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Authorization"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            services.AddCors(options =>
            {
                options.AddPolicy(Settings.POLICY_NAME, builder =>
                {
                    builder.WithOrigins(Settings.FRONT_END_CORS_ORIGIN)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration["cubosConnectionString:blob"], preferMsi: true);
                builder.AddQueueServiceClient(Configuration["cubosConnectionString:queue"], preferMsi: true);
            });
        }//End of public void ConfigureServices(IServiceCollection services)

        /// <summary>
        /// Method Name				: Configure
        /// Method Description		: This method is used to configure the HTTP request pipeline.
        /// Requirement Id   		: 
        /// Method input Parameters	: services - collection of services
        /// Method Return Parameter	: void
        /// </summary>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),  
            // specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1.0");
                c.RoutePrefix = string.Empty;
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Use CORS middleware before routing to handle CORS requests
            app.UseCors(Settings.POLICY_NAME);

            // Maps controllers to endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }//End of public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    }//End of public class Startup

    /// <summary>
    /// Class Name		  : StartupExtensions
    /// Class Description : This class contains extension methods to organize code and keep the Startup
    ///                     class clean and concise.. 
    /// </summary>
    internal static class StartupExtensions
    {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }//End of public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)

        public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddQueueServiceClient(serviceUri);
            }
            else
            {
                return builder.AddQueueServiceClient(serviceUriOrConnectionString);
            }
        }//End of public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
    }//End of internal static class StartupExtensions
}//End of namespace LogFileAnalysis
