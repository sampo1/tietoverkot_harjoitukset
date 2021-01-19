using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// UDP kaikupalvelinsovellus
/// </summary>
namespace Kaikupalvelin
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket palvelin = null;
            

                Console.WriteLine("Kaikupalvelinsovellus");

                palvelin = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, 8888);

                palvelin.Bind(iep);


                IPEndPoint sender = new IPEndPoint(IPAddress.Loopback, 0); // Vastaanotetun paketin lähettäjä
                EndPoint senderRemote = (EndPoint)sender; // Typecast EndPoint:ksi
                byte[] rec = new byte[2048]; // Tavutaulukko vastaanottoa varten
                
                String viesti = "";
                while (viesti != "sulje")
                {
                    int paljon = palvelin.ReceiveFrom(rec, ref senderRemote);
                    palvelin.ReceiveTimeout = 3000; // Lopetetaan jos palvelin ei lähetä mitään 3000 ms
                    viesti = System.Text.Encoding.ASCII.GetString(rec, 0, paljon);
                    viesti = "Sampon palvelin;" + viesti;
                    byte[] koodattuna = new byte[paljon + 256];
                    koodattuna = System.Text.Encoding.ASCII.GetBytes(viesti);
                    palvelin.SendTo(koodattuna, senderRemote);
                }
                

                Console.ReadKey();
                palvelin.Close();
                
        }
    }
}

