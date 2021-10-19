using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Test
{
    public class DummyGetRequestContent : IWebGetRequestContent
    {
        public int foobar { get; set; }
    }
    
    public class DummyGetResponseContent : IWebResponseContent
    {
        public string hoge { get; set; }
    }

    public class DummyGetRequest : IWebGet<DummyGetRequestContent, DummyGetResponseContent>
    {
        public Task<(DummyGetResponseContent, IErrors)> HandleRequest(DummyGetRequestContent req)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class DummyPostRequestContent : IWebPostRequestContent
    {
        public int foobar { get; set; }
    }
    
    public class DummyPostResponseContent : IWebResponseContent
    {
        public string hoge { get; set; }
    }

    public class DummyPostRequest : IWebPost<DummyPostRequestContent, DummyPostResponseContent>
    {
        public Task<(DummyPostResponseContent, IErrors)> HandleRequest(DummyPostRequestContent req)
        {
            throw new System.NotImplementedException();
        }
    }
    
}