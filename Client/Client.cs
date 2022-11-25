using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Client.Requests;
using Client.Responses;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class Client
    {
        private Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Client()
        {
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
                byte[] bytes = System.Text.Encoding.Default.GetBytes($"{request.ToJson().ToString()}\0");
                _client.Send(bytes);

                
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


                while (_client.Connected)
                {
                    byte[] answerData = new byte[300000];

                    _client.Receive(answerData);

                    Task.Run(() =>
                    {
                        string answer = System.Text.Encoding.Default.GetString(answerData);

                        string[] responses = answer.Split('\0');
                        foreach (var stringResponse in responses)
                        {
                            
                                if (stringResponse.Length == 0)
                                {
                                    return;
                                }
                                JObject jObject = JObject.Parse(stringResponse.ToString());

                                Response response = ResponseIdentifier.GetResponse(jObject);
                                response.Execute();

                            

                        }
                    });
                    



                }
            }
            ).Start();
        }

        public void Disconnect()
        {
            _client.Close();
            Console.WriteLine("DISCONNECTING");
        }
        
        
    }
}