using Newtonsoft.Json.Linq;
using Server.DeviceApi;
using Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Requests
{
    public class MouseButtonUpRequest : Request
    {
        private string _button;
        private int _x;
        private int _y;
        public MouseButtonUpRequest(JObject jObject) : base(jObject)
        {
            _button = jObject["Button"].ToString();
            _x = int.Parse(jObject["X"].ToString());
            _y = int.Parse(jObject["Y"].ToString());
        }

        public override void Execute()
        {
            if (_button == "Left")
            {
                MouseApi.Send(_x, _y, Input.MOUSEEVENTF.LEFTUP);
                //User32.mouse_event(User32.MouseEventFlags.ABSOLUTE | User32.MouseEventFlags.LEFTUP, _x, _y, 0, UIntPtr.Zero);
                return;
            }

            if (_button == "Right")
            {
                MouseApi.Send(_x, _y, Input.MOUSEEVENTF.RIGHTUP);
                //User32.mouse_event(User32.MouseEventFlags.ABSOLUTE | User32.MouseEventFlags.RIGHTUP, _x, _y, 0, UIntPtr.Zero);
                return;
            }
        }

        public override Response GetResponse()
        {
            return null;
        }
    }
}
