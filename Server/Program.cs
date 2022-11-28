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
        public static async Task Main(string[] args)
        {

            // MemoryStream stream = new MemoryStream();
            // Image image = ScreenMaker.Capture(0,0,1920,1080);
            // byte[] bytes = imageToByteArray(image);
            //
            // string data = System.Text.Encoding.UTF8.GetString(bytes);
            //
            // byte[] bytes1 = System.Text.Encoding.UTF8.GetBytes(data);
            //
            // Image image1 = byteArrayToImage(bytes1);
            // image1.Save("bui", ImageFormat.Jpeg);

            //
            // JObject jObject = new JObject();
            // jObject.Add("Screen", data);
            //qq
            // string data1 = jObject["Screen"].ToString();
            //
            // Console.WriteLine(data == data1);
            // Print result as JSON

            _server = new Server(IPAddress.Parse("192.168.0.103"), 13000);
            _server.Start();
            
            await _server.AcceptClient();
            _server.StartAcceptRequests();
            ScreenManager screenManager = new ScreenManager();
            screenManager.ScreenChanged += Manager_ScreenChanged;
            screenManager.Start();


            while (true) { }

            //while (true)
            //{
            //    server.SendResponse(new TestResponse());
            //}


            // TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);
            // listener.Start();
            //
            // TcpClient client = listener.AcceptTcpClient();
            // NetworkStream stream = client.GetStream();
            //
            //
            // while (true)
            // {
            //     Byte[] bytes = new Byte[256];
            //     int i = stream.Read(bytes, 0, bytes.Length);
            //     String data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            //     
            //     Console.WriteLine("Received: {0}", data);
            // }

        }   

        private static async Task Manager_ScreenChanged(Image screen)
        {

            //TestResponse testResponse = new TestResponse();
            //_server.SendResponse(testResponse);
            ScreenResponse screenResponse = new ScreenResponse(screen);
            await _server.SendResponse(screenResponse);
            //Console.WriteLine($"X: {startPoint.X} Y: {startPoint.Y}");
            //Console.WriteLine($"Width: {changedPart.Width} Height: {changedPart.Height}");
            //Console.WriteLine();
        }

  
    }
}