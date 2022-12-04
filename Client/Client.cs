using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Client.Requests;
using Client.Responses;
using Newtonsoft.Json.Linq;


namespace Client
{
    public class Client
    {
        private Socket _client;
        public Client()
        {
            InitClient();
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


        public bool SendRequest(Request request)
        {
            try
            {
                if (request == null) 
                    return false;
                
                string stringRequest = request.ToJson().ToString();
                int sendingBytesCount = stringRequest.Length;
                byte[] sendingBytesCountBytes = new byte[6];

                byte[] bytes = BitConverter.GetBytes(sendingBytesCount);
                Buffer.BlockCopy(bytes, 0, sendingBytesCountBytes, 0, bytes.Length);
                

                _client.Send(sendingBytesCountBytes, SocketFlags.None);
                byte[] sendingBytes = System.Text.Encoding.Default.GetBytes(stringRequest);
                _client.Send(sendingBytes, SocketFlags.None);
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
            Task.Run(Handle);
        }

        private void Handle()
        {
            
            int responseBytesCount = BitConverter.ToInt32(GetResponseBytes(6), 0);
            
            byte[] answerData = GetResponseBytes(responseBytesCount);
            
            Task.Run(Handle);
            string answer = System.Text.Encoding.Default.GetString(answerData);

            JObject jObject = JObject.Parse(answer);

            Response response = ResponseIdentifier.GetResponse(jObject);
            response.Execute();
        }

        private byte[] GetResponseBytes(int responseBytesCount)
        {
            int mustHandle = responseBytesCount;
            int alreadyHandle = 0;
            
            byte[] answerData = new byte[responseBytesCount];

            while (alreadyHandle < mustHandle)
            {
                int receivedBytes = _client.Receive(answerData, alreadyHandle, mustHandle - alreadyHandle, SocketFlags.None);
                alreadyHandle += receivedBytes;
            }

            return answerData;
        }
        public void Disconnect()
        {
            _client.Close();
            InitClient();
            Console.WriteLine("DISCONNECTING");
        }
        
        
    }
}