namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsWebPost<in TReq, TRes> : IPsWebCommunication<TReq, TRes>
        where TReq : IPsWebPostRequestContent where TRes : IPsWebResponseContent
    {
        
    }
}