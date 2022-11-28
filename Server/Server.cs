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

        public async Task AcceptClient()
        {
            _client = await _listener.AcceptAsync(); 
            Console.WriteLine("USER CONNECTED");
        }
        
        public void StartAcceptRequests()
        {

            Task.Run(HandleIncomingRequest);
            
            //new Task(() =>
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            AweSock.ReceiveMessage(_client, _inBuffer);

            //            new Task( () =>
            //            {
            //                string data = Buffer.Get<string>(_inBuffer);
            //                //string data = System.Text.Encoding.Default.GetString(_inBuffer);


            //                string[] requests = data.Split('\0');
            //                foreach (string stringRequest in requests)
            //                {
            //                    JObject jObject = JObject.Parse(stringRequest);
            //                    Request request = RequestIdentifier.GetRequest(jObject);
            //                    request.Execute();
            //                    Response response = request.GetResponse();
            //                    SendResponse(response);
            //                }
            //            }).Start();
                        
            //        }
            //        catch (System.IO.IOException e)
            //        {
            //            Console.WriteLine("USER DISCONNECTED");
            //        }
            //    }
            //}).Start();
            
        }



        public async Task<bool> SendResponse(Response response)
        {



            //byte[] bytes = System.Text.Encoding.Default.GetBytes($"{stringResponse}{String.Concat(IEnumerator("\0", 300000 - stringResponse.Length))}");
            //_client.Send(bytes);
            if (response == null)
                return false;
            string stringResponse = response.ToJson().ToString();
            byte[] bytes = new byte[330000];
            //ArraySegment<byte> bytes = new ArraySegment<byte>(new byte[170000]);
            Buffer.BlockCopy(System.Text.Encoding.Default.GetBytes(stringResponse), 0, bytes, 0, stringResponse.Length);
            ArraySegment<byte> sendingBytes = new ArraySegment<byte>(bytes);
            //string sendResponse = $"{stringResponse}{String.Concat(Enumerable.Repeat("\0", 170000 - stringResponse.Length))}";
            //ArraySegment<byte> bytes = new ArraySegment<byte>(System.Text.Encoding.Default.GetBytes(sendResponse));
            int sent = await _client.SendAsync(sendingBytes, SocketFlags.None);
            //Console.WriteLine($"Sent {sent}");
            return true;
        }
        
        private async Task HandleIncomingRequest()
        {
            ArraySegment<byte> incomingData = new ArraySegment<byte>(new byte[200]);
            var res = await _client.ReceiveAsync(incomingData, SocketFlags.None);
            Task.Run(HandleIncomingRequest);
            string answer = System.Text.Encoding.Default.GetString(incomingData.Array);

            JObject jObject = JObject.Parse(answer);
            Request request = RequestIdentifier.GetRequest(jObject);
            request.Execute();
            Console.WriteLine(jObject.ToString());
            Response response = request.GetResponse();
            SendResponse(response);
        }
        
        public void Stop()
        {
            _listener.Close();
            Console.WriteLine("LISTENER STOPPED");
        }
    }
}