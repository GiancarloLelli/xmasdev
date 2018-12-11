using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;
using System.Linq;
using System.Net;

namespace XmasDev.Seeder
{
    class Program
    {
        private static Random m_rand = new Random();

        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString);

            // Possible options
            var kids = new string[]
            {
                "BTJWDR97H61C137D", "GTHZDS70E31C141G",  "GDVQVP89M07E873N", "RRTCBD43D58F693T", "KGKTGR58B64M023P", "ZZGBRF53L01H706G",
                "RWQVNZ47C67M282U", "TLTNDG83A54D264V", "GFDRLP87S25A057J", "NKLHDE50A69G661Q", "TRTDBS76E50B477I",
                "PCVMBW31R31E114E", "SVZWFU93P06C807J"
            };

            var gifts = new string[]
            {
                "001", "002", "003", "004","005", "006", "007", "008","009", "010",
                "011", "012","013", "014", "015", "016","017", "018", "019"
            };

            var startDate = new DateTime(2007, 12, 29, 9, 30, 00);

            // Randomizers
            foreach (var iteration in Enumerable.Range(0, 4000))
            {
                var kid = kids[m_rand.Next(0, 12)];
                var gift = gifts[m_rand.Next(0, 18)];
                var rate = m_rand.Next(1, 5);

                var entity = new Entity("xms_feedback");
                entity["xms_name"] = $"Feedback for iteration #{iteration}";
                entity["xms_usercode"] = kid;
                entity["xms_productcode"] = gift;
                entity["xms_rating"] = rate;
                entity["overriddencreatedon"] = startDate.AddDays(iteration);

                client.Create(entity);
                Console.WriteLine($"Created feedback item #{iteration} @ {startDate.ToString("dd-MM-yyyy")}");
            }
        }
    }
}