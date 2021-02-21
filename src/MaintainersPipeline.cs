using Statiq.Common;
using Statiq.Core;
using Statiq.Yaml;

namespace Cake.Website
{
    public class MaintainersPipeline : Pipeline
    {
        public MaintainersPipeline()
        {
            InputModules = new ModuleList(
                new ReadFiles("../maintainers/*.yml"));

            ProcessModules = new ModuleList(
                new ParseYaml());
        }
    }
}
