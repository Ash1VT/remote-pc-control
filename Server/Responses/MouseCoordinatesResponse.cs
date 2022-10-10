namespace Server.Responses
{
    public class MouseCoordinatesResponse : Response
    {
        protected override string GetResponseType()
        {
            return "MouseCoordinatesResponse";
        }
    }
}