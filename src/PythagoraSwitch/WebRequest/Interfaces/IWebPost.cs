namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebPost<in TReq, TRes> : IWebCommunication<TReq, TRes>
        where TReq : IPsWebPostRequestContent where TRes : IPsWebResponseContent
    {
        
    }
}