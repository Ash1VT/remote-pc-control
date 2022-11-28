using Newtonsoft.Json.Linq;

namespace Client.Requests;

public class MouseDownRequest : Request
{
    private int _x;
    private int _y;
    private string _button;
    
    public MouseDownRequest(int x, int y, string button)
    {
        _x = x;
        _y = y;
        _button = button;
    }

    public override string GetRequestType()
    {
        return "MouseButtonDownRequest";
    }
    
    public override JObject ToJson()
    {
        JObject jObject = base.ToJson();
        jObject.Add("X", _x.ToString());
        jObject.Add("Y", _y.ToString());
        jObject.Add("Button", _button);
        return jObject;
    }
}