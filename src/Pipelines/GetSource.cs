using Microsoft.Extensions.Logging;
using Statiq.Common;
using Statiq.Core;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cake.Website.Pipelines
{
    public class GetSource : ExecutionPipeline
    {
        public override HashSet<string> Dependencies => new HashSet<string> { nameof(CleanSource) };

        protected override async Task<IEnumerable<IDocument>> ExecuteInputPhaseAsync(IExecutionContext context)
        {
            IDirectory releaseDir = context.FileSystem.GetRootDirectory(context.GetString(SiteKeys.ReleaseDir));     

            // Get the release info
            CakeGitHubReleaseInfo releaseInfo = CakeGitHubReleaseInfo.Get(context, releaseDir);

            // Download and unzip the source
            await DownloadAndUnzipAsync(context, releaseInfo.LatestReleaseZipUrl, releaseDir);
            
            // Need to rename the container directory in the zip file to something consistent
            IDirectory containerDir = releaseDir.GetDirectories().First(x => x.Path.Name.StartsWith("cake"));       
            IDirectory cakeSourceDir = releaseDir.GetDirectory(context.GetString(SiteKeys.CakeSourceDir));
            context.LogInformation($"Moving {containerDir.Path.FullPath} to {cakeSourceDir.Path.FullPath}");
            containerDir.MoveTo(cakeSourceDir);

            // Download cake-contrib Home repository
            await DownloadAndUnzipAsync(context, "https://github.com/cake-contrib/Home/archive/master.zip", releaseDir);
            
            // Need to rename the container directory in the zip file to something consistent
            IDirectory cakeContribContainerDir = releaseDir.GetDirectories().First(x => x.Path.Name.StartsWith("Home-master"));
            IDirectory cakeContribSourceDir = releaseDir.GetDirectory(context.GetString(SiteKeys.CakeContribSourceDir));
            context.LogInformation($"Moving {cakeContribContainerDir} to {cakeContribSourceDir}");
            cakeContribContainerDir.MoveTo(cakeContribSourceDir);

            return null;
        }

        private async Task DownloadAndUnzipAsync(IExecutionContext context, string url, IDirectory directory)
        {            
            context.LogInformation($"Downloading zip file from {url}");
            using (HttpClient httpClient = context.CreateHttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                using (Stream responseStream = await response.Content.ReadAsStreamAsync(context.CancellationToken))
                {
                    using (ZipArchive archive = new ZipArchive(responseStream, ZipArchiveMode.Read))
                    {
                        archive.ExtractToDirectory(directory.Path.FullPath, true);
                    }
                }
            }
        }
    }
}
