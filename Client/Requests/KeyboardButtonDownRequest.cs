using System;
using Newtonsoft.Json.Linq;

namespace Client.Requests;

public class KeyboardButtonDownRequest : Request
{
    private Int32 _buttonCode; 

    public KeyboardButtonDownRequest(Int32 buttonCode) : base()
    {
        _buttonCode = buttonCode;
    }

    public override JObject ToJson()
    {
        JObject jObject = base.ToJson();
        jObject.Add("ButtonCode", _buttonCode.ToString());
        return jObject;
    }
    public override string GetRequestType()
    {
        return "KeyboardButtonDownRequest";
    }
}