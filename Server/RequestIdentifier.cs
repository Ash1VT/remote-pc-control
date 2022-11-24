using Newtonsoft.Json.Linq;
using Server.Requests;

namespace Server
{
    public static class RequestIdentifier
    {
        public static Request GetRequest(JObject jObject)
        {
            string requestType = jObject["Type"].ToString();
            switch (requestType)
            {
                //case "ScreenRequest": return new ScreenRequest(jObject);
                case "MouseCoordinatesRequest": return new MouseCoordinatesRequest(jObject);
                case "Test": return new TestRequest(jObject);
                default: return null;
            }

        }
    }
}