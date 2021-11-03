namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebPost<in TReq, TRes> : IWebCommunication<TReq, TRes>
        where TReq : IWebPostRequestContent where TRes : IWebResponseContent
    {
        
    }
}