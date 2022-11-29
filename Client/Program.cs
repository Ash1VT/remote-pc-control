using Client.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Client client = new Client(230000, 200);
                client.Connect("192.168.0.103", 13000);
            
                client.StartAcceptResponses();
            
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(client));
            }
            catch
            {
                Console.WriteLine("error");
            }
        }
    }
}