using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// UDP-Chatin asiakassovellus
/// </summary>
namespace Chatasiakas
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket soketti = null;
            Console.WriteLine("Chatasiakassovellus UDP");

            soketti = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            int port = 9999;

            System.Net.IPEndPoint iep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, port); // IPEndPoint luokan olio, alustettu arvoon null

            //IPEndPoint sender = new IPEndPoint(IPAddress.Loopback, 0); // Vastaanotetun paketin lähettäjä
            EndPoint senderRemote = (EndPoint)iep; // Typecast EndPoint:ksi
            soketti.ReceiveTimeout = 1000; // Lopetetaan jos palvelin ei lähetä mitään 3000 ms
            byte[] rec = new byte[2048]; // Tavutaulukko vastaanottoa varten
                                        
            String viesti = "";
            Boolean jatketaanko = true;
            do
            {
                Console.Write(">");
                viesti = Console.ReadLine();
                if (viesti.Equals("sulje"))
                {
                    jatketaanko = false;
                }
                else
                {
                    soketti.SendTo(Encoding.ASCII.GetBytes(viesti), senderRemote);

                    while (!Console.KeyAvailable)
                    {
                        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                        EndPoint PalvelinEP = (EndPoint)remote;

                        try
                        {
                            int received = soketti.ReceiveFrom(rec, ref PalvelinEP);
                            String vastString = Encoding.ASCII.GetString(rec, 0, received);
                            char[] delim = { ';' };
                            String[] palat = vastString.Split(delim, 2);
                            if (palat.Length < 2)
                            {
                                Console.WriteLine("Virheellisen muotoinen viesti, anna viesti muotoa (lähettäjä;viesti)");
                            }
                            else
                            {
                                Console.WriteLine("{0}: {1}", palat[0], palat[1]);
                            }
                        }
                        catch
                        {
                            // timeout
                        }
                    }
                }
            } while (jatketaanko);

            soketti.Close();

        }
    }
}
