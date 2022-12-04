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
using Timer = System.Timers.Timer;

namespace Server
{
    public class Server
    {
        private Socket _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket _client;

        private EndPoint _serverAdress;

        private bool _autoRestart = false;

        public Server(string serverAddress, int serverPort)
        {
            _serverAdress = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);

            InitListener();
            BindListener();
        }

        public Server(string serverAddress, int serverPort, bool autoRestart)
        {
         
            _serverAdress = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
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

            Task.Run(Handle);
        }



        public bool SendResponse(Response response)
        {
            if (response == null)
                return false;

            try
            {
                
                string stringResponse = response.ToJson().ToString();
                
                int sendingBytesCount = stringResponse.Length;

                byte[] sendingBytesCountBytes = new byte[6];
                byte[] bytes = BitConverter.GetBytes(sendingBytesCount);
                
                Buffer.BlockCopy(bytes, 0, sendingBytesCountBytes, 0, bytes.Length);
                
                //ArraySegment<byte> sendingBytesCountBytes =
                //    new ArraySegment<byte>(temp);
                
                _client.Send(sendingBytesCountBytes, SocketFlags.None);
                byte[] sendingBytes = System.Text.Encoding.Default.GetBytes(stringResponse);
                //ArraySegment<byte> sendingBytes = new ArraySegment<byte>(System.Text.Encoding.Default.GetBytes(stringResponse));
                _client.Send(sendingBytes, SocketFlags.None);
                return true;

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
        }

        private void Handle()
        {
            int requestBytesCount = BitConverter.ToInt32(GetRequestBytes(6), 0);

            byte[] incomingData = GetRequestBytes(requestBytesCount);
            
            Task.Run(Handle);
            
            string answer = System.Text.Encoding.Default.GetString(incomingData);
            JObject jObject = JObject.Parse(answer);
                        
            Request request = RequestIdentifier.GetRequest(jObject);
            request.Execute();

            Response response = request.GetResponse();
            SendResponse(response);
        }
        private byte[] GetRequestBytes(int requestBytesCount)
        {
            int mustHandle = requestBytesCount;
            int alreadyHandle = 0;
            
            byte[] answerData = new byte[requestBytesCount];

            while (alreadyHandle < mustHandle)
            {
                int receivedBytes = _client.Receive(answerData, alreadyHandle, mustHandle - alreadyHandle, SocketFlags.None);
                alreadyHandle += receivedBytes;
            }

            return answerData;
        }
        public void Stop()
        {
            _listener.Close();
            Debug.WriteLine("STOPPED LISTENER");
        }
    }
}