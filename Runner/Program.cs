using System;

using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Store;
//using Microsoft.Azure.Graph.RBAC;
using System.Threading;
using System.Threading.Tasks;

namespace Runner
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string subscriptionId = "c3ae256d-a545-45f7-a910-1a436bbd6bb7";

            string domain = "5b01151f-91d2-462a-813b-a944c466d4fe";
            var armTokenAudience = new Uri(@"https://management.core.windows.net/");
            var adlTokenAudience = new Uri(@"https://datalake.azure.net/");
            var aadTokenAudience = new Uri(@"https://graph.windows.net/");

            // NON-INTERACTIVE WITH SECRET KEY
            string clientId = "fe7da26c-657a-4685-956d-860af748d838";
            string secretKey = "a1T86XPtikWZFQ8Ej6El9ZeWWB1QdLxd4k6SZzpxkf4=";
            string accountName = "esperimenti";

            var armCreds = GetCredsServicePrincipalSecretKey(domain, armTokenAudience, clientId, secretKey);
            var adlCreds = GetCredsServicePrincipalSecretKey(domain, adlTokenAudience, clientId, secretKey);
            var aadCreds = GetCredsServicePrincipalSecretKey(domain, aadTokenAudience, clientId, secretKey);
            
            // ----------------------------------------
            // Create the REST clients using the credentials
            // ----------------------------------------

            var adlaAccountClient = new DataLakeAnalyticsAccountManagementClient(armCreds);
            adlaAccountClient.SubscriptionId = subscriptionId;

            var adlsAccountClient = new DataLakeStoreAccountManagementClient(armCreds);
            adlsAccountClient.SubscriptionId = subscriptionId;

            var adlaCatalogClient = new DataLakeAnalyticsCatalogManagementClient(adlCreds);
            var adlaJobClient = new DataLakeAnalyticsJobManagementClient(adlCreds);
            var adlsFileSystemClient = new DataLakeStoreFileSystemManagementClient(adlCreds);

            try
            {
                
                var re = adlaCatalogClient.Catalog.GetDatabase(accountName, "master");
                var assemblies = await adlaCatalogClient.Catalog.ListAssembliesAsync(accountName, "master");
                var jobs = await adlaJobClient.Job.ListAsync(accountName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
            //var account = adlaAccountClient.Account.Get(resourceGroupName, adlaAccountName);
            //Console.WriteLine($"My account's location is: {account.Location}!");

            Console.ReadLine();
        }

        private static ServiceClientCredentials GetCredsServicePrincipalSecretKey(string domain, Uri tokenAudience, string clientId, string secretKey)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var serviceSettings = ActiveDirectoryServiceSettings.Azure;
            serviceSettings.TokenAudience = tokenAudience;

            var creds = ApplicationTokenProvider.LoginSilentAsync(domain, clientId, secretKey, serviceSettings).GetAwaiter().GetResult();

            return creds;
        }
    }
}
