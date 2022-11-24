using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Responses
{
    internal class TestResponse : Response
    {
        protected override string GetResponseType()
        {
            return "Test";
        }
    }
}
