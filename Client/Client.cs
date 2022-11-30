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
        private Socket _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private int _incomingResponseBytesCount;
        private int _outgoingRequestBytesCount;
        public Client(int incomingResponseBytesCount, int outgoingRequestBytesCount)
        {
            _incomingResponseBytesCount = incomingResponseBytesCount;
            _outgoingRequestBytesCount = outgoingRequestBytesCount;
        }

        public void Connect(string serverAddress, int serverPort)
        {
            _server.Connect(serverAddress, serverPort);
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

                int sent = await _server.SendAsync(sendingBytes, SocketFlags.None);
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
            Task.Run(async () => {
                while (_server.Connected)
                {
                    ArraySegment<byte> answerData = new ArraySegment<byte>(new byte[_incomingResponseBytesCount]);
                    var res = await _server.ReceiveAsync(answerData, SocketFlags.None);
                    _ = Task.Run(() => {
                        string answer = System.Text.Encoding.Default.GetString(answerData.Array);

                        JObject jObject = JObject.Parse(answer);

                        Response response = ResponseIdentifier.GetResponse(jObject);
                        response.Execute();
                    });   
                }
            });
        }

        public void Disconnect()
        {
            _server.Close();
            Console.WriteLine("DISCONNECTING");
        }
        
        
    }
}