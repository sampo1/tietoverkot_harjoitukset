using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// SMTPAsiakassovellus
/// </summary>
public class SMTPAsiakas
{
    public static void Main()
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            s.Connect("localhost", 25000);
        }
        catch (Exception ex)
        {
            Console.Write("Virhe " + ex.Message);
            return;
        }
        NetworkStream ns = new NetworkStream(s);

        StreamReader sr = new StreamReader(ns);
        StreamWriter sw = new StreamWriter(ns);

        Console.ReadKey();

        Boolean on = true;
        String viesti = "";
        Console.WriteLine(viesti);

        while(on)
        {
            viesti = sr.ReadLine();
            String[] status = viesti.Split(" ");
            
            switch (status[0])
            {
                case "220":
                    sw.WriteLine("HELO jyu.fi");
                    break;
                case "250":
                    switch (status[1])
                    {
                        case "2.0.0":
                            sw.WriteLine("QUIT");
                            on = false;
                            break;
                        case "2.1.0":
                            sw.WriteLine("RCPT TO: vastaanottaja");
                            break;
                        case "2.1.5":
                            sw.WriteLine("DATA ");
                            break;
                        default:
                            sw.WriteLine("MAIL FROM: lahettaja");
                            //Console.Write("Virhe status 1");
                            break;
                    }
                    break;
                case "354":
                    sw.Write("viesti \r\n.\r\n");
                    break;
                case "221":
                    on = false;
                    //lahetettava = "END";
                    break;
                default:
                    Console.Write("Virhe status 0");
                    sw.WriteLine("QUIT");
                    break;
            }
            sw.Flush();
        }

        sw.Close();
        sr.Close();
        ns.Close();
        s.Close();
    }

    /// <summary>
    /// Funktio palauttaa SMTP asiakkaan viestin palvelimen viestin perusteella.
    /// </summary>
    /// <param name="viesti">string SMTP palvelimen viesti</param>
    /// <returns>string SMTP asiakkaan viesti</returns>
    public static string smtpAsiakasali(string vastaanotettu)
    {

        string lahetettava = "";
        Console.Write(vastaanotettu);
        String[] status = vastaanotettu.Split(" ");
        switch (status[0])
        {
            case "220":
                lahetettava = "HELO jyu.fi\r\n";
                break;
            case "250":   
                switch(status[1])
                {
                    case "2.0.0":
                        lahetettava = "QUIT\r\n";
                        break;
                    case "2.1.0":
                        lahetettava = "RCPT TO: vastaanottaja\r\n";
                        break;
                    case "2.1.5":
                        lahetettava = "DATA \r\n";
                        break;
                    default:
                        lahetettava = "MAIL FROM: lahettaja\r\n";
                        //Console.Write("Virhe status 1");
                        break;
                }
                break;
            case "354":
                lahetettava = "viesti \r\n.\r\n";
                break;
            case "221":
                lahetettava = "END";
                break;
            default:
                Console.Write("Virhe status 0");
                break;
        }
        Console.Write(lahetettava);
        return lahetettava;
    }



}
