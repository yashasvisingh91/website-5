using Statiq.Common;
using Statiq.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cake.Website.Pipelines
{
    public class CleanSource : ExecutionPipeline
    {
        protected override Task<IEnumerable<IDocument>> ExecuteInputPhaseAsync(IExecutionContext context)
        {
            IDirectory releaseDir = context.FileSystem.GetRootDirectory(context.GetString(SiteKeys.ReleaseDir));
            
            IDirectory cakeSourceDir = releaseDir.GetDirectory(context.GetString(SiteKeys.CakeSourceDir));
            if (cakeSourceDir.Exists)
            {
                cakeSourceDir.Delete(true);
            }
            
            IDirectory cakeContribSourceDir = releaseDir.GetDirectory(context.GetString(SiteKeys.CakeContribSourceDir));
            if (cakeContribSourceDir.Exists)
            {
                cakeContribSourceDir.Delete(true);
            }

            foreach (IDirectory cakeDir in releaseDir.GetDirectories().Where(x => x.Path.Name.StartsWith("cake")))
            {
                cakeDir.Delete(true);
            }

            return Task.FromResult<IEnumerable<IDocument>>(null);
        }
    }
}
