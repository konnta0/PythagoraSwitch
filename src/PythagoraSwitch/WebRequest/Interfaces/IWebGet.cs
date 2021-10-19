namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebGet<in TReq, TRes> : IWebCommunication<TReq, TRes>
        where TReq : IPsWebGetRequestContent where TRes : IPsWebResponseContent
    {
        
    }
}