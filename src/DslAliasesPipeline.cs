using Statiq.Common;
using Statiq.Core;

namespace Cake.Website
{
    public class DslAliasesPipeline : Pipeline
    {
        public DslAliasesPipeline()
        {
            InputModules = new ModuleList(
                new ReadFiles("*.dll"));
        }
    }
}
