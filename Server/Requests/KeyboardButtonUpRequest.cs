using Newtonsoft.Json.Linq;
using Server.DeviceApi;
using Server.Input;
using Server.Responses;
using System;

namespace Server.Requests
{
    public class KeyboardButtonUpRequest : Request
    {
        private VirtualKeyShort _buttonCode;
        public KeyboardButtonUpRequest(JObject jObject) : base(jObject)
        {
            _buttonCode = (VirtualKeyShort)Enum.Parse(typeof(VirtualKeyShort), jObject["ButtonCode"].ToString());
        }

        public override void Execute()
        {
            KeyboardApi.SendKeyUp(_buttonCode);
        }

        public override Response GetResponse()
        {
            return null;
        }
    }
}
