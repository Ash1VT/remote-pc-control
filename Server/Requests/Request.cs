using Newtonsoft.Json.Linq;
using Server.Responses;

namespace Server.Requests
{
    public abstract class Request
    {
        protected Request(JObject jObject)
        {
            
        }
        
        public abstract void Execute();

        public abstract Response GetResponse();

    }
}