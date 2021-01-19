using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// TCP Kaikuasiakassovellus
/// </summary>
namespace KaikuAsiakas
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket soketti = null;
            NetworkStream ns = null;
            StreamReader sr = null;
            StreamWriter sw = null;

            try
            {
                Console.WriteLine("Kaikuasiakassovellus");
            
                soketti = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
                String ip = "127.0.0.1";
                IPAddress osoite = IPAddress.Parse(ip);

                soketti.Connect(osoite, 25000);

                ns = new NetworkStream(soketti);
                sr = new StreamReader(ns);

                sw = new StreamWriter(ns);
                String viesti = "";
                String vastaus = "";
                string[] osat = new string[2];
                Boolean jatketaanko = true;

                while (jatketaanko)
                {
                    Console.Write("Kirjoita viesti: ");
                    viesti = Console.ReadLine();
                    sw.WriteLine(viesti);
                    sw.Flush();
                    
                    vastaus = sr.ReadLine();
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
