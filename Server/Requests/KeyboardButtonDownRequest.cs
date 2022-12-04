using Newtonsoft.Json.Linq;
using Server.DeviceApi;
using Server.Input;
using Server.Responses;
using System;

namespace Server.Requests
{
    public class KeyboardButtonDownRequest : Request
    {
        private VirtualKeyShort _buttonCode;
        public KeyboardButtonDownRequest(JObject jObject) : base(jObject)
        {
            _buttonCode = (VirtualKeyShort)Enum.Parse(typeof(VirtualKeyShort),jObject["ButtonCode"].ToString());
        }

        public override void Execute()
        {

            KeyboardApi.SendKeyDown(_buttonCode);
        }

        public override Response GetResponse()
        {
            return null;
        }
    }
}
