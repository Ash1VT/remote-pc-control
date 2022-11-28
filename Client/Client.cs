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

        public Client()
        {
        }

        public async Task Connect(string serverAddress, int serverPort)
        {
            await _server.ConnectAsync(serverAddress, serverPort);
            Console.WriteLine($"CONNECTED TO {serverAddress}:{serverPort}");
        }
    

        public async Task<bool> SendRequest(Request request)
        {
            try
            {
                if (request == null) 
                    return false;
                string stringRequest = request.ToJson().ToString();
                byte[] bytes = new byte[200];
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
            Task.Run(HandleIncomingResponse);
        }

        private async Task HandleIncomingResponse()
        {
            ArraySegment<byte> answerData = new ArraySegment<byte>(new byte[330000]);
            var res = await _server.ReceiveAsync(answerData, SocketFlags.None);
            Task.Run(HandleIncomingResponse);
            string answer = System.Text.Encoding.Default.GetString(answerData.Array);
            try
            {
                JObject jObject = JObject.Parse(answer);

                Response response = ResponseIdentifier.GetResponse(jObject);
                response.Execute();
            }
            catch
            {
            }
        }
        public void Disconnect()
        {
            _server.Close();
            Console.WriteLine("DISCONNECTING");
        }
        
        
    }
}