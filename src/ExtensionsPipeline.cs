using System.Threading.Tasks;
using Statiq.Common;
using Statiq.Core;
using Statiq.Yaml;

namespace Cake.Website
{
    public class ExtensionsPipeline : Pipeline
    {
        public ExtensionsPipeline()
        {
            InputModules = new ModuleList(
                new ReadFiles("../extensions/*.yml"));

            ProcessModules = new ModuleList((Module)
                new CacheDocuments(
                    new ParseYaml(),
                    new SetMetadata("SupportedCakeVersions", Config.FromDocument(async (doc, ctx) 
                        => ctx.FileSystem.GetInputFile($"../release/extensions/{doc.GetString("NuGet")}.supportedcakeversions").Exists
                            ? await ctx.FileSystem.GetInputFile($"../release/extensions/{doc.GetString("NuGet")}.supportedcakeversions").ReadAllTextAsync()
                            : await Task.FromResult<string>(null))),
                    new SetMetadata(Keys.DestinationPath, Config.FromDocument(doc
                        => new NormalizedPath("extensions/" + doc.GetString("Name").ToLower().Replace(".", "-") + "/index.html"))),
                    new SetMetadata("NoSidebar", true)));
        }
    }
}
