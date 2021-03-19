using Microsoft.Extensions.Logging;
using NuGet.Versioning;
using Octokit;
using Statiq.Common;
using Statiq.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Website
{
    public class CakeGitHubReleaseInfo
    {
        private static CakeGitHubReleaseInfo _cakeGitHubReleaseInfo;

        public static CakeGitHubReleaseInfo Get(IExecutionContext context, IDirectory releaseDir)
        {
            if (_cakeGitHubReleaseInfo is object)
            {
                return _cakeGitHubReleaseInfo;
            }

            // We cache the latest Cake release information in a file to speed up local development and also
            // to decrease the chance of getting rate limited by GitHub when running unauthenticated
            IDirectory cachedGitHubDir = releaseDir.GetDirectory("cake-github");
            IFile cachedReleaseInfoFile = cachedGitHubDir.GetFile($"{nameof(CakeGitHubReleaseInfo)}.yml");
            if (cachedReleaseInfoFile.Exists)
            {
                context.LogDebug($"Retrieving Cake release information from cached file {cachedReleaseInfoFile.Path.FullPath}...");
                _cakeGitHubReleaseInfo = cachedReleaseInfoFile.DeserializeYaml<CakeGitHubReleaseInfo>();
                LogGitHubReleaseInfo(context, _cakeGitHubReleaseInfo);
                return _cakeGitHubReleaseInfo;
            }

            context.LogInformation("Retrieving Cake release information from GitHub...");

            GitHubClient client = new GitHubClient(new ProductHeaderValue("cake-build-website"));

            string gitHubAccessToken = context.GetString("git_access_token");
            if (!string.IsNullOrWhiteSpace(gitHubAccessToken))
            {
                client.Credentials = new Credentials(gitHubAccessToken);
            }

            List<Release> allCakeReleases = client.Repository.Release.GetAll("cake-build", "cake")
                .GetAwaiter()
                .GetResult()
                .Where(r => !r.Draft && r.PublishedAt.HasValue && r.Name.StartsWith("v", StringComparison.Ordinal))
                .OrderByDescending(r => new NuGetVersion(r.Name.Substring(1)))
                .ToList();

            Release latestCakeRelease = allCakeReleases.First();

            _cakeGitHubReleaseInfo = new CakeGitHubReleaseInfo
            {
                LatestReleaseName = latestCakeRelease.Name.TrimStart('v'),
                LatestReleaseUrl = latestCakeRelease.HtmlUrl,
                LatestReleaseZipUrl = latestCakeRelease.ZipballUrl,
            };

            LogGitHubReleaseInfo(context, _cakeGitHubReleaseInfo);

            cachedReleaseInfoFile.SerializeYaml(_cakeGitHubReleaseInfo);

            return _cakeGitHubReleaseInfo;
        }

        private static void LogGitHubReleaseInfo(IExecutionContext context, CakeGitHubReleaseInfo releaseInfo)
        {
            context.LogInformation($"Cake Latest Release Name: {releaseInfo.LatestReleaseName}");
            context.LogInformation($"Cake Latest Release Url: {releaseInfo.LatestReleaseUrl}");
            context.LogInformation($"Cake Latest Release Zip Url: {releaseInfo.LatestReleaseZipUrl}");
        }

        public string LatestReleaseName { get; set; }

        public string LatestReleaseUrl { get; set; }

        public string LatestReleaseZipUrl { get; set; }
    }
}
