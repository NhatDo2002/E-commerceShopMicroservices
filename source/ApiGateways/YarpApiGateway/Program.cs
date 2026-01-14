using Microsoft.AspNetCore.RateLimiting;
using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);
//Add services to container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(config =>
{
    config.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 5;
    });
});

var app = builder.Build();
//Configure the HTTP request pipeline.
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
