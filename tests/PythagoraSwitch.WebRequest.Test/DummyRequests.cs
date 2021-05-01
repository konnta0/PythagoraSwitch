using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest.Test
{
    public class DummyGetRequestContent : IPsWebGetRequestContent
    {
        public int foobar { get; set; }
    }
    
    public class DummyGetResponseContent : IPsWebResponseContent
    {
        public string hoge { get; set; }
    }

    public class DummyGetRequest : IPsWebGet<DummyGetRequestContent, DummyGetResponseContent>
    {
        public Task<(DummyGetResponseContent, IErrors)> HandleRequest(DummyGetRequestContent req)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class DummyPostRequestContent : IPsWebPostRequestContent
    {
        public int foobar { get; set; }
    }
    
    public class DummyPostResponseContent : IPsWebResponseContent
    {
        public string hoge { get; set; }
    }

    public class DummyPostRequest : IPsWebPost<DummyPostRequestContent, DummyPostResponseContent>
    {
        public Task<(DummyPostResponseContent, IErrors)> HandleRequest(DummyPostRequestContent req)
        {
            throw new System.NotImplementedException();
        }
    }
    
}