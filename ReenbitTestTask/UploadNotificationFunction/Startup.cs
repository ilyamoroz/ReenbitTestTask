using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using UploadNotificationFunction.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(UploadNotificationFunction.Startup))]
namespace UploadNotificationFunction
{
	public class Startup : FunctionsStartup
	{
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddScoped<ISASService,SASService>();
        }
    }
}

