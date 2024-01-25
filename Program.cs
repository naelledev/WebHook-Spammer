using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

internal class Program
{
    public static string WebHookAddress = "";

    public static int Counter = 0;

    public static string Everyone = "";

    public static string Message = "";

    private static void Main()
    {
        Console.Clear();
        Console.Title = "Discord WebHook Spammer by naelledev";
        Console.WriteLine(Utils.CenterText("https://github.com/naelledev"));
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("\n\t\t\t\t\t WebHook Address : ");
        WebHookAddress = Console.ReadLine();
        Console.Write("\n\t\t\t\t\t\tMessage : ");
        string message = Console.ReadLine();
        Console.Write("\n\t\t\t\t\tPing Everyone? (Y/N) : ");
        Everyone = Console.ReadLine();
        if (Everyone == "Y")
        {
            message = message + "@everyone";
        }
        else if (Everyone == "N")
        {
            message = Message;
        }
        else if (Everyone == "y")
        {
            message = message + "@everyone";
        }
        else if (Everyone == "n")
        {
            message = Message;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(Utils.CenterText("ERROR : WRONG INPUT!"));
            Thread.Sleep(2000);
            Main();
        }
        Console.Write("\n\t\t\t\t\tHow many requests do you want to send? : ");
        int Requests = int.Parse(Console.ReadLine());

        for (int i = 0; i < Requests; i++)
        {
            Counter++;
            SendMs(message);

            if (IsRateLimited())
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nERROR! : RATE LIMITED");
            }

            else
            {
                Console.Title = "Requests : " + Counter;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSUCCESS!");
            }
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(Utils.CenterText("\nAll Requests have been sent! \nClick Enter to Go to The Menu!"));
        Console.ReadKey();
        Main();
    }

    private static void SendMs(string message)
    {
        try
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json");
            string s = "{\"content\": \"" + message + "\"}";
            webClient.UploadData(WebHookAddress, Encoding.UTF8.GetBytes(s));
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(Utils.CenterText($"\nERROR WHILE SENDING MESSAGE : {ex.Message}"));
        }
    }

    private static bool IsRateLimited()
    {
        try
        {
            HttpWebResponse lastResponse = (HttpWebResponse)GetLastResponse();

            if (lastResponse != null &&
                lastResponse.Headers.AllKeys.Contains("X-RateLimit-Remaining") &&
                lastResponse.Headers.AllKeys.Contains("X-RateLimit-Reset"))
            {
                int remainingAttempts = int.Parse(lastResponse.Headers["X-RateLimit-Remaining"]);
                int resetTimeInSeconds = int.Parse(lastResponse.Headers["X-RateLimit-Reset"]);

                if (remainingAttempts == 0)
                {
                    Console.WriteLine($"RATE LIMITED! RESET : {resetTimeInSeconds} SECONDS!");
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(Utils.CenterText($"ERROR WHILE CHECKING RATE LIMIT : {ex.Message}"));
        }

        return false;
    }

    private static WebResponse GetLastResponse()
    {
        return null;
    }
}