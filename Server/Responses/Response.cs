using Newtonsoft.Json.Linq;

namespace Server.Responses
{
    public abstract class Response
    {

        protected Response()
        {
            
        }

        protected abstract string GetResponseType();

        public virtual JObject ToJson()
        {
            JObject jObject = new JObject();
            jObject.Add("Type", GetResponseType());
            return jObject;
        }
    }
}