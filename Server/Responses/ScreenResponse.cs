using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Server.Responses
{
    public class ScreenResponse : Response
    {

        private byte[] _changedPart;
        private Point _startPoint;
        
        public ScreenResponse(Image changedPart, Point startPoint) : base()
        {
            _changedPart = imageToByteArray(changedPart);
            _startPoint = startPoint;
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream memoryStream = new MemoryStream();
            {
                 imageIn.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return memoryStream.ToArray();
        }
        protected override string GetResponseType()
        {
            return "ScreenResponse";
        }

        public override JObject ToJson()
        {
            JObject jObject = base.ToJson();
            jObject.Add("Screen", System.Text.Encoding.Default.GetString(_changedPart));
            jObject.Add("X", _startPoint.X.ToString());
            jObject.Add("Y", _startPoint.Y.ToString());

            return jObject;
        }
    }
}