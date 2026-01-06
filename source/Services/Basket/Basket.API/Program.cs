using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;

var builder = WebApplication.CreateBuilder(args);

//Adding Services and Depending Injection here

//Get configuration values
var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("BasketConnectionString");
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");

// - Application Services

//Add Carter for minimal APIs
builder.Services.AddCarter();

//Add MediatR and register cross-cutting concerns behaviors
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// - Data Services

//Add MartenDB as document database
builder.Services.AddMarten(opt => {
    opt.Connection(connectionString!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

//Register BasketRepository and CachedBasketRepository dependency
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Mannually register dependency injection for CachedBasketRepository
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetService<IBasketRepository>();
//    return new CachedBasketRepository(basketRepository!, provider.GetService<IDistributedCache>()!);
//});

//Using Scrutor library for decorate CachedBasketRepository
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//Add Redis distributed cache
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = redisConnectionString!;
});

//Add Grpc client for Discount.Grpc service
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    option =>
    {
        option.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    }
).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

//Add Message Broker - MassTransit with RabbitMQ
builder.Services.AddMessageBroker(builder.Configuration);

//Add cross-cutting concerns middleware
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!)
    .AddRedis(redisConnectionString!);

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

//Adding https pipeline here
app.MapCarter();

app.UseExceptionHandler((opt) => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
