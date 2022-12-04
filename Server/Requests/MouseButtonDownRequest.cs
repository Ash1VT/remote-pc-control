using Newtonsoft.Json.Linq;
using Server.DeviceApi;
using Server.Responses;

namespace Server.Requests
{
    public class MouseButtonDownRequest : Request
    {
        private string _button;
        private int _x;
        private int _y;
        public MouseButtonDownRequest(JObject jObject) : base(jObject)
        {
            _button = jObject["Button"].ToString();
            _x = int.Parse(jObject["X"].ToString());
            _y = int.Parse(jObject["Y"].ToString());
        }

        public override void Execute()
        {
            if(_button == "Left")
            {
                MouseApi.Send(_x, _y, Input.MOUSEEVENTF.LEFTDOWN);
                return;
            }
            
            if(_button == "Right")
            {
                MouseApi.Send(_x, _y, Input.MOUSEEVENTF.RIGHTDOWN);
                return;
            }

        }

        public override Response GetResponse()
        {
            return null;
        }
    }
}
