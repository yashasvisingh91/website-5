using System.Threading.Tasks;
using Statiq.App;
using Statiq.Web;

namespace Cake.Website
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            return await Bootstrapper.Factory
                .CreateWeb(args)
                .RunAsync();
        }
    }
}
