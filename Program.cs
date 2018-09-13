using System;
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
            pub.Publish(canal, "teste");
        }
    }
}
