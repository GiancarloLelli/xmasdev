using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace XmasDev.Plugin
{
    public class OnQquoteCreateSendFeedbackEmailAsync : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Context data
            var context = serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;
            var factory = serviceProvider.GetService(typeof(IOrganizationServiceFactory)) as IOrganizationServiceFactory;
            var trace = serviceProvider.GetService(typeof(ITracingService)) as ITracingService;
            var quote = context.InputParameters.ContainsKey("Target") ? context.InputParameters["Target"] as Entity : null;
            var service = factory.CreateOrganizationService(context.InitiatingUserId);

            // Gate checks
            if (context.Depth > 1 || !context.PrimaryEntityName.Equals("xms_gift") || quote == null)
            {
                trace.Trace("Error in Gate Checks");
                return;
            }

            // Gift references
            var goodKid = quote.GetAttributeValue<EntityReference>("xms_kid");
            var product = quote.GetAttributeValue<EntityReference>("xms_product");

            var userName = service.Retrieve(goodKid.LogicalName, goodKid.Id, new ColumnSet("governmentid")).GetAttributeValue<string>("governmentid");

            var productEntity = service.Retrieve(product.LogicalName, product.Id, new ColumnSet("productnumber", "name"));
            var productCode = productEntity.GetAttributeValue<string>("productnumber");
            var productName = productEntity.GetAttributeValue<string>("name");

            // Email subjects
            var fromParty = new Entity("activityparty");
            fromParty["partyid"] = new EntityReference("systemuser", context.InitiatingUserId);

            var toPartyContacts = new Entity("activityparty");
            toPartyContacts["partyid"] = goodKid;

            try
            {
                // Feedback email entity
                var email = new Entity("email");
                email["from"] = new Entity[] { fromParty };
                email["to"] = new Entity[] { toPartyContacts };
                email["subject"] = $"Feedback requested for gift: {productName} / ";
                email["description"] = $"Please provide your feedback <a href='https://xmasdevfeedback.azurewebsites.net/Home/Index/{userName}/{productCode}'>here</a>";
                var emailId = service.Create(email);

                // Email send
                var request = new SendEmailRequest { EmailId = emailId, IssueSend = true };
                var response = service.Execute(request) as SendEmailResponse;
            }
            catch (Exception ex)
            {
                trace.Trace(ex.Message);
            }
        }
    }
}
