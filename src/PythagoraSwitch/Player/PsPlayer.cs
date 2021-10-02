using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.Player.Interfaces;
using PythagoraSwitch.Recorder.Interfaces;
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

        public async Task<IErrors> Handle<T>(string path) where T : IPsRequestRecordContent
        {
            var (contents, error) = _importer.Handle<T>(path);
            if (Errors.IsOccurred(error)) return error;

            foreach (var content in contents)
            {
                await Task.Delay(1);
            }

            return Errors.Nothing();
        }
    }
}