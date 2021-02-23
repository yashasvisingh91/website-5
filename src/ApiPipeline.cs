using Statiq.Core;
using Statiq.Common;
using Statiq.CodeAnalysis;
using System.Linq;
using Statiq.Razor;

namespace Cake.Website
{
    public class ApiPipeline : Pipeline
    {
        public ApiPipeline()
        {
            Dependencies.Add(nameof(ExtensionsPipeline));

            InputModules = new ModuleList(
                new ReadFiles("../release/cake-repo/src/**/{!bin,!obj,!packages,!*.Tests,}/**/*.cs"));

            ProcessModules = new ModuleList((Module)
                new CacheDocuments(
                    new AnalyzeCSharp() // Put analysis module inside execute to have access to global metadata at runtime
                        .WhereNamespaces(includeGlobal: false)
                        .WherePublic()
                        .WithAssemblies(Config.FromContext(ctx =>
                        {
                            var extensions = ctx.Outputs.FromPipeline(nameof(ExtensionsPipeline));

                            var assemblies = extensions
                                .Where(extension => extension.ContainsKey("Assemblies"))
                                .SelectMany(extension => extension
                                    .GetList<string>("Assemblies")
                                    .Select(assembly =>
                                        $"../release/extensions/{extension.GetString("NuGet").ToLower()}.{extension.GetString("AnalyzedPackageVersion")}{assembly}"));

                            return assemblies;
                        }))
                        .WithCssClasses("code", "cs")
                        .WithDestinationPrefix("api/")
                        .WithAssemblySymbols()
                        .WithImplicitInheritDoc(true)));
        }
    }

    public class ApiIndexPipeline : Pipeline
    {
        public ApiIndexPipeline()
        {
            Dependencies.Add(nameof(ApiPipeline));

            InputModules = new ModuleList(
                new ReadFiles("_ApiIndex.cshtml"));

            ProcessModules = new ModuleList(
                new SetDestination("api/index.html"),
                new SetMetadata(Keys.Title, "API"));

            PostProcessModules = new ModuleList(
                new RenderRazor()
                    .IgnorePrefix(null));

            OutputModules = new ModuleList(
                new WriteFiles());
        }
    }

    public class RenderApiPipeline : Pipeline
    {
        public RenderApiPipeline()
        {
            Dependencies.Add(nameof(ApiPipeline));

            PostProcessModules = new ModuleList(
                new ConcatDocuments(nameof(ApiPipeline)),
                new RenderRazor()
                    .WithLayout("/_ApiLayout.cshtml")
                );
        }
    }
}
