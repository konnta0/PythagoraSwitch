using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    internal sealed class ScenarioTemplate : IScenarioTemplate
    {
        public string GetSyntax => 
            @"<ul id='products'>
  {{ for product in products }}
    <li>
      <h2>{{ product.name }}</h2>
           Price: {{ product.price }}
           {{ product.description | string.truncate 15 }}
    </li>
  {{ end }}
</ul>
";
    }
}