using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using AwesomeSockets.Buffers;
using Newtonsoft.Json.Linq;
using Server.Requests;
using Server.Responses;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace Server
{
    public class Server
    {
        private ISocket _listener;
        private ISocket _client;

        private int _port;

        private Buffer _inBuffer;
        private Buffer _outBuffer;
        //private Socket _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //private Socket _client = null;

        
        
        public Server(IPAddress serverAddress, int serverPort)
        {
            //EndPoint serverEndPoint = new IPEndPoint(serverAddress, serverPort);
            _port = serverPort;
            _inBuffer = Buffer.New(20000);
            _outBuffer = Buffer.New(300000);
        }


        public void Start()
        {
            _listener = AweSock.TcpListen(_port);
            Console.WriteLine("STARTED LISTENER");
        }

        public void AcceptClient()
        {
            _client = AweSock.TcpAccept(_listener);
            Console.WriteLine("USER CONNECTED");
        }
        
        public void StartAcceptRequests()
        {
            
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        AweSock.ReceiveMessage(_client, _inBuffer);

                        new Task( () =>
                        {
                            string data = Buffer.Get<string>(_inBuffer);
                            //string data = System.Text.Encoding.Default.GetString(_inBuffer);


                            string[] requests = data.Split('\0');
                            foreach (string stringRequest in requests)
                            {
                                JObject jObject = JObject.Parse(stringRequest);
                                Request request = RequestIdentifier.GetRequest(jObject);
                                request.Execute();
                                Response response = request.GetResponse();
                                SendResponse(response);
                            }
                        }).Start();
                        
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine("USER DISCONNECTED");
                    }
                }
            }).Start();
            
        }



        public void SendResponse(Response response)
        {

            
            //string stringResponse = response.ToJson().ToString();
            //byte[] bytes = System.Text.Encoding.Default.GetBytes($"{stringResponse}{String.Concat(IEnumerator("\0", 300000 - stringResponse.Length))}");
            //_client.Send(bytes);
            if (response != null)
            {
                string stringResponse = response.ToJson().ToString();
                Buffer.Add(_outBuffer, stringResponse);
                Buffer.FinalizeBuffer(_outBuffer);
                //byte[] bytes = System.Text.Encoding.Default.GetBytes($"{stringResponse}{String.Concat(Enumerable.Repeat("\0", 400000 - stringResponse.Length))}");
                AweSock.SendMessage(_client, _outBuffer);
                Buffer.ClearBuffer(_outBuffer);

            }
        }

       
        
        public void Stop()
        {
            _listener.Close();
            Console.WriteLine("LISTENER STOPPED");
        }
    }
}