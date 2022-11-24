using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Requests
{
    internal class TestRequest : Request
    {
        public override string GetRequestType()
        {
            return "Test";
        }
    }
}
