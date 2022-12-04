using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Aspose.Imaging.ImageOptions;
using Newtonsoft.Json.Linq;

namespace Server.Responses
{
    public class ScreenResponse : Response
    {
        private byte[] _screen;
        
        public ScreenResponse(Image screen) : base()
        {
            byte[] initialBytes = ImageUtils.ImageToByteArray(ImageUtils.CompressImage(screen, 1300, 800, 100));

            byte[] decodedBytes = Lz4Net.Lz4.CompressBytes(initialBytes, Lz4Net.Lz4Mode.Fast);
            _screen = decodedBytes;
        }
        
        protected override string GetResponseType()
        {
            return "ScreenResponse";
        }

        public override JObject ToJson()
        {
            JObject jObject = base.ToJson();
            jObject.Add("Screen", System.Text.Encoding.Default.GetString(_screen));

            return jObject;
        }


    }
}