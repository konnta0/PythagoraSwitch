using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.Player.Interfaces;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Player
{
    public class PsPlayer : IPsPlayer
    {
        private readonly IPsWebRequester _webRequester;
        private readonly IPsImporter _importer;

        public PsPlayer(IPsWebRequester webRequester, IPsImporter importer)
        {
            _webRequester = webRequester;
            _importer = importer;
        }

        public async Task<IErrors> Handle(string path)
        {
            var (contents, error) = _importer.Handle<PsRequestRecordContent>(path);
            if (Errors.IsOccurred(error)) return error;

            foreach (var content in contents)
            {
                
            }
            throw new System.NotImplementedException();
        }
    }
}