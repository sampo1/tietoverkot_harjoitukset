using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;


/// <summary>
/// UDP Chatpalvelinsovellus
/// </summary>
namespace Kaikupalvelin
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket palvelin = null;
            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, 9999);

            List<EndPoint> asiakkaat = new List<EndPoint>();
            Console.WriteLine("Chatpalvelinsovellus");
            try
            {
                palvelin = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                palvelin.Bind(iep);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine("Virhe... " + ex.Message);
                Console.ReadKey();
                return;
            }

            Console.WriteLine("odotetaan asiakasta...");

            while (!Console.KeyAvailable)
            {
                byte[] rec = new byte[256];
                IPEndPoint asiakas = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)asiakas;
                int received = palvelin.ReceiveFrom(rec, ref remote);

                String vastString = Encoding.ASCII.GetString(rec, 0, received);
                char[] delim = { ';' };
                String[] palat = vastString.Split(delim, 2);
                if (palat.Length < 2)
                {
                    Console.WriteLine("Virheellisen muotoinen viesti, anna viesti muotoa (lähettäjä;viesti)");
                }
                else
                {
                    if (!asiakkaat.Contains(remote))
                    {
                        asiakkaat.Add(remote);
                        Console.WriteLine("Uusi asiakas: [{0}:{1}]", ((IPEndPoint)remote).Address, ((IPEndPoint)remote).Port);
                    }
                    Console.WriteLine("{0}: {1}", palat[0], palat[1]);
                    foreach(EndPoint client in asiakkaat)
                    {
                        palvelin.SendTo(Encoding.ASCII.GetBytes(vastString), client);
                    }
                }
            }

            Console.ReadKey();
            palvelin.Close();
            
        }
    }
}

