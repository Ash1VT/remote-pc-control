using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
        private TcpClient _client1;
        private TcpClient _client2;


        private NetworkStream _stream;
        private NetworkStream _stream1;
        private NetworkStream _stream2;

        
        
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
            _client1 = _listener.AcceptTcpClient();
            _client2 = _listener.AcceptTcpClient();

            GetStream();
            Console.WriteLine("USER CONNECTED");
        }

        private void GetStream()
        {
            _stream = _client.GetStream();
            _stream1 = _client1.GetStream();
            _stream2 = _client2.GetStream();
        }
        
        public void Run()
        {
            AcceptClient();
            
            Semaphore sem = new Semaphore(1, 1);
            
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        Byte[] bytes = new Byte[256];
                        int i = _stream.Read(bytes, 0, bytes.Length);
                        string data = System.Text.Encoding.Default.GetString(bytes, 0, i);
                        
                        JObject jObject = JObject.Parse(data);

                        Request request = RequestIdentifier.GetRequest(jObject);

                        sem.WaitOne();
                        request.Execute();
                        sem.Release();
                        Console.WriteLine("STREAM");
                        Response response = request.GetResponse();

                        string data1 = response.ToJson().ToString();
                        
                        byte[] bytes1 = System.Text.Encoding.Default.GetBytes(data1);
                        _stream.Write(bytes1, 0, bytes1.Length);
                        
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine("USER DISCONNECTED");
                        AcceptClient();
                    }
                }
            }).Start();
            
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        // string data = ReadMessage();
                        
                        Byte[] bytes = new Byte[256];
                        int i = _stream1.Read(bytes, 0, bytes.Length);
                        string data = System.Text.Encoding.Default.GetString(bytes, 0, i);
                        
                        JObject jObject = JObject.Parse(data);
            
                        Request request = RequestIdentifier.GetRequest(jObject);
                        sem.WaitOne();
                        request.Execute();
                        sem.Release();

                        Console.WriteLine("STREAM1");
                        Response response = request.GetResponse();
            
                        string data1 = response.ToJson().ToString();
                        
                        byte[] bytes1 = System.Text.Encoding.Default.GetBytes(data1);
                        _stream1.Write(bytes1, 0, bytes1.Length);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine("USER DISCONNECTED");
                        AcceptClient();
                    }
                }
            }).Start();
            
            
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        // string data = ReadMessage();
                        
                        Byte[] bytes = new Byte[256];
                        int i = _stream2.Read(bytes, 0, bytes.Length);
                        string data = System.Text.Encoding.Default.GetString(bytes, 0, i);
                        
                        JObject jObject = JObject.Parse(data);
            
                        Request request = RequestIdentifier.GetRequest(jObject);
                        sem.WaitOne();
                        request.Execute();
                        sem.Release();

                        Console.WriteLine("STREAM2");

                        Response response = request.GetResponse();
            
                        string data1 = response.ToJson().ToString();
                        
                        byte[] bytes1 = System.Text.Encoding.Default.GetBytes(data1);
                        _stream2.Write(bytes1, 0, bytes1.Length);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine("USER DISCONNECTED");
                        AcceptClient();
                    }
                }
            }).Start();
            
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