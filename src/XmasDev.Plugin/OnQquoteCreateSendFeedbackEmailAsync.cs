﻿using Microsoft.Crm.Sdk.Messages;
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
            var quote = context.InputParameters.ContainsKey("Target") ? context.InputParameters["Target"] as Entity : null;
            var service = factory.CreateOrganizationService(null);

            // Gate checks
            if (context.Depth > 1 || !context.PrimaryEntityName.Equals("xms_gift") || quote == null)
                return;

            // Gift references
            var goodKid = quote.GetAttributeValue<EntityReference>("xms_kid");
            var product = quote.GetAttributeValue<EntityReference>("xms_product");

            var userName = service.Retrieve(goodKid.LogicalName, goodKid.Id, new ColumnSet("governmentid")).GetAttributeValue<string>("governmentid");
            var productName = service.Retrieve(product.LogicalName, product.Id, new ColumnSet("productnumber")).GetAttributeValue<string>("productnumber");

            // Email subjects
            var fromParty = new Entity("activityparty");
            fromParty["partyid"] = new EntityReference("systemuser", context.InitiatingUserId);

            var toPartyContacts = new Entity("activityparty");
            toPartyContacts["partyid"] = goodKid;

            // Feedback email entity
            var email = new Entity("email");
            email["from"] = new Entity[] { fromParty };
            email["to"] = new Entity[] { toPartyContacts };
            email["subject"] = $"Feedback requested for gift: {product.Name}";
            email["description"] = $"Please provide your feedback here: https://xmasdevfeedback.azurewebsites.net/Home/Index/{userName}/{productName}";
            var emailId = service.Create(email);

            // Email send
            var request = new SendEmailRequest { EmailId = emailId, IssueSend = true };
            var response = service.Execute(request) as SendEmailResponse;
        }
    }
}
