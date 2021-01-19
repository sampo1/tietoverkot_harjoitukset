using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// UDP kaikuasiakassovellus
/// </summary>
namespace KaikuAsiakas
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket soketti = null;

            try
            {
                Console.WriteLine("Kaikuasiakassovellus UDP");
            
                soketti = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                System.Net.IPEndPoint iep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 8888); // IPEndPoint luokan olio, alustettu arvoon null

                IPEndPoint sender = new IPEndPoint(IPAddress.Loopback, 0); // Vastaanotetun paketin lähettäjä
                EndPoint senderRemote = (EndPoint)sender; // Typecast EndPoint:ksi
                byte[] rec = new byte[2048]; // Tavutaulukko vastaanottoa varten

                String viesti = "";
                String vastaus = "";
                string[] osat = new string[2];
                Boolean jatketaanko = true;

                while (jatketaanko)
                {
                    Console.Write("Kirjoita viesti: ");
                    viesti = Console.ReadLine();

                    byte[] tavuina = Encoding.ASCII.GetBytes(viesti);
                    soketti.SendTo(tavuina, iep);
                    

                    int paljon = soketti.ReceiveFrom(rec, ref senderRemote);
                    soketti.ReceiveTimeout = 3000; // Lopetetaan jos palvelin ei lähetä mitään 3000 ms
                    vastaus = System.Text.Encoding.ASCII.GetString(rec, 0, paljon);
                    osat = vastaus.Split(";");
                    if (osat[1] == "sulje") jatketaanko = false;
                    
                    Console.WriteLine("Palvelin: " + osat[0]);
                    Console.WriteLine("Teksti: " + osat[1]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (soketti != null)
                {
                    // pyrkii varmistamaan että molemmat osapuolet
                    // ehtivät lähettää kaiken datan
                    if (soketti.Connected) soketti.Shutdown(SocketShutdown.Both);
                    // suljetaan soketti ja vapautetaan resurssit
                    soketti.Close();
                    Console.WriteLine("Soketti suljettiin onnistuneesti");
                }
            }
        }
    }
}
