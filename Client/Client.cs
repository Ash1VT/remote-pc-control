using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using AwesomeSockets.Buffers;
using Client.Requests;
using Client.Responses;
using Newtonsoft.Json.Linq;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace Client
{
    public class Client
    {
        //private Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private ISocket _server;

        private Buffer _inBuffer;
        private Buffer _outBuffer;

        public Client()
        {
            _inBuffer = Buffer.New(300000);
            _outBuffer = Buffer.New(20000);
        }

        public void Connect(string serverAddress, int serverPort)
        {
            _server = AweSock.TcpConnect(serverAddress, serverPort);
            //_client.Connect(serverAddress, serverPort);
            Console.WriteLine($"CONNECTED TO {serverAddress}:{serverPort}");
        }
    

        public bool SendRequest(Request request)
        {
            try
            {


                Buffer.ClearBuffer(_outBuffer);
                Buffer.Add(_outBuffer, request.ToJson().ToString());
                Buffer.FinalizeBuffer(_outBuffer);
                AweSock.SendMessage(_server, _outBuffer);
                //byte[] bytes = System.Text.Encoding.Default.GetBytes($"{request.ToJson().ToString()}\0");
                //_server.Send(bytes);

                
                // ANSWER
                //byte[] answerData = new byte[4000000];
                //string answer = String.Empty;
                
                //_stream.Read(answerData, 0, answerData.Length);
                //try
                //{
                    
                //    answer = System.Text.Encoding.Default.GetString(answerData);
                //    JObject jObject = JObject.Parse(answer);
                //    Response response = ResponseIdentifier.GetResponse(jObject);
                //    response.Execute();
                //}
                //catch
                //{
                //    Console.WriteLine(answer);
                //}




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
            new Task(() =>
            {


                while (true)
                {
                    //byte[] answerData = new byte[300000];

                    //_client.Receive(answerData);
                    AweSock.ReceiveMessage(_server, _inBuffer);
                    Buffer.FinalizeBuffer(_inBuffer);
                    
                    //Task.Run(() =>
                    //{

                    string answer = Buffer.Get<string>(_inBuffer);
                        
                    Buffer.ClearBuffer(_inBuffer);
                    //string answer = System.Text.Encoding.Default.GetString(answerData);

                    //string[] responses = answer.Split('\0');
                        
                    JObject jObject = JObject.Parse(answer);

                    Response response = ResponseIdentifier.GetResponse(jObject);
                    response.Execute();

                            

                        //}
                    //});




                }
            }
            ).Start();
        }

        public void Disconnect()
        {
            _server.Close();
            Console.WriteLine("DISCONNECTING");
        }
        
        
    }
}