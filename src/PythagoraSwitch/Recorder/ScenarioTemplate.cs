using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    internal sealed class ScenarioTemplate : IScenarioTemplate
    {
        public string GetSyntax => @"
// This file generated. Do not edit
namespace PythagoraSwitch.Recorder.Scenario
{
    public class {{scenario_name}} : ScenarioAbstract {
        public {{scenario_name}} 
        {
{{~for content in contents~}}
            Order.Add(_{{for.index + 1}});
{{~end~}}
        }

{{for content in contents}}
        private async Task<(string, IErrors)> _{{for.index + 1}}(Uri uri, IOption option)
        {
            var content = new HogeGetContent();
            var (response, errors) = await _webRequestHandler.GetAsync<HogeGetContent, IWebResponseContent>(uri, content, option);
            if (Errors.IsOccurred(errors)) return (string.Empty, errors);
            return (System.Text.Json.JsonSerializer.Serialize(response), Errors.Nothing());
        }
{{end}}
    }

{{for content in contents}}
    public class {{request_content_type_to_string content.request_content_type}} : {{content_implement_interface_string content.method}} {
    }
{{end}}
}
";
    }
}