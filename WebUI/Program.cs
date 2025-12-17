using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebUI.Components;
using WebUI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(c => c.DetailedErrors = true);

builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7059"); // урл апи
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapGet("/docs", async context =>
{
    context.Response.Redirect("/docs.html");
});

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();  
