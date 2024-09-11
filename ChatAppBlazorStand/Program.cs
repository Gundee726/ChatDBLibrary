using ChatAppBlazorStand;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        // Register the root component (App) to the app
        builder.RootComponents.Add<App>("#app");

        // Register HttpClient with a base address for API communication
        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7153/")  // Ensure this matches your API base address
        });

        // Build and run the WebAssembly app
        await builder.Build().RunAsync();
    }
}
