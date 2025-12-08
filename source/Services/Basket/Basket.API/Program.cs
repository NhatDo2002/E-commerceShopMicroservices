var builder = WebApplication.CreateBuilder(args);

//Adding Services and Depending Injection here
var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("BasketConnectionString");

//builder.Services.AddMarten(opt => opt.Connection(connectionString!));
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddCarter();

var app = builder.Build();

//Adding https pipeline here
app.MapCarter();

app.Run();
