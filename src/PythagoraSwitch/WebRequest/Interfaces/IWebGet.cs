namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebGet<in TReq, TRes> : IWebCommunication<TReq, TRes>
        where TReq : IWebGetRequestContent where TRes : IWebResponseContent
    {
        
    }
}