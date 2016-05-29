using System;
using Microsoft.Owin.Hosting;
using OwinServer.Properties;

namespace OwinServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var url = Settings.Default.ServerUrl;
                WebApp.Start<OwinConfig>(url);
                Console.WriteLine("Owin API started on {0}", url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}