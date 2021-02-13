using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace AzureDevopsCICD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((context, builder) =>
             {
                 var azureServiceTokenProvider = new AzureServiceTokenProvider();
                 var keyVaultClient = new KeyVaultClient(
                     new KeyVaultClient.AuthenticationCallback(
                         azureServiceTokenProvider.KeyVaultTokenCallback));

                 builder.AddAzureKeyVault(
                     $"https://cicdkv01.vault.azure.net/",
                     keyVaultClient,
                     new DefaultKeyVaultSecretManager());
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
