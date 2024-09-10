using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using ChatAppBlazorStand.Pages;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        // Register root component (App) to the app
        builder.RootComponents.Add<Instagram>("#app");

        // Register HttpClient with a base address (API for communication)
        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7153/")
        });

        // Build and run the WebAssembly app
        await builder.Build().RunAsync();
    }
}
