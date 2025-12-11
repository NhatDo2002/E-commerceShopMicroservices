var builder = WebApplication.CreateBuilder(args);

//Adding Services and Depending Injection here
var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("BasketConnectionString");
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");

builder.Services.AddMarten(opt => {
    opt.Connection(connectionString!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddCarter();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Mannually register dependency injection for CachedBasketRepository
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetService<IBasketRepository>();
//    return new CachedBasketRepository(basketRepository!, provider.GetService<IDistributedCache>()!);
//});

//Using Scrutor library for decorate CachedBasketRepository
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = redisConnectionString!;
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!)
    .AddRedis(redisConnectionString!);

var app = builder.Build();

//Adding https pipeline here
app.MapCarter();

app.UseExceptionHandler((opt) => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
