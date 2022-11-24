using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Responses;
using Newtonsoft.Json.Linq;

namespace Client
{
    public static class ResponseIdentifier
    {

        public static Response GetResponse(JObject jObject)
        {
            string responseType = jObject["Type"].ToString();
            switch (responseType)
            {
                case "ScreenResponse": return new ScreenResponse(jObject);
                case "MouseCoordinatesResponse": return new MouseCoordinatesResponse(jObject);
                case "Test": return new TestResponse(jObject);
                default: return null;
            }

        }
    }
}
