

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DiscountDbDatabase");
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(opt =>
{
    opt.UseSqlite(connectionString);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseMigration();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
