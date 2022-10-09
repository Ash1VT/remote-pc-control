using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Server.Requests;
using Server.Responses;

namespace Server
{
    public class Server
    {

        private TcpListener _listener;
        private TcpClient _client;

        private NetworkStream _stream;
        
        
        public Server(IPAddress serverAddress, int serverPort)
        {
            _listener = new TcpListener(serverAddress, serverPort);
        }


        public void Start()
        {
            _listener.Start();
            Console.WriteLine("STARTED LISTENER");
        }

        private void AcceptClient()
        {
            _client = _listener.AcceptTcpClient();
            GetStream();
            Console.WriteLine("USER CONNECTED");
        }

        private void GetStream()
        {
            _stream = _client.GetStream();
        }
        
        public static Bitmap Capture(int x, int y, int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(x, y, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);

                User32.CURSORINFO cursorInfo;
                cursorInfo.cbSize = Marshal.SizeOf(typeof(User32.CURSORINFO));

                if (User32.GetCursorInfo(out cursorInfo))
                {
                    // if the cursor is showing draw it on the screen shot
                    if (cursorInfo.flags == User32.CURSOR_SHOWING)
                    {
                        var iconPointer = User32.CopyIcon(cursorInfo.hCursor);
                        User32.ICONINFO iconInfo;
                        int iconX, iconY;

                        if (User32.GetIconInfo(iconPointer, out iconInfo))
                        {
                            iconX = (int)((cursorInfo.ptScreenPos.x - ((int)iconInfo.xHotspot)) * 1.25);
                            iconY = (int)((cursorInfo.ptScreenPos.y - ((int)iconInfo.yHotspot)) * 1.25);
                            
                            User32.DrawIcon(g.GetHdc(), iconX, iconY, cursorInfo.hCursor);

                            g.ReleaseHdc();
                        }
                    }
                }
            }
            return bitmap;
        }
        public void Run()
        {
            AcceptClient();
            
            while (true)
            {
                try
                {
                    string data = ReadMessage();
                    JObject jObject = JObject.Parse(data);

                    Request request = new ScreenRequest(jObject);
                    
                    request.Execute();

                    Response response = new ScreenResponse(Data.Screen);

                    
                    // MemoryStream memoryStream = new MemoryStream();
                    // {
                    //     Capture(0,0,1920,1080).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    // }
                    
                    string data1 = response.ToJson().ToString();
                    SendAnswer(data1);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine("USER DISCONNECTED");
                    AcceptClient();
                }
            }
        }

        private string ReadMessage()
        {
            
            Byte[] bytes = new Byte[256];
            int i = _stream.Read(bytes, 0, bytes.Length);
            return System.Text.Encoding.Default.GetString(bytes, 0, i);
        }

        private void SendAnswer(string data)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(data);
            _stream.Write(bytes, 0, bytes.Length);
        }
        
        public void Stop()
        {
            _listener.Stop();
            Console.WriteLine("LISTENER STOPPED");
        }
    }
}