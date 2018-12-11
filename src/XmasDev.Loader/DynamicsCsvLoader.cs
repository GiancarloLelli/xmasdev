using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Text;
using XmasDev.Loader.Common;
using XmasDev.Loader.Models;

namespace XmasDev.Loader
{
    public static class DynamicsCsvLoader
    {
        [FunctionName("DynamicsCsvLoader")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            // Initialize Dynamics 365 client
            var client = new CrmServiceClient(ConfigurationHelper.ConnectionString);

            // Fetch feedbacks from Dynamics
            var query = new QueryExpression("xms_feedback") { NoLock = true };
            query.ColumnSet = new ColumnSet("xms_usercode", "xms_rating", "xms_productcode", "createdon");
            var records = client.RetrieveMultiple(query);

            // CSV rows
            var builder = new StringBuilder();
            foreach (var feedback in records.Entities)
            {
                var row = new RowModel
                {
                    Product = feedback.GetAttributeValue<string>("xms_productcode"),
                    Rating = feedback.GetAttributeValue<int>("xms_rating"),
                    CreatedOn = feedback.GetAttributeValue<DateTime>("createdon"),
                    User = feedback.GetAttributeValue<string>("xms_usercode")
                };

                builder.AppendLine(row.ToString());
            }

            // Azure upload
            var storageAccount = CloudStorageAccount.Parse(ConfigurationHelper.StorageConnection);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("input-files");

            if (container.CreateIfNotExists())
            {
                var perms = new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container };
                container.SetPermissions(perms);
            }

            var blockBlob = container.GetBlockBlobReference("usage\\interactions.csv");
            blockBlob.UploadText(builder.ToString());
        }
    }
}
