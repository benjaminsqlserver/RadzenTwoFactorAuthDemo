using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using HouseholdAppliancesApp.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<HouseholdAppliancesApp.Client.ConDataService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("HouseholdAppliancesApp.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HouseholdAppliancesApp.Server"));
builder.Services.AddScoped<HouseholdAppliancesApp.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, HouseholdAppliancesApp.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();