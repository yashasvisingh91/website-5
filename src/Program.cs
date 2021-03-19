using System.Threading.Tasks;
using Statiq.App;
using Statiq.Docs;

namespace Cake.Website
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args) =>
          await Bootstrapper
            .Factory
            .CreateDocs(args)
            .RunAsync();
    }
}
