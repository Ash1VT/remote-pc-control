using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Responses
{
    internal class TestResponse : Response
    {
        public TestResponse(JObject jObject) : base(jObject)
        {
            Console.WriteLine(jObject["Type"].ToString());
        }

        public override void Execute()
        {
            Console.WriteLine("Hello");
        }
    }
}
