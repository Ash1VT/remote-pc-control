using Newtonsoft.Json.Linq;

namespace Client.Requests
{
    public abstract class Request
    {
        protected Request()
        {
            
        }

        public abstract string GetRequestType();

        public virtual JObject ToJson()
        {
            JObject jObject = new JObject();
            jObject.Add("Type", GetRequestType());
            return jObject;
        }

    }
}