﻿using Client.Responses;
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
                default: return null;
            }

        }
    }
}
