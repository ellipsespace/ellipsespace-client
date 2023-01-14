using Octokit;

namespace EllipseSpaceClient.Core.Version
{
    public static class GitAPI
    {
        public static string GetLatestTag()
        {
            var github = new GitHubClient(new ProductHeaderValue("EllipseSpaceClient"));
            var tag = github.Repository.GetAllTags("ellipsespace", "EllipseSpace-client").Result;

            return tag[tag.Count - 1].Name;
        }

        public static string MakeAddress(string relative)
        {
            return "https://github.com/" + relative;
        }
    }
}
