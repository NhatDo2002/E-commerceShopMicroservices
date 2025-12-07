var builder = WebApplication.CreateBuilder(args);

//Adding Services and Depending Injection here


var app = builder.Build();

//Adding https pipeline here

app.Run();
