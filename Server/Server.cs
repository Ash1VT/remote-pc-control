using System;
using System.Collections;
using System.Diagnostics;
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

        private EndPoint _serverAdress;

        private bool _autoRestart = false;
        private int _incomingRequestBytesCount;
        private int _outgoingResponseBytesCount;

        public Server(string serverAddress, int serverPort, int incomingRequestBytesCount, int outgoingResponseBytesCount)
        {
            _serverAdress = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
            _incomingRequestBytesCount = incomingRequestBytesCount;
            _outgoingResponseBytesCount = outgoingResponseBytesCount;

            InitListener();
            BindListener();
        }

        public Server(string serverAddress, int serverPort, int incomingRequestBytesCount, int outcomingResponseBytesCount, bool autoRestart)
        {
            _serverAdress = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
            _incomingRequestBytesCount= incomingRequestBytesCount;
            _outgoingResponseBytesCount = outcomingResponseBytesCount;
            _autoRestart = autoRestart;

            InitListener();
            BindListener();
        }

        private void InitListener()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void BindListener()
        {
            _listener.Bind(_serverAdress);
        }

        public void Start()
        {
            StartListener();
            AcceptClient();
            StartAcceptRequests();
        }

        private void StartListener()
        {
            _listener.Listen(1);
            Debug.WriteLine("STARTED LISTENER");
        }

        private void AcceptClient()
        {
            _client = _listener.Accept();
            Debug.WriteLine("CLIENT CONNECTED");
        }
        
        private void StartAcceptRequests()
        {
            Debug.WriteLine("TRYING TO START ACCEPTING REQUESTS FROM CLIENT");

            if(_client == null )
            {
                Debug.WriteLine("CLIENT MUST BE CONNECTED BEFORE STARTING ACCEPTING REQESTS. ABORTING.");
                return;
            }

            Task.Run(async () =>
            {
                while (_client.Connected)
                {
                    ArraySegment<byte> incomingData = new ArraySegment<byte>(new byte[_incomingRequestBytesCount]);
                    var res = await _client.ReceiveAsync(incomingData, SocketFlags.None);

                    _ = Task.Run(() =>
                    {
                        string answer = System.Text.Encoding.Default.GetString(incomingData.Array);

                        JObject jObject = JObject.Parse(answer);
                        
                        Request request = RequestIdentifier.GetRequest(jObject);
                        request.Execute();

                        Response response = request.GetResponse();
                        SendResponse(response);
                    });
                }
            });
            Debug.WriteLine("STARTED ACCEPTING REQUESTS FROM CLIENT");

        }



        public async Task<bool> SendResponse(Response response)
        {
            if (response == null)
                return false;

            try
            {
                string stringResponse = response.ToJson().ToString();
                byte[] bytes = new byte[_outgoingResponseBytesCount];
                Buffer.BlockCopy(System.Text.Encoding.Default.GetBytes(stringResponse), 0, bytes, 0, stringResponse.Length);
                ArraySegment<byte> sendingBytes = new ArraySegment<byte>(bytes);
                int sent = await _client.SendAsync(sendingBytes, SocketFlags.None);
            }
            catch
            {
                Debug.WriteLine("USER IS NOT RESPONDING. DISCONNECTING.");
                Stop();

                if (_autoRestart)
                {
                    InitListener();
                    BindListener();

                    Start();
                }
                return false;
            }
            return true;
        }
        
        public void Stop()
        {
            _listener.Close();
            Debug.WriteLine("STOPPED LISTENER");
        }
    }
}