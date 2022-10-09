using Newtonsoft.Json.Linq;

namespace Client.Responses
{
    public abstract class Response
    {

        protected Response(JObject jObject)
        {
            
        }
        public abstract void Execute();
    }
}