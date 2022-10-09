using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Client.Responses
{
    public class ScreenResponse : Response
    {
        private Image _screen;
        
        public ScreenResponse(JObject jObject) : base(jObject)
        {
            string screen = jObject["Screen"].ToString();
            byte[] bytes = System.Text.Encoding.Default.GetBytes(screen);
            _screen = byteArrayToImage(bytes);
        }
        
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms, true, false);
            return returnImage;
        }
        public override void Execute()
        {
            Data.Image = _screen;
        }

    }
}