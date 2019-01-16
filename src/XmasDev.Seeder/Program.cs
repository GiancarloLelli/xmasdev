using System;
using System.IO;
using System.Linq;

namespace XmasDev.Seeder
{
    class Program
    {
        private static Random m_rand = new Random();

        static void Main(string[] args)
        {
            // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // var client = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString);

            // Possible options
            var kids = new string[]
            {
                "BJLRMK67A28B195V", "BVZSCD82E06E251Z",  "DZZYOI85M64I172T", "FDUGJH68H10A606I", "LKVWNC79H68G570H", "MHLCBP74C10L230Q",
                "NVHRCD78M09D590Rv", "PBWDDP73T41A108E", "GFDRLP87S25A057J", "RPMDRG98P67I683V", "TGRMQN98E55H429Y",
                "TQBNSA70R23C443H", "VTGYDT32T21D643S", "XSLTNK44E11C034E"
            };

            var gifts = new string[]
            {
                "001", "002", "003", "004","005", "006", "007", "008","009", "010",
                "011", "012","013", "014", "015", "016","017", "018", "019"
            };

            var events = new string[] { "Click", "RecommendationClick", "AddShopCart", "RemoveShopCart", "Purchase" };

            var startDate = new DateTime(1936, 11, 27, 9, 30, 00);

            // Randomizers
            foreach (var iteration in Enumerable.Range(0, 30000))
            {
                var kid = kids[m_rand.Next(0, 13)];
                var gift = gifts[m_rand.Next(0, 18)];
                var eventType = events[m_rand.Next(0, 4)];
                var createdOnOverride = startDate.AddDays(iteration);

                File.AppendAllText("Transactions.csv", $"{kid},{gift},{createdOnOverride.ToString("yyyy-MM-ddTHH:mm:ss")},{eventType}{Environment.NewLine}");
                Console.WriteLine($"Created feedback item #{iteration} @ {createdOnOverride.ToString("dd-MM-yyyy")} AM");
            }
        }
    }
}