using Newtonsoft.Json.Linq;
using Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Requests
{
    internal class TestRequest : Request
    {
        public TestRequest(JObject jObject) : base(jObject)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Hi");
        }

        public override Response GetResponse()
        {
            return new TestResponse();
        }
    }
}
