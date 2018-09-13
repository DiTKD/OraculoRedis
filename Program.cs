using System;
using System.Net;
using StackExchange.Redis;

namespace ConsoleApp1
{
    class Program
    {

        public static RedisChannel canal = "ch1";
        public static IConnectionMultiplexer client = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            
            var db = client.GetDatabase();

            var sub = client.GetSubscriber();
            sub.Subscribe(canal, (ch, msg) =>
            {
                if(msg.ToString().StartsWith("p"))
                    GeraResposta(msg.ToString());
            });

            Console.ReadKey();
        }

        public static void GeraResposta(string msg)
        {
            var pub = client.GetSubscriber();
            var resposta = getRespostaGoogle(msg);
            pub.Publish(canal, resposta);
        }

        public static string getRespostaGoogle(string msg)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Only a test!");

            string apiKey = "AIzaSyDiRCI-3Djsw8qTrXIYF7AIqnCSyyHmFmM";
            string cx = "009431853094902135308:axmjfgvmaam";


               //var results = webClient.DownloadString(String.Format("https://www.google.com.au/search?q={0}&alt=json", msg));
            var results = webClient.DownloadString(String.Format("https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}&alt=json", apiKey, cx, msg));

            // web.Dispose();
            return results;
        }
    }
}
