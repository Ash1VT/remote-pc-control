using Newtonsoft.Json.Linq;
using Server.Responses;

namespace Server.Requests
{
    public class MouseCoordinatesRequest : Request
    {
        private int _x;
        private int _y;
        
        public MouseCoordinatesRequest(JObject jObject) : base(jObject)
        {
            _x = int.Parse(jObject["X"].ToString());
            _y = int.Parse(jObject["Y"].ToString());
        }

        public override void Execute()
        {
            User32.SetCursorPos((int)(_x), (int)(_y));
        }

        public override Response GetResponse()
        {
            return null;
        }
    }
}