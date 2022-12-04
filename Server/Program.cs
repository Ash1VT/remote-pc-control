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

            ConfigReader configReader = new ConfigReader("config.ini");
            _server = new Server(configReader.ServerAddress, configReader.ServerPort, true);
            _server.Start();
            
            ScreenManager screenManager = new ScreenManager(true);
            screenManager.ScreenChanged += Manager_ScreenChanged;
            screenManager.Start();

            while (true) { }
        }   

        private static bool Manager_ScreenChanged(Image screen)
        {
            ScreenResponse screenResponse = new ScreenResponse(screen);
            return _server.SendResponse(screenResponse);
        }

    }
}