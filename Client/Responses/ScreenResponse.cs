using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using LZ4;
using Newtonsoft.Json.Linq;

namespace Client.Responses
{
    public class ScreenResponse : Response
    {
        private Image _screen;
        private int _x;
        private int _y;
        public ScreenResponse(JObject jObject) : base(jObject)
        {
            string screen = jObject["Screen"].ToString();
            byte[] encodedBytes = System.Text.Encoding.Default.GetBytes(screen);
            byte[] decodedBytes = new byte[encodedBytes.Length + 2000000];

            //LZ4Codec.Decode(encodedBytes, 0, encodedBytes.Length, decodedBytes, 0, decodedBytes.Length, false);

            _x = int.Parse(jObject["X"].ToString());
            _y = int.Parse(jObject["Y"].ToString());
            _screen = byteArrayToImage(encodedBytes);
        }
        
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms, true, false);
            return returnImage;
        }
        public override void Execute()
        {
            if (Data.Image == null)
                Data.Image = _screen;
            else
            {
                Graphics gImage = Graphics.FromImage(Data.Image);
                gImage.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                gImage.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gImage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                gImage.DrawImage(_screen, _x, _y, new Rectangle(0, 0, _screen.Width, _screen.Height), GraphicsUnit.Pixel);

                ScreenManager.ChangeScreen(Data.Image);

                gImage.ReleaseHdc();
                gImage.Dispose();
            }
        }

    }
}