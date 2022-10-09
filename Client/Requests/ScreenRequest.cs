using Newtonsoft.Json.Linq;

namespace Client.Requests
{
    public class ScreenRequest : Request
    {
        
        
        public override string GetRequestType()
        {
            return "ScreenRequest";
        }
        
        public override JObject ToJson()
        {
            return base.ToJson();
        }
        
    }
}