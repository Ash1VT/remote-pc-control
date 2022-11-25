using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Aspose.Imaging.ImageOptions;
using Newtonsoft.Json.Linq;

namespace Server.Responses
{
    public class ScreenResponse : Response
    {

        private byte[] _changedPart;
        private Point _startPoint;
        
        public ScreenResponse(Image changedPart, Point startPoint) : base()
        {
            
            
            _changedPart = imageToByteArray(CompressImage(changedPart, 800, 600, 50));
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
        private Image CompressImage(Image image, int newWidth, int newHeight,
                            int newQuality)   // set quality to 1-100, eg 50
        {
            using (Image memImage = new Bitmap(image, newWidth, newHeight))
            {
                ImageCodecInfo myImageCodecInfo;
                System.Drawing.Imaging.Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, newQuality);
                myEncoderParameters.Param[0] = myEncoderParameter;

                MemoryStream memStream = new MemoryStream();
                memImage.Save(memStream, myImageCodecInfo, myEncoderParameters);
                Image newImage = Image.FromStream(memStream);
                ImageAttributes imageAttributes = new ImageAttributes();
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.InterpolationMode =
                      System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;  //**
                    g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                      newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                }
                return newImage;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in encoders)
                if (ici.MimeType == mimeType) return ici;

            return null;
        }

    }
}