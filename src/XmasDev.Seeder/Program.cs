using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;
using System.Linq;

namespace XmasDev.Seeder
{
    class Program
    {
        private static Random m_rand = new Random();
        static void Main(string[] args)
        {
            var client = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString);

            // Possible options
            var kids = new string[] { "KID01", "KID02" };
            var gifts = new string[]
            {
                "001", "002", "003", "004","005", "006", "007", "008","009", "010",
                "011", "012","013", "014", "015", "016","017", "018", "019"
            };

            // Randomizers
            foreach (var iteration in Enumerable.Range(0, 4000))
            {
                var kid = kids[m_rand.Next(0, 1)];
                var gift = gifts[m_rand.Next(0, 18)];
                var rate = m_rand.Next(1, 5);

                var entity = new Entity("xms_feeback");
                entity["xms_name"] = $"Feedback for iteration #{iteration}";
                entity["xms_usercode"] = kid;
                entity["xms_productcode"] = gift;
                entity["xms_rating"] = rate;

                client.Create(entity);
            }
        }
    }
}
