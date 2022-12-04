using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Client.Responses
{
    public class ScreenResponse : Response
    {
        private Image _screen;
        public ScreenResponse(JObject jObject) : base(jObject)
        {
            string screen = jObject["Screen"].ToString();
            byte[] encodedBytes = System.Text.Encoding.Default.GetBytes(screen);
            try
            {
                byte[] decodedBytes = Lz4Net.Lz4.DecompressBytes(encodedBytes);
                _screen = byteArrayToImage(decodedBytes);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

           
        }
        
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms, true, false);
            return returnImage;
        }
        public override void Execute()
        {
            ScreenChanger.ChangeScreen(_screen);
        }

    }
}