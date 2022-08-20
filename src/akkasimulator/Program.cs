using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using akkasimulator.Data;
using Akka.Hosting;
using akkaservice;
using Akka.Actor;
using Petabridge.Cmd.Host;
using Akka.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAkka("akkasimulation", (configurationBuilder, provider) =>
{
    configurationBuilder
        .AddPetabridgeCmd(cmd =>
        {
            
        })
        .WithActors((system, registry) =>
        {
            var instanceCoordinator = system.ActorOf(Props.Create(() => new InstanceCoordinatorActor(provider)), "InstanceCoordinator");
            registry.Register<InstanceCoordinatorActor>(instanceCoordinator);
        });
});

builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
