using Newtonsoft.Json.Linq;

namespace Server.Requests
{
    public abstract class Request
    {
        protected Request(JObject jObject)
        {
            
        }
        
        public abstract void Execute();
        
    }
}