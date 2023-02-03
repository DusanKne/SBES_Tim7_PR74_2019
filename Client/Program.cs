using System;
using System.ServiceModel;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/TimerService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address));
            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
                string input = "";

                while(true)
                {
                    Console.WriteLine("Welcome to the Timer Service. Please pick an option from below");
                    Console.WriteLine("1. Start the timer.");
                    Console.WriteLine("2. Stop the timer.");
                    Console.WriteLine("3. Cancel the timer.");
                    Console.WriteLine("4. Set the timer time.");
                    Console.WriteLine("5. Read the timer.");
                    Console.WriteLine("Q. Exit");
                    input = Console.ReadLine();

                    if(input == "Q")
                    {
                        Console.WriteLine("Closing the connection, press any key to confirm.");
                        Console.ReadKey();
                        break;
                    }

                    switch (input)
                    {
                        case "1":
                            proxy.StartTimer();
                            break;
                        case "2":
                            proxy.StopTimer();
                            break;
                        case "3":
                            proxy.CancelTimer();
                            break;
                        case "4":
                            Console.WriteLine("Input the number of seconds you want the timer to have");
                            int secs;
                            input = Console.ReadLine();
                            bool isDouble = Int32.TryParse(input, out secs);

                            if (isDouble)
                            {
                                proxy.SetTimer(TimeSpan.FromSeconds(secs).TotalMilliseconds.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Input error.");
                            }
                            break;
                        case "5":
                            TimeSpan ts = TimeSpan.FromMilliseconds(proxy.ReadTimer());
                            Console.WriteLine($"Time left: {ts.ToString(@"hh\:mm\:ss")}");
                            break;
                        default:
                            Console.WriteLine("Input error, please try again.");
                            break;
                    }



                }
            }


        }
    }
}
