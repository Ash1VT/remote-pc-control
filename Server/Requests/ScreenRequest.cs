using System.Drawing;
using Newtonsoft.Json.Linq;
using Server.Responses;

namespace Server.Requests
{
    public class ScreenRequest : Request
    {
        public ScreenRequest(JObject jObject) : base(jObject)
        {
            
        }
        

        public override void Execute()
        {
            Data.Screen = ScreenMaker.Capture(0, 0, 1920, 1080);
        }

        public override Response GetResponse()
        {
            return new ScreenResponse(Data.Screen);
        }
    }
}