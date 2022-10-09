using System;
using System.Drawing;
using System.Drawing.Imaging;

using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace Server
{
    
   
    
    internal class Program
    {
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms, true, false);
            return returnImage;
        }
        
        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream memoryStream = new MemoryStream();
            {
                imageIn.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return memoryStream.ToArray();
        }
        
        public static void Main(string[] args)
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
            //
            // string data1 = jObject["Screen"].ToString();
            //
            // Console.WriteLine(data == data1);

// Print result as JSON
            Server server = new Server(IPAddress.Parse("127.0.0.1"), 13000);
            server.Start();
            server.Run();
            server.Stop();



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
    }
}