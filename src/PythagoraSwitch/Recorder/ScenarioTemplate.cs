using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    internal sealed class ScenarioTemplate : IScenarioTemplate
    {
        public string GetSyntax => @"
// This file generated. Do not edit
namespace PythagoraSwitch.Recorder.Scenario
{
    IList<
    public class {{scenario_name}} : IScenario {
    }

{{for content in contents}}
    public class {{request_content_type_to_string content.request_content_type}} : {{content_implement_interface_string content.method}} {
}
{{end}}
}
";
    }
}