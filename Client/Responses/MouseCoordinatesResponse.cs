using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Client.Responses
{
    class MouseCoordinatesResponse : Response
    {
        public MouseCoordinatesResponse(JObject jObject) : base(jObject)
        {
        }

        public override void Execute()
        {
            
        }
    }
}
