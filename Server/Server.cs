using System;
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
using Newtonsoft.Json.Linq;
using Server.Requests;
using Server.Responses;

namespace Server
{
    public class Server
    {

        private Socket _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket _client = null;

        
        
        public Server(IPAddress serverAddress, int serverPort)
        {
            EndPoint serverEndPoint = new IPEndPoint(serverAddress, serverPort);
            _listener.Bind(serverEndPoint);
        }


        public void Start()
        {
            _listener.Listen(1);
            Console.WriteLine("STARTED LISTENER");
        }

        public void AcceptClient()
        {
            _client = _listener.Accept();

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
                        byte[] bytes = new byte[256];
                        _client.Receive(bytes);

                        new Task( () =>
                        {
                            string data = System.Text.Encoding.Default.GetString(bytes);


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
            byte[] bytes = System.Text.Encoding.Default.GetBytes($"{response.ToJson().ToString()}\0");
            _client.Send(bytes);
        }
        
        public void Stop()
        {
            _listener.Close();
            Console.WriteLine("LISTENER STOPPED");
        }
    }
}