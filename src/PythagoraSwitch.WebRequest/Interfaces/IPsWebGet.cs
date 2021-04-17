namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsWebGet<in TReq, TRes> : IPsWebCommunication<TReq, TRes>
        where TReq : IPsWebGetRequestContent where TRes : IPsWebResponseContent
    {
        
    }
}