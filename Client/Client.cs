using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Client.Requests;
using Client.Responses;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public Client()
        {
            _client = new TcpClient();
        }

        public void Connect(string serverAddress, int serverPort)
        {
            _client.Connect(serverAddress, serverPort);
            InitStream();
            Console.WriteLine($"CONNECTED TO {serverAddress}:{serverPort}");
        }
        
        private void InitStream()
        {
            _stream = _client.GetStream();
        }

        public bool Send(Request request)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(request.ToJson().ToString());
                _stream.Write(bytes, 0, bytes.Length);

                
                // ANSWER
                byte[] answerData = new byte[5000000];
                string answer = String.Empty;
                
                _stream.Read(answerData, 0, answerData.Length);
                try
                {
                    answer = System.Text.Encoding.Default.GetString(answerData);
                    JObject jObject = JObject.Parse(answer);
                    Response response = ResponseIdentifier.GetResponse(jObject);
                    response.Execute();
                }
                catch
                {
                    Console.WriteLine(answer);
                }




                return true;
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("SERVER IS NOT AVAILABLE");
                Disconnect();
                return false;
            }
        }

        
        public void Disconnect()
        {
            _client.Close();
            Console.WriteLine("DISCONNECTING");
        }
        
        
    }
}