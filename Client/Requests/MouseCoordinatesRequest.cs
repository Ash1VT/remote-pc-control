using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Client.Requests
{
    class MouseCoordinatesRequest : Request
    {

        private int _x;
        private int _y;

        public MouseCoordinatesRequest(int x, int y) : base()
        {
            _x = x;
            _y = y;
        }
        public override string GetRequestType()
        {
            return "MouseCoordinatesRequest";
        }

        public override JObject ToJson()
        {
            JObject jObject = base.ToJson();
            jObject.Add("X", _x.ToString());
            jObject.Add("Y", _y.ToString());
            return jObject;
        }
    }
}
