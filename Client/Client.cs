using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Client.Requests;
using Client.Responses;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class Client
    {
        private Socket _client;
        private int _incomingResponseBytesCount;
        private int _outgoingRequestBytesCount;
        public Client(int incomingResponseBytesCount, int outgoingRequestBytesCount)
        {
            InitClient();
            _incomingResponseBytesCount = incomingResponseBytesCount;
            _outgoingRequestBytesCount = outgoingRequestBytesCount;
        }

        private void InitClient()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        
        public void Connect(string serverAddress, int serverPort)
        {
            _client.Connect(serverAddress, serverPort);
            Console.WriteLine($"CONNECTED TO {serverAddress}:{serverPort}");
        }
    

        public async Task<bool> SendRequest(Request request)
        {
            try
            {
                if (request == null) 
                    return false;

                string stringRequest = request.ToJson().ToString();
                byte[] bytes = new byte[_outgoingRequestBytesCount];
                Buffer.BlockCopy(System.Text.Encoding.Default.GetBytes(stringRequest), 0, bytes, 0, stringRequest.Length);
                ArraySegment<byte> sendingBytes = new ArraySegment<byte>(bytes);

                int sent = await _client.SendAsync(sendingBytes, SocketFlags.None);
                return true;
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("SERVER IS NOT AVAILABLE");
                Disconnect();
                return false;
            }
        }

        public void StartAcceptResponses()
        {
            Task.Run(() => {
                while (_client.Connected)
                {
                    byte[] answerData = new byte[_incomingResponseBytesCount];
                    var res = _client.Receive(answerData, 0, answerData.Length, SocketFlags.None);
                    _ = Task.Run(() => {
                        string answer = System.Text.Encoding.Default.GetString(answerData);

                        JObject jObject = JObject.Parse(answer);

                        Response response = ResponseIdentifier.GetResponse(jObject);
                        response.Execute();
                    });   
                }
            });
        }

        public void Disconnect()
        {
            _client.Close();
            InitClient();
            Console.WriteLine("DISCONNECTING");
        }
        
        
    }
}