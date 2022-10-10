using System.Windows.Forms;
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
            User32.SetCursorPos((int)(_x / 1.25), (int)(_y / 1.25));
        }

        public override Response GetResponse()
        {
            return new MouseCoordinatesResponse();
        }
    }
}