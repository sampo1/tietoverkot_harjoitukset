using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// TCP kaikupalvelinsovellus
/// </summary>
namespace Kaikupalvelin
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket palvelin = null;
            

                Console.WriteLine("Kaikupalvelinsovellus");

                palvelin = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, 25000);

                palvelin.Bind(iep);

                palvelin.Listen(5);

                while(true)
                {
                    Socket asiakas = palvelin.Accept();
                    IPEndPoint iap = (IPEndPoint)asiakas.RemoteEndPoint;

                    Console.WriteLine("Yhteys osoitteesta : {0} portista {1}", iap.Address, iap.Port);

                    NetworkStream ns = new NetworkStream(asiakas);

                    StreamWriter sw = new StreamWriter(ns);
                    StreamReader sr = new StreamReader(ns);

                    String rec = sr.ReadLine();
                    sw.WriteLine("Sampon palvelin;" + rec);
                    sw.Flush();
                    asiakas.Close();
                }
                

                Console.ReadKey();
                palvelin.Close();
                
        }
    }
}

