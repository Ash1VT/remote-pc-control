using System;
using System.Drawing;
using System.Drawing.Imaging;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Server.DeviceApi;
using Server.Managers;
using Server.Responses;

namespace Server
{
    
   

    public class Program
    {

        private static Server _server;
        public static void Main(string[] args)
        {

            _server = new Server("192.168.0.103", 13000, 200, 230000, true);
            _server.Start();
            
            ScreenManager screenManager = new ScreenManager(true);
            screenManager.ScreenChanged += Manager_ScreenChanged;
            screenManager.Start();

            while (true) { }
        }   

        private static async Task<bool> Manager_ScreenChanged(Image screen)
        {
            ScreenResponse screenResponse = new ScreenResponse(screen);
            return await _server.SendResponse(screenResponse);
        }

  
    }
}